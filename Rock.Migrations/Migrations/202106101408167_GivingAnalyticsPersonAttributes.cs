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
using System.Collections.Generic;

namespace Rock.Migrations
{   
    /// <summary>
    ///
    /// </summary>
    public partial class GivingAnalyticsPersonAttributes : Rock.Migrations.RockMigration
    {
        /// <summary>
        /// Operations to be performed during the upgrade process.
        /// </summary>
        public override void Up()
        {
            var givingAnalyticsCategory = new List<string>() { SystemGuid.Category.PERSON_ATTRIBUTES_GIVING_ANALYTICS };

            // Person Attribute "Giving History JSON"
            RockMigrationHelper.AddOrUpdatePersonAttributeByGuid( 
                SystemGuid.FieldType.CODE_EDITOR,
                givingAnalyticsCategory, 
                "Giving History JSON", 
                "",
                "GivingHistoryJson", 
                "", 
                "", 
                2000, 
                "", 
                SystemGuid.Attribute.PERSON_GIVING_HISTORY_JSON );

            // Person Attribute "Last 12 Month Giving"
            RockMigrationHelper.AddOrUpdatePersonAttributeByGuid(
                SystemGuid.FieldType.CURRENCY,
                givingAnalyticsCategory,
                "Last 12 Months Giving",
                "",
                "LastTwelveMonthsGiving",
                "",
                "",
                2001,
                "",
                SystemGuid.Attribute.PERSON_GIVING_12_MONTHS );

            // Person Attribute "Last 90 Days Giving"
            RockMigrationHelper.AddOrUpdatePersonAttributeByGuid(
                SystemGuid.FieldType.CURRENCY,
                givingAnalyticsCategory,
                "Last 90 Days Giving",
                "",
                "LastNinetyDaysGiving",
                "",
                "",
                2002,
                "",
                SystemGuid.Attribute.PERSON_GIVING_90_DAYS );

            // Person Attribute "Prior 90 Days Giving"
            RockMigrationHelper.AddOrUpdatePersonAttributeByGuid(
                SystemGuid.FieldType.CURRENCY,
                givingAnalyticsCategory,
                "Prior 90 Days Giving",
                "",
                "PriorNinetyDaysGiving",
                "",
                "",
                2003,
                "",
                SystemGuid.Attribute.PERSON_GIVING_PRIOR_90_DAYS );

            // Person Attribute "Last 12 Month Gift Count"
            RockMigrationHelper.AddOrUpdatePersonAttributeByGuid(
                SystemGuid.FieldType.INTEGER,
                givingAnalyticsCategory,
                "Last 12 Months Gift Count",
                "",
                "LastTwelveMonthsGiftCount",
                "",
                "",
                2004,
                "",
                SystemGuid.Attribute.PERSON_GIVING_12_MONTHS_COUNT );

            // Person Attribute "Last 90 Day Gift Count"
            RockMigrationHelper.AddOrUpdatePersonAttributeByGuid(
                SystemGuid.FieldType.INTEGER,
                givingAnalyticsCategory,
                "Last 90 Days Gift Count",
                "",
                "LastNinetyDaysGiftCount",
                "",
                "",
                2005,
                "",
                SystemGuid.Attribute.PERSON_GIVING_90_DAYS_COUNT );
        }

        /// <summary>
        /// Operations to be performed during the downgrade process.
        /// </summary>
        public override void Down()
        {
        }
    }
}
