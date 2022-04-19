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
namespace Rock.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    /// <summary>
    ///
    /// </summary>
    public partial class NewPersonDetailGroupMembersBlockType : Rock.Migrations.RockMigration
    {
        /// <summary>
        /// Operations to be performed during the upgrade process.
        /// </summary>
        public override void Up()
        {
            RockMigrationHelper.UpdateBlockTypeByGuid( "Group Members (V1)", "Allows you to view the other members of a group person belongs to (e.g. Family groups).", "~/Blocks/Crm/PersonDetail/GroupMembersV1.ascx", "CRM > Person Detail", "FC137BDA-4F05-4ECE-9899-A249C90D11FC" );
            RockMigrationHelper.AddDefinedTypeAttribute("2E68D37C-FB7B-4AA5-9E09-3785D52156CB","9C204CD0-1233-41C5-818A-C5DA439445AA","Icon CSS Class","IconCSSClass","",2014,"","348B1563-BBC9-4853-BD4E-4CEAE87CA2B6");
            RockMigrationHelper.AddDefinedValueAttributeValue("8C52E53C-2A66-435A-AE6E-5EE307D9A0DC","348B1563-BBC9-4853-BD4E-4CEAE87CA2B6",@"fa fa-home");
            RockMigrationHelper.AddDefinedValueAttributeValue("E071472A-F805-4FC4-917A-D5E3C095C35C","348B1563-BBC9-4853-BD4E-4CEAE87CA2B6",@"fa fa-building");
            RockMigrationHelper.AddDefinedValueAttributeValue("853D98F1-6E08-4321-861B-520B4106CFE0","348B1563-BBC9-4853-BD4E-4CEAE87CA2B6",@"fa fa-map-marker");
        }
        
        /// <summary>
        /// Operations to be performed during the downgrade process.
        /// </summary>
        public override void Down()
        {
            RockMigrationHelper.UpdateBlockTypeByGuid( "Group Members", "Allows you to view the other members of a group person belongs to (e.g. Family groups).", "~/Blocks/Crm/PersonDetail/GroupMembers.ascx", "CRM > Person Detail", "FC137BDA-4F05-4ECE-9899-A249C90D11FC" );
            RockMigrationHelper.DeleteAttribute("348B1563-BBC9-4853-BD4E-4CEAE87CA2B6"); // IconCSSClass
        }
    }
}
