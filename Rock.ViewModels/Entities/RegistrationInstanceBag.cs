//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Rock.CodeGeneration project
//     Changes to this file will be lost when the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
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
using System.Linq;

using Rock.ViewModels.Utility;

namespace Rock.ViewModels.Entities
{
    /// <summary>
    /// RegistrationInstance View Model
    /// </summary>
    public partial class RegistrationInstanceBag : EntityBagBase
    {
        /// <summary>
        /// Gets or sets the AccountId.
        /// </summary>
        /// <value>
        /// The AccountId.
        /// </value>
        public int? AccountId { get; set; }

        /// <summary>
        /// Gets or sets the AdditionalConfirmationDetails.
        /// </summary>
        /// <value>
        /// The AdditionalConfirmationDetails.
        /// </value>
        public string AdditionalConfirmationDetails { get; set; }

        /// <summary>
        /// Gets or sets the AdditionalReminderDetails.
        /// </summary>
        /// <value>
        /// The AdditionalReminderDetails.
        /// </value>
        public string AdditionalReminderDetails { get; set; }

        /// <summary>
        /// Gets or sets the ContactEmail.
        /// </summary>
        /// <value>
        /// The ContactEmail.
        /// </value>
        public string ContactEmail { get; set; }

        /// <summary>
        /// Gets or sets the ContactPersonAliasId.
        /// </summary>
        /// <value>
        /// The ContactPersonAliasId.
        /// </value>
        public int? ContactPersonAliasId { get; set; }

        /// <summary>
        /// Gets or sets the ContactPhone.
        /// </summary>
        /// <value>
        /// The ContactPhone.
        /// </value>
        public string ContactPhone { get; set; }

        /// <summary>
        /// Gets or sets the Cost.
        /// </summary>
        /// <value>
        /// The Cost.
        /// </value>
        public decimal? Cost { get; set; }

        /// <summary>
        /// Gets or sets the DefaultPayment.
        /// </summary>
        /// <value>
        /// The DefaultPayment.
        /// </value>
        public decimal? DefaultPayment { get; set; }

        /// <summary>
        /// Gets or sets the Details.
        /// </summary>
        /// <value>
        /// The Details.
        /// </value>
        public string Details { get; set; }

        /// <summary>
        /// Gets or sets the EndDateTime.
        /// </summary>
        /// <value>
        /// The EndDateTime.
        /// </value>
        public DateTime? EndDateTime { get; set; }

        /// <summary>
        /// Gets or sets the ExternalGatewayFundId.
        /// </summary>
        /// <value>
        /// The ExternalGatewayFundId.
        /// </value>
        public int? ExternalGatewayFundId { get; set; }

        /// <summary>
        /// Gets or sets the ExternalGatewayMerchantId.
        /// </summary>
        /// <value>
        /// The ExternalGatewayMerchantId.
        /// </value>
        public int? ExternalGatewayMerchantId { get; set; }

        /// <summary>
        /// Gets or sets the IsActive.
        /// </summary>
        /// <value>
        /// The IsActive.
        /// </value>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the MaxAttendees.
        /// </summary>
        /// <value>
        /// The MaxAttendees.
        /// </value>
        public int? MaxAttendees { get; set; }

        /// <summary>
        /// Gets or sets the MinimumInitialPayment.
        /// </summary>
        /// <value>
        /// The MinimumInitialPayment.
        /// </value>
        public decimal? MinimumInitialPayment { get; set; }

        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        /// <value>
        /// The Name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the RegistrationInstructions.
        /// </summary>
        /// <value>
        /// The RegistrationInstructions.
        /// </value>
        public string RegistrationInstructions { get; set; }

        /// <summary>
        /// Gets or sets the RegistrationMeteringThreshold.
        /// </summary>
        /// <value>
        /// The RegistrationMeteringThreshold.
        /// </value>
        public int? RegistrationMeteringThreshold { get; set; }

        /// <summary>
        /// Gets or sets the RegistrationTemplateId.
        /// </summary>
        /// <value>
        /// The RegistrationTemplateId.
        /// </value>
        public int RegistrationTemplateId { get; set; }

        /// <summary>
        /// Gets or sets the RegistrationWorkflowTypeId.
        /// </summary>
        /// <value>
        /// The RegistrationWorkflowTypeId.
        /// </value>
        public int? RegistrationWorkflowTypeId { get; set; }

        /// <summary>
        /// Gets or sets the ReminderSent.
        /// </summary>
        /// <value>
        /// The ReminderSent.
        /// </value>
        public bool ReminderSent { get; set; }

        /// <summary>
        /// Gets or sets the SendReminderDateTime.
        /// </summary>
        /// <value>
        /// The SendReminderDateTime.
        /// </value>
        public DateTime? SendReminderDateTime { get; set; }

        /// <summary>
        /// Gets or sets the StartDateTime.
        /// </summary>
        /// <value>
        /// The StartDateTime.
        /// </value>
        public DateTime? StartDateTime { get; set; }

        /// <summary>
        /// Gets or sets the TimeoutIsEnabled.
        /// </summary>
        /// <value>
        /// The TimeoutIsEnabled.
        /// </value>
        public bool TimeoutIsEnabled { get; set; }

        /// <summary>
        /// Gets or sets the TimeoutLengthMinutes.
        /// </summary>
        /// <value>
        /// The TimeoutLengthMinutes.
        /// </value>
        public int? TimeoutLengthMinutes { get; set; }

        /// <summary>
        /// Gets or sets the TimeoutThreshold.
        /// </summary>
        /// <value>
        /// The TimeoutThreshold.
        /// </value>
        public int? TimeoutThreshold { get; set; }

        /// <summary>
        /// Gets or sets the CreatedDateTime.
        /// </summary>
        /// <value>
        /// The CreatedDateTime.
        /// </value>
        public DateTime? CreatedDateTime { get; set; }

        /// <summary>
        /// Gets or sets the ModifiedDateTime.
        /// </summary>
        /// <value>
        /// The ModifiedDateTime.
        /// </value>
        public DateTime? ModifiedDateTime { get; set; }

        /// <summary>
        /// Gets or sets the CreatedByPersonAliasId.
        /// </summary>
        /// <value>
        /// The CreatedByPersonAliasId.
        /// </value>
        public int? CreatedByPersonAliasId { get; set; }

        /// <summary>
        /// Gets or sets the ModifiedByPersonAliasId.
        /// </summary>
        /// <value>
        /// The ModifiedByPersonAliasId.
        /// </value>
        public int? ModifiedByPersonAliasId { get; set; }

    }
}
