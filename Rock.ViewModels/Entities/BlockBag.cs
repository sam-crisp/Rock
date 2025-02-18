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
    /// Block View Model
    /// </summary>
    public partial class BlockBag : EntityBagBase
    {
        /// <summary>
        /// Gets or sets the AdditionalSettings.
        /// </summary>
        /// <value>
        /// The AdditionalSettings.
        /// </value>
        public string AdditionalSettings { get; set; }

        /// <summary>
        /// Gets or sets the BlockTypeId.
        /// </summary>
        /// <value>
        /// The BlockTypeId.
        /// </value>
        public int BlockTypeId { get; set; }

        /// <summary>
        /// Gets or sets the CssClass.
        /// </summary>
        /// <value>
        /// The CssClass.
        /// </value>
        public string CssClass { get; set; }

        /// <summary>
        /// Gets or sets the IsSystem.
        /// </summary>
        /// <value>
        /// The IsSystem.
        /// </value>
        public bool IsSystem { get; set; }

        /// <summary>
        /// Gets or sets the LayoutId.
        /// </summary>
        /// <value>
        /// The LayoutId.
        /// </value>
        public int? LayoutId { get; set; }

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
        /// Gets or sets the OutputCacheDuration.
        /// </summary>
        /// <value>
        /// The OutputCacheDuration.
        /// </value>
        public int OutputCacheDuration { get; set; }

        /// <summary>
        /// Gets or sets the PageId.
        /// </summary>
        /// <value>
        /// The PageId.
        /// </value>
        public int? PageId { get; set; }

        /// <summary>
        /// Gets or sets the PostHtml.
        /// </summary>
        /// <value>
        /// The PostHtml.
        /// </value>
        public string PostHtml { get; set; }

        /// <summary>
        /// Gets or sets the PreHtml.
        /// </summary>
        /// <value>
        /// The PreHtml.
        /// </value>
        public string PreHtml { get; set; }

        /// <summary>
        /// Gets or sets the SiteId.
        /// </summary>
        /// <value>
        /// The SiteId.
        /// </value>
        public int? SiteId { get; set; }

        /// <summary>
        /// Gets or sets the Zone.
        /// </summary>
        /// <value>
        /// The Zone.
        /// </value>
        public string Zone { get; set; }

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
