// <copyright>
// Copyright by the Spark Development Network
//
// Licensed under the Rock Community License (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.rockrms.com/license
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
//
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web.UI;

using Rock;
using Rock.Attribute;
using Rock.Data;
using Rock.Model;
using Rock.SystemKey;
using Rock.Utility.Settings.GivingAnalytics;
using Rock.Web.Cache;

namespace RockWeb.Blocks.Utility
{
    /// <summary>
    /// Template block for developers to use to start a new block.
    /// </summary>
    [DisplayName( "Stark Detail" )]
    [Category( "Utility" )]
    [Description( "Template block for developers to use to start a new detail block." )]

    #region Block Attributes

    [BooleanField(
        "Show Email Address",
        Key = AttributeKey.ShowEmailAddress,
        Description = "Should the email address be shown?",
        DefaultBooleanValue = true,
        Order = 1 )]

    [EmailField(
        "Email",
        Key = AttributeKey.Email,
        Description = "The Email address to show.",
        DefaultValue = "ted@rocksolidchurchdemo.com",
        Order = 2 )]

    #endregion Block Attributes
    public partial class StarkDetail : Rock.Web.UI.RockBlock
    {

        #region Attribute Keys

        private static class AttributeKey
        {
            public const string ShowEmailAddress = "ShowEmailAddress";
            public const string Email = "Email";
        }

        #endregion Attribute Keys

        #region PageParameterKeys

        private static class PageParameterKey
        {
            public const string StarkId = "StarkId";
        }

        #endregion PageParameterKeys

        #region Fields

        // Used for private variables.

        #endregion

        #region Properties

        // Used for public / protected properties.

        #endregion

        #region Base Control Methods

        // Overrides of the base RockBlock methods (i.e. OnInit, OnLoad)

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnInit( EventArgs e )
        {
            base.OnInit( e );

            // This event gets fired after block settings are updated. It's nice to repaint the screen if these settings would alter it.
            this.BlockUpdated += Block_BlockUpdated;
            this.AddConfigurationUpdateTrigger( upnlContent );
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load" /> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnLoad( EventArgs e )
        {
            base.OnLoad( e );


            if ( !Page.IsPostBack )
            {
                //ShowPluginDllsReport();
                ProcessGivingJourneys();
            }
        }

        /// <summary>
        /// Processes the giving journeys.
        /// </summary>
        private void ProcessGivingJourneys()
        {

            var givingAnalyticsSetting = GetGivingAnalyticsSettings();
            var transctionType = givingAnalyticsSetting.TransactionTypeGuids.Select( a => DefinedValueCache.Get( a ) );

            var rockContext = new RockContext();
            var personService = new PersonService( rockContext );

            // TODO Include Businesses?
            var personQuery = personService.Queryable();

            var personAliasService = new PersonAliasService( rockContext );
            var personAliasQuery = personAliasService.Queryable();
            var financialTransactionService = new FinancialTransactionService( rockContext );
            var financialTransactionGivingAnalyticsQuery = financialTransactionService.GetGivingAnalyticsSourceTransactionQuery();

            rockContext.SqlLogging( true );

            /* Get Non-Giver GivingIds */
            var nonGiverGivingIdsQuery = personQuery.Where( p => financialTransactionGivingAnalyticsQuery.Any( ft => !personAliasQuery.Any( pa => pa.Id == ft.AuthorizedPersonAliasId && pa.PersonId == p.Id ) ) );
            var nonGiverGivingIdsList = nonGiverGivingIdsQuery.Select( a => a.GivingId ).ToList().Distinct().ToList();

            /* Get TransactionDateList for each GivingId in the system */
            var transactionDateTimes = financialTransactionGivingAnalyticsQuery.Select( a => new
            {
                GivingId = personAliasQuery.Where( pa => pa.Id == a.AuthorizedPersonAliasId ).Select( pa => pa.Person.GivingId ).FirstOrDefault(),
                a.TransactionDateTime
            } ).ToList();

            var transactionDateTimesByGivingId = transactionDateTimes
                    .GroupBy( g => g.GivingId )
                    .Select( s => new
                    {
                        GivingId = s.Key,
                        TransactionDateTimeList = s.Select( x => x.TransactionDateTime ).ToList()
                    } ).ToDictionary( k => k.GivingId, v => v.TransactionDateTimeList );


            // TODO...
        }

        

        /// <summary>
        /// Gets the giving analytics settings.
        /// </summary>
        /// <returns></returns>
        private static GivingAnalyticsSetting GetGivingAnalyticsSettings()
        {
            return Rock.Web.SystemSettings
                .GetValue( SystemSetting.GIVING_ANALYTICS_CONFIGURATION )
                .FromJsonOrNull<GivingAnalyticsSetting>() ?? new GivingAnalyticsSetting();
        }

        private void ShowPluginDllsReport()
        {
            var pluginDlls = Reflection.GetPluginAssemblies().Where( a => !a.GetName().Name.StartsWith( "Rock." ) && !a.GetName().Name.StartsWith( "App_Code")  );
            var stringBuilderPluginReport = new StringBuilder();
            foreach ( var pluginDll in pluginDlls.OrderBy( a => a.FullName ) )
            {
                var rockReference = pluginDll.GetReferencedAssemblies().Where( a => a.Name == "Rock"  ).FirstOrDefault();
                stringBuilderPluginReport.AppendLine( $@"
<div class='row'>
    <div class='col-md-6'>
        <span>{pluginDll.GetName().Name}</span>
    </div>
    <div class='col-md-6'>
        Rock.Version: {rockReference?.Version}
    </div>
</div>" );
            }

            stringBuilderPluginReport.AppendLine( $@"
<div class='row'>
    <div class='col-md-6'>
        <span class='label label-warning'>bad.knownbadpluginname.dll</span>
    </div>
    <div class='col-md-6'>
        Rock.Version: 1.4.0.1
    </div>
</div>" );

            lPluginDllsReport.Text = $"{stringBuilderPluginReport.ToString()}";
        }

        #endregion

        #region Events

        // Handlers called by the controls on your block.

        /// <summary>
        /// Handles the BlockUpdated event of the control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Block_BlockUpdated( object sender, EventArgs e )
        {

        }

        #endregion

        #region Methods

        // helper functional methods (like BindGrid(), etc.)

        #endregion
    }
}