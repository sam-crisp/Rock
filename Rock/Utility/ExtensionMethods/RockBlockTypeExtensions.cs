﻿// <copyright>
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

using Rock.Blocks;

namespace Rock
{
    /// <summary>
    /// Extension methods for <see cref="RockBlockType"/>
    /// </summary>
    internal static class RockBlockTypeExtensions
    {
        /// <summary>
        /// Builds and returns the URL for a linked <see cref="Rock.Model.Page"/>
        /// from a <see cref="Rock.Attribute.LinkedPageAttribute"/> and any necessary
        /// query parameters.
        /// </summary>
        /// <param name="block">The block to get instance data from.</param>
        /// <param name="attributeKey">The attribute key that contains the linked page value.</param>
        /// <param name="queryParams">Any query string parameters that should be included in the built URL.</param>
        /// <returns>A string representing the URL to the linked <see cref="Rock.Model.Page"/>.</returns>
        public static string GetLinkedPageUrl( this RockBlockType block, string attributeKey, IDictionary<string, string> queryParams = null )
        {
            var pageReference = new Rock.Web.PageReference( block.GetAttributeValue( attributeKey ), queryParams != null ? new Dictionary<string, string>( queryParams ) : null );

            if ( pageReference.PageId > 0 )
            {
                return pageReference.BuildUrl();
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Builds and returns the URL for the parent <see cref="Rock.Model.Page"/>
        /// from the current page and any necessary query parameters.
        /// </summary>
        /// <param name="block">The block to get instance data from.</param>
        /// <param name="queryParams">Any query string parameters that should be included in the built URL.</param>
        /// <returns>A string representing the URL to the parent <see cref="Rock.Model.Page"/>.</returns>
        public static string GetParentPageUrl( this RockBlockType block, IDictionary<string, string> queryParams = null )
        {
            if ( block.PageCache.ParentPage == null )
            {
                return string.Empty;
            }

            var pageReference = new Rock.Web.PageReference( block.PageCache.ParentPage.Guid.ToString(), queryParams != null ? new Dictionary<string, string>( queryParams ) : null );

            if ( pageReference.PageId > 0 )
            {
                return pageReference.BuildUrl();
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Builds and returns the URL for the current <see cref="Rock.Model.Page"/>
        /// and any necessary query parameters.
        /// </summary>
        /// <param name="block">The block to get instance data from.</param>
        /// <param name="queryParams">Any query string parameters that should be included in the built URL.</param>
        /// <returns>A string representing the URL to the current <see cref="Rock.Model.Page"/>.</returns>
        public static string GetCurrentPageUrl( this RockBlockType block, IDictionary<string, string> queryParams = null )
        {
            var pageReference = new Rock.Web.PageReference( block.PageCache.Guid.ToString(), queryParams != null ? new Dictionary<string, string>( queryParams ) : null );

            if ( pageReference.PageId > 0 )
            {
                return pageReference.BuildUrl();
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
