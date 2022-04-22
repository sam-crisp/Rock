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

namespace Rock.Data
{
    /// <summary>
    /// Used to decorate items in Rock with well-known GUID attributes.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [System.AttributeUsage( System.AttributeTargets.Class | System.AttributeTargets.Method, Inherited = false, AllowMultiple = false )]
    public class RockGuidAttribute : System.Attribute
    {
        /// <summary>
        /// Gets the unique identifier.
        /// </summary>
        /// <value>The unique identifier.</value>
        public readonly Guid Guid;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Rock.RockGuidAttribute" /> class from the specified  <see cref="string"/>.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        public RockGuidAttribute( string guid )
        {
            this.Guid = new Guid( guid );
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Rock.RockGuidAttribute" /> class from the specified <see cref="System.Guid"/>.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        public RockGuidAttribute( Guid guid )
        {
            this.Guid = guid;
        }
    }

    /// <summary>
    /// Class RockBlockTypeGuid.
    /// Implements the <see cref="Rock.Data.RockGuidAttribute" />
    /// </summary>
    /// <seealso cref="Rock.Data.RockGuidAttribute" />
    public class RockBlockTypeGuidAttribute : RockGuidAttribute
    {
        /// <summary>
        /// Gets the unique identifier.
        /// </summary>
        /// <value>The unique identifier.</value>
        public readonly Guid? EntityTypeGuid;

        /// <summary>
        /// Initializes a new instance of the <see cref="RockBlockTypeGuidAttribute"/> class.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        public RockBlockTypeGuidAttribute( string guid ) : base( guid ) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="RockBlockTypeGuidAttribute"/> class.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        public RockBlockTypeGuidAttribute( Guid guid ) : base( guid ) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="RockBlockTypeGuidAttribute"/> class.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <param name="entityTypeGuid">The entity type unique identifier.</param>
        public RockBlockTypeGuidAttribute( string guid, string entityTypeGuid )
            : this( new Guid( guid ), entityTypeGuid.AsGuidOrNull() ) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Rock.RockGuidAttribute" /> class from the specified <see cref="System.Guid" />.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <param name="entityTypeGuid">The entity type unique identifier.</param>
        public RockBlockTypeGuidAttribute( Guid guid, Guid? entityTypeGuid ) : base( guid )
        {
            this.EntityTypeGuid = entityTypeGuid;
        }
    }
}
