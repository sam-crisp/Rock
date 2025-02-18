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
    /// ConnectionOpportunity View Model
    /// </summary>
    public partial class ConnectionOpportunityBag : EntityBagBase
    {
        /// <summary>
        /// Gets or sets the ConnectionTypeId.
        /// </summary>
        /// <value>
        /// The ConnectionTypeId.
        /// </value>
        public int ConnectionTypeId { get; set; }

        /// <summary>
        /// Gets or sets the Description.
        /// </summary>
        /// <value>
        /// The Description.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the IconCssClass.
        /// </summary>
        /// <value>
        /// The IconCssClass.
        /// </value>
        public string IconCssClass { get; set; }

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
        /// Gets or sets the Order.
        /// </summary>
        /// <value>
        /// The Order.
        /// </value>
        public int Order { get; set; }

        /// <summary>
        /// Gets or sets the PhotoId.
        /// </summary>
        /// <value>
        /// The PhotoId.
        /// </value>
        public int? PhotoId { get; set; }

        /// <summary>
        /// Gets or sets the PublicName.
        /// </summary>
        /// <value>
        /// The PublicName.
        /// </value>
        public string PublicName { get; set; }

        /// <summary>
        /// Gets or sets the ShowCampusOnTransfer.
        /// </summary>
        /// <value>
        /// The ShowCampusOnTransfer.
        /// </value>
        public bool ShowCampusOnTransfer { get; set; }

        /// <summary>
        /// Gets or sets the ShowConnectButton.
        /// </summary>
        /// <value>
        /// The ShowConnectButton.
        /// </value>
        public bool ShowConnectButton { get; set; }

        /// <summary>
        /// Gets or sets the ShowStatusOnTransfer.
        /// </summary>
        /// <value>
        /// The ShowStatusOnTransfer.
        /// </value>
        public bool ShowStatusOnTransfer { get; set; }

        /// <summary>
        /// Gets or sets the Summary.
        /// </summary>
        /// <value>
        /// The Summary.
        /// </value>
        public string Summary { get; set; }

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
