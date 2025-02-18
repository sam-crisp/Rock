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
using System.Linq;
using System.Web.Http;

using Rock.Data;
using Rock.Model;
using Rock.Rest.Filters;

namespace Rock.Rest.Controllers
{
    /// <summary>
    /// HtmlContents REST API
    /// </summary>
    [RockGuid( "fe7e5808-b5a0-46ba-9a42-9e2e020ac822" )]
    public partial class HtmlContentsController
    {
        /// <summary>
        /// Updates the contents.
        /// </summary>
        /// <param name="blockId">The block identifier.</param>
        /// <param name="htmlContents">The HTML contents.</param>
        [Authenticate]
        [HttpPost]
        [System.Web.Http.Route( "api/HtmlContents/UpdateContents/{blockId}" )]
        [RockGuid( "6236deeb-8536-4485-890b-0fa4b0c86f81" )]
        public void UpdateContents( int blockId, [FromBody] HtmlContents htmlContents )
        {
            // Enable proxy creation since security is being checked and need to navigate parent authorities
            SetProxyCreation( true );

            var person = GetPerson();

            var block = new BlockService( ( RockContext ) Service.Context ).Get( blockId );
            if ( block != null && block.IsAuthorized( Rock.Security.Authorization.EDIT, person ) )
            {
                var htmlContentService = ( HtmlContentService ) Service;
                var htmlContent = htmlContentService.GetActiveContentQueryable( blockId, htmlContents.EntityValue ).FirstOrDefault();
                if ( htmlContent != null )
                {
                    htmlContent.Content = htmlContents.Content;
                    System.Web.HttpContext.Current.AddOrReplaceItem( "CurrentPerson", person );

                    Service.Context.SaveChanges();

                    HtmlContentService.FlushCachedContent( blockId, htmlContents.EntityValue );
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        public class HtmlContents
        {
            /// <summary>
            /// Gets or sets the entity value.
            /// </summary>
            /// <value>
            /// The entity value.
            /// </value>
            public string EntityValue { get; set; }

            /// <summary>
            /// Gets or sets the content.
            /// </summary>
            /// <value>
            /// The content.
            /// </value>
            public string Content { get; set; }
        }
    }
}
