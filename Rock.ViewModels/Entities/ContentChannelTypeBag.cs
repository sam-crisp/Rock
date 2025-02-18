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
    /// ContentChannelType View Model
    /// </summary>
    public partial class ContentChannelTypeBag : EntityBagBase
    {
        /// <summary>
        /// Gets or sets the DateRangeType.
        /// </summary>
        /// <value>
        /// The DateRangeType.
        /// </value>
        public int DateRangeType { get; set; }

        /// <summary>
        /// Gets or sets the DisableContentField.
        /// </summary>
        /// <value>
        /// The DisableContentField.
        /// </value>
        public bool DisableContentField { get; set; }

        /// <summary>
        /// Gets or sets the DisablePriority.
        /// </summary>
        /// <value>
        /// The DisablePriority.
        /// </value>
        public bool DisablePriority { get; set; }

        /// <summary>
        /// Gets or sets the DisableStatus.
        /// </summary>
        /// <value>
        /// The DisableStatus.
        /// </value>
        public bool DisableStatus { get; set; }

        /// <summary>
        /// Gets or sets the IncludeTime.
        /// </summary>
        /// <value>
        /// The IncludeTime.
        /// </value>
        public bool IncludeTime { get; set; }

        /// <summary>
        /// Gets or sets the IsSystem.
        /// </summary>
        /// <value>
        /// The IsSystem.
        /// </value>
        public bool IsSystem { get; set; }

        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        /// <value>
        /// The Name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the ShowInChannelList.
        /// </summary>
        /// <value>
        /// The ShowInChannelList.
        /// </value>
        public bool ShowInChannelList { get; set; }

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
