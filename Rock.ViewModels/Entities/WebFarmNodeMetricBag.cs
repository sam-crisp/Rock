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
    /// WebFarmNodeMetric View Model
    /// </summary>
    public partial class WebFarmNodeMetricBag : EntityBagBase
    {
        /// <summary>
        /// Gets or sets the MetricType.
        /// </summary>
        /// <value>
        /// The MetricType.
        /// </value>
        public int MetricType { get; set; }

        /// <summary>
        /// Gets or sets the MetricValue.
        /// </summary>
        /// <value>
        /// The MetricValue.
        /// </value>
        public decimal MetricValue { get; set; }

        /// <summary>
        /// Gets or sets the MetricValueDateTime.
        /// </summary>
        /// <value>
        /// The MetricValueDateTime.
        /// </value>
        public DateTime MetricValueDateTime { get; set; }

        /// <summary>
        /// Gets or sets the Note.
        /// </summary>
        /// <value>
        /// The Note.
        /// </value>
        public string Note { get; set; }

        /// <summary>
        /// Gets or sets the WebFarmNodeId.
        /// </summary>
        /// <value>
        /// The WebFarmNodeId.
        /// </value>
        public int WebFarmNodeId { get; set; }

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
