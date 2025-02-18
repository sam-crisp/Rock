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

using System;
using System.Linq;

using Rock.Attribute;
using Rock.Data;
using Rock.ViewModels;
using Rock.ViewModels.Entities;
using Rock.Web.Cache;

namespace Rock.Model
{
    /// <summary>
    /// DocumentType Service class
    /// </summary>
    public partial class DocumentTypeService : Service<DocumentType>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentTypeService"/> class
        /// </summary>
        /// <param name="context">The context.</param>
        public DocumentTypeService(RockContext context) : base(context)
        {
        }

        /// <summary>
        /// Determines whether this instance can delete the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <returns>
        ///   <c>true</c> if this instance can delete the specified item; otherwise, <c>false</c>.
        /// </returns>
        public bool CanDelete( DocumentType item, out string errorMessage )
        {
            errorMessage = string.Empty;

            if ( new Service<Document>( Context ).Queryable().Any( a => a.DocumentTypeId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", DocumentType.FriendlyTypeName, Document.FriendlyTypeName );
                return false;
            }
            return true;
        }
    }

    /// <summary>
    /// DocumentType View Model Helper
    /// </summary>
    [DefaultViewModelHelper( typeof( DocumentType ) )]
    public partial class DocumentTypeViewModelHelper : ViewModelHelper<DocumentType, DocumentTypeBag>
    {
        /// <summary>
        /// Converts the model to a view model.
        /// </summary>
        /// <param name="model">The entity.</param>
        /// <param name="currentPerson">The current person.</param>
        /// <param name="loadAttributes">if set to <c>true</c> [load attributes].</param>
        /// <returns></returns>
        public override DocumentTypeBag CreateViewModel( DocumentType model, Person currentPerson = null, bool loadAttributes = true )
        {
            if ( model == null )
            {
                return default;
            }

            var viewModel = new DocumentTypeBag
            {
                Id = model.Id,
                IdKey = model.IdKey,
                Guid = model.Guid,
                BinaryFileTypeId = model.BinaryFileTypeId,
                DefaultDocumentNameTemplate = model.DefaultDocumentNameTemplate,
                EntityTypeId = model.EntityTypeId,
                EntityTypeQualifierColumn = model.EntityTypeQualifierColumn,
                EntityTypeQualifierValue = model.EntityTypeQualifierValue,
                IconCssClass = model.IconCssClass,
                IsImage = model.IsImage,
                IsSystem = model.IsSystem,
                MaxDocumentsPerEntity = model.MaxDocumentsPerEntity,
                Name = model.Name,
                Order = model.Order,
                UserSelectable = model.UserSelectable,
                CreatedDateTime = model.CreatedDateTime,
                ModifiedDateTime = model.ModifiedDateTime,
                CreatedByPersonAliasId = model.CreatedByPersonAliasId,
                ModifiedByPersonAliasId = model.ModifiedByPersonAliasId,
            };

            AddAttributesToViewModel( model, viewModel, currentPerson, loadAttributes );
            ApplyAdditionalPropertiesAndSecurityToViewModel( model, viewModel, currentPerson, loadAttributes );
            return viewModel;
        }
    }


    /// <summary>
    /// Generated Extension Methods
    /// </summary>
    public static partial class DocumentTypeExtensionMethods
    {
        /// <summary>
        /// Clones this DocumentType object to a new DocumentType object
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="deepCopy">if set to <c>true</c> a deep copy is made. If false, only the basic entity properties are copied.</param>
        /// <returns></returns>
        public static DocumentType Clone( this DocumentType source, bool deepCopy )
        {
            if (deepCopy)
            {
                return source.Clone() as DocumentType;
            }
            else
            {
                var target = new DocumentType();
                target.CopyPropertiesFrom( source );
                return target;
            }
        }

        /// <summary>
        /// Clones this DocumentType object to a new DocumentType object with default values for the properties in the Entity and Model base classes.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static DocumentType CloneWithoutIdentity( this DocumentType source )
        {
            var target = new DocumentType();
            target.CopyPropertiesFrom( source );

            target.Id = 0;
            target.Guid = Guid.NewGuid();
            target.ForeignKey = null;
            target.ForeignId = null;
            target.ForeignGuid = null;
            target.CreatedByPersonAliasId = null;
            target.CreatedDateTime = RockDateTime.Now;
            target.ModifiedByPersonAliasId = null;
            target.ModifiedDateTime = RockDateTime.Now;

            return target;
        }

        /// <summary>
        /// Copies the properties from another DocumentType object to this DocumentType object
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="source">The source.</param>
        public static void CopyPropertiesFrom( this DocumentType target, DocumentType source )
        {
            target.Id = source.Id;
            target.BinaryFileTypeId = source.BinaryFileTypeId;
            target.DefaultDocumentNameTemplate = source.DefaultDocumentNameTemplate;
            target.EntityTypeId = source.EntityTypeId;
            target.EntityTypeQualifierColumn = source.EntityTypeQualifierColumn;
            target.EntityTypeQualifierValue = source.EntityTypeQualifierValue;
            target.ForeignGuid = source.ForeignGuid;
            target.ForeignKey = source.ForeignKey;
            target.IconCssClass = source.IconCssClass;
            target.IsImage = source.IsImage;
            target.IsSystem = source.IsSystem;
            target.MaxDocumentsPerEntity = source.MaxDocumentsPerEntity;
            target.Name = source.Name;
            target.Order = source.Order;
            target.UserSelectable = source.UserSelectable;
            target.CreatedDateTime = source.CreatedDateTime;
            target.ModifiedDateTime = source.ModifiedDateTime;
            target.CreatedByPersonAliasId = source.CreatedByPersonAliasId;
            target.ModifiedByPersonAliasId = source.ModifiedByPersonAliasId;
            target.Guid = source.Guid;
            target.ForeignId = source.ForeignId;

        }

        /// <summary>
        /// Creates a view model from this entity
        /// </summary>
        /// <param name="model">The entity.</param>
        /// <param name="currentPerson" >The currentPerson.</param>
        /// <param name="loadAttributes" >Load attributes?</param>
        public static DocumentTypeBag ToViewModel( this DocumentType model, Person currentPerson = null, bool loadAttributes = false )
        {
            var helper = new DocumentTypeViewModelHelper();
            var viewModel = helper.CreateViewModel( model, currentPerson, loadAttributes );
            return viewModel;
        }

    }

}
