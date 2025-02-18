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
    /// InteractionChannel View Model
    /// </summary>
    public partial class InteractionChannelBag : EntityBagBase
    {
        /// <summary>
        /// Gets or sets the ChannelData.
        /// </summary>
        /// <value>
        /// The ChannelData.
        /// </value>
        public string ChannelData { get; set; }

        /// <summary>
        /// Gets or sets the ChannelDetailTemplate.
        /// </summary>
        /// <value>
        /// The ChannelDetailTemplate.
        /// </value>
        public string ChannelDetailTemplate { get; set; }

        /// <summary>
        /// Gets or sets the ChannelEntityId.
        /// </summary>
        /// <value>
        /// The ChannelEntityId.
        /// </value>
        public int? ChannelEntityId { get; set; }

        /// <summary>
        /// Gets or sets the ChannelListTemplate.
        /// </summary>
        /// <value>
        /// The ChannelListTemplate.
        /// </value>
        public string ChannelListTemplate { get; set; }

        /// <summary>
        /// Gets or sets the ChannelTypeMediumValueId.
        /// </summary>
        /// <value>
        /// The ChannelTypeMediumValueId.
        /// </value>
        public int? ChannelTypeMediumValueId { get; set; }

        /// <summary>
        /// Gets or sets the ComponentCacheDuration.
        /// </summary>
        /// <value>
        /// The ComponentCacheDuration.
        /// </value>
        public int? ComponentCacheDuration { get; set; }

        /// <summary>
        /// Gets or sets the ComponentCustom1Label.
        /// </summary>
        /// <value>
        /// The ComponentCustom1Label.
        /// </value>
        public string ComponentCustom1Label { get; set; }

        /// <summary>
        /// Gets or sets the ComponentCustom2Label.
        /// </summary>
        /// <value>
        /// The ComponentCustom2Label.
        /// </value>
        public string ComponentCustom2Label { get; set; }

        /// <summary>
        /// Gets or sets the ComponentCustomIndexed1Label.
        /// </summary>
        /// <value>
        /// The ComponentCustomIndexed1Label.
        /// </value>
        public string ComponentCustomIndexed1Label { get; set; }

        /// <summary>
        /// Gets or sets the ComponentDetailTemplate.
        /// </summary>
        /// <value>
        /// The ComponentDetailTemplate.
        /// </value>
        public string ComponentDetailTemplate { get; set; }

        /// <summary>
        /// Gets or sets the ComponentEntityTypeId.
        /// </summary>
        /// <value>
        /// The ComponentEntityTypeId.
        /// </value>
        public int? ComponentEntityTypeId { get; set; }

        /// <summary>
        /// Gets or sets the ComponentListTemplate.
        /// </summary>
        /// <value>
        /// The ComponentListTemplate.
        /// </value>
        public string ComponentListTemplate { get; set; }

        /// <summary>
        /// Gets or sets the EngagementStrength.
        /// </summary>
        /// <value>
        /// The EngagementStrength.
        /// </value>
        public int? EngagementStrength { get; set; }

        /// <summary>
        /// Gets or sets the InteractionCustom1Label.
        /// </summary>
        /// <value>
        /// The InteractionCustom1Label.
        /// </value>
        public string InteractionCustom1Label { get; set; }

        /// <summary>
        /// Gets or sets the InteractionCustom2Label.
        /// </summary>
        /// <value>
        /// The InteractionCustom2Label.
        /// </value>
        public string InteractionCustom2Label { get; set; }

        /// <summary>
        /// Gets or sets the InteractionCustomIndexed1Label.
        /// </summary>
        /// <value>
        /// The InteractionCustomIndexed1Label.
        /// </value>
        public string InteractionCustomIndexed1Label { get; set; }

        /// <summary>
        /// Gets or sets the InteractionDetailTemplate.
        /// </summary>
        /// <value>
        /// The InteractionDetailTemplate.
        /// </value>
        public string InteractionDetailTemplate { get; set; }

        /// <summary>
        /// Gets or sets the InteractionEntityTypeId.
        /// </summary>
        /// <value>
        /// The InteractionEntityTypeId.
        /// </value>
        public int? InteractionEntityTypeId { get; set; }

        /// <summary>
        /// Gets or sets the InteractionListTemplate.
        /// </summary>
        /// <value>
        /// The InteractionListTemplate.
        /// </value>
        public string InteractionListTemplate { get; set; }

        /// <summary>
        /// Gets or sets the IsActive.
        /// </summary>
        /// <value>
        /// The IsActive.
        /// </value>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        /// <value>
        /// The Name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the RetentionDuration.
        /// </summary>
        /// <value>
        /// The RetentionDuration.
        /// </value>
        public int? RetentionDuration { get; set; }

        /// <summary>
        /// Gets or sets the SessionDetailTemplate.
        /// </summary>
        /// <value>
        /// The SessionDetailTemplate.
        /// </value>
        public string SessionDetailTemplate { get; set; }

        /// <summary>
        /// Gets or sets the SessionListTemplate.
        /// </summary>
        /// <value>
        /// The SessionListTemplate.
        /// </value>
        public string SessionListTemplate { get; set; }

        /// <summary>
        /// Gets or sets the UsesSession.
        /// </summary>
        /// <value>
        /// The UsesSession.
        /// </value>
        public bool UsesSession { get; set; }

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
