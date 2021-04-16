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
    public partial class AddMediaRelatedTable : Rock.Migrations.RockMigration
    {
        /// <summary>
        /// Operations to be performed during the upgrade process.
        /// </summary>
        public override void Up()
        {
            CreateTable(
                "dbo.MediaAccount",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 200),
                        LastRefreshDateTime = c.DateTime(),
                        IsActive = c.Boolean(nullable: false),
                        ComponentEntityTypeId = c.Int(nullable: false),
                        CreatedDateTime = c.DateTime(),
                        ModifiedDateTime = c.DateTime(),
                        CreatedByPersonAliasId = c.Int(),
                        ModifiedByPersonAliasId = c.Int(),
                        Guid = c.Guid(nullable: false),
                        ForeignId = c.Int(),
                        ForeignGuid = c.Guid(),
                        ForeignKey = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.EntityType", t => t.ComponentEntityTypeId)
                .ForeignKey("dbo.PersonAlias", t => t.CreatedByPersonAliasId)
                .ForeignKey("dbo.PersonAlias", t => t.ModifiedByPersonAliasId)
                .Index(t => t.ComponentEntityTypeId)
                .Index(t => t.CreatedByPersonAliasId)
                .Index(t => t.ModifiedByPersonAliasId)
                .Index(t => t.Guid, unique: true);
            
            CreateTable(
                "dbo.MediaFolder",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MediaAccountId = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 200),
                        Description = c.String(),
                        IsPublic = c.Boolean(),
                        SourceData = c.String(),
                        MetricData = c.String(),
                        SourceKey = c.String(maxLength: 60),
                        IsContentChannelSyncEnabled = c.Boolean(nullable: false),
                        ContentChannelId = c.Int(),
                        Status = c.Int(),
                        ContentChannelAttributeId = c.Int(),
                        CreatedDateTime = c.DateTime(),
                        ModifiedDateTime = c.DateTime(),
                        CreatedByPersonAliasId = c.Int(),
                        ModifiedByPersonAliasId = c.Int(),
                        Guid = c.Guid(nullable: false),
                        ForeignId = c.Int(),
                        ForeignGuid = c.Guid(),
                        ForeignKey = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ContentChannel", t => t.ContentChannelId)
                .ForeignKey("dbo.Attribute", t => t.ContentChannelAttributeId)
                .ForeignKey("dbo.PersonAlias", t => t.CreatedByPersonAliasId)
                .ForeignKey("dbo.MediaAccount", t => t.MediaAccountId, cascadeDelete: true)
                .ForeignKey("dbo.PersonAlias", t => t.ModifiedByPersonAliasId)
                .Index(t => t.MediaAccountId)
                .Index(t => t.ContentChannelId)
                .Index(t => t.ContentChannelAttributeId)
                .Index(t => t.CreatedByPersonAliasId)
                .Index(t => t.ModifiedByPersonAliasId)
                .Index(t => t.Guid, unique: true);
            
            CreateTable(
                "dbo.MediaElement",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MediaFolderId = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 200),
                        Description = c.String(),
                        Duration = c.Decimal(precision: 18, scale: 2),
                        SourceCreatedDateTime = c.DateTime(),
                        SourceModifiedDateTime = c.DateTime(),
                        SourceData = c.String(),
                        SourceMetric = c.String(),
                        SourceKey = c.String(maxLength: 60),
                        ThumbnailData = c.String(),
                        MediaElementData = c.String(),
                        DownloadData = c.String(),
                        CreatedDateTime = c.DateTime(),
                        ModifiedDateTime = c.DateTime(),
                        CreatedByPersonAliasId = c.Int(),
                        ModifiedByPersonAliasId = c.Int(),
                        Guid = c.Guid(nullable: false),
                        ForeignId = c.Int(),
                        ForeignGuid = c.Guid(),
                        ForeignKey = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PersonAlias", t => t.CreatedByPersonAliasId)
                .ForeignKey("dbo.MediaFolder", t => t.MediaFolderId, cascadeDelete: true)
                .ForeignKey("dbo.PersonAlias", t => t.ModifiedByPersonAliasId)
                .Index(t => t.MediaFolderId)
                .Index(t => t.CreatedByPersonAliasId)
                .Index(t => t.ModifiedByPersonAliasId)
                .Index(t => t.Guid, unique: true);

            // Add Pages
            //------------------------
            // Add Page Media Accounts to Site:Rock RMS              
            RockMigrationHelper.AddPage( true, "B4A24AB7-9369-4055-883F-4F4892C39AE3","D65F783D-87A9-4CC9-8110-E83466A0EADB","Media Accounts","","07CB7BB5-1465-4E75-8DD4-28FA6EA48222","fa fa-play-circle");  
            // Add Page Media Account to Site:Rock RMS              
            RockMigrationHelper.AddPage( true, "07CB7BB5-1465-4E75-8DD4-28FA6EA48222","D65F783D-87A9-4CC9-8110-E83466A0EADB","Media Account","","52548B49-6D09-467E-BEA9-04DD6F51637D","");  
            // Add Page Media Folder to Site:Rock RMS              
            RockMigrationHelper.AddPage( true, "52548B49-6D09-467E-BEA9-04DD6F51637D","D65F783D-87A9-4CC9-8110-E83466A0EADB","Media Folder","","65DE6218-2850-4924-AA55-6F6FB572E9A3","");  
            // Add Page Media Element to Site:Rock RMS              
            RockMigrationHelper.AddPage( true, "65DE6218-2850-4924-AA55-6F6FB572E9A3","D65F783D-87A9-4CC9-8110-E83466A0EADB","Media Element","","F1AB34EE-941F-41D6-9BA1-22348D09724C","");

            // Add Block Types
            //------------------------
            // Add/Update BlockType Media Account Detail              
            RockMigrationHelper.UpdateBlockType("Media Account Detail","Edit details of a Media Account","~/Blocks/Cms/MediaAccountDetail.ascx","CMS","0361FFC9-F32F-4C97-98BD-9DFE5F4A777E");
            // Add/Update BlockType Media Account List              
            RockMigrationHelper.UpdateBlockType("Media Account List","List Media Accounts","~/Blocks/Cms/MediaAccountList.ascx","CMS","7537AB61-F80B-43B1-998B-1D2B03303B36");
            // Add/Update BlockType Media Element List              
            RockMigrationHelper.UpdateBlockType("Media Element List","List Media Elements","~/Blocks/Cms/MediaElementList.ascx","CMS","28D6F57B-59D9-4DA6-A8DC-6DBD3E157554");
            // Add/Update BlockType Media Folder Detail              
            RockMigrationHelper.UpdateBlockType("Media Folder Detail","Edit details of a Media Folder","~/Blocks/Cms/MediaFolderDetail.ascx","CMS","3C9D442B-D066-43FA-9380-98C60936992E");
            // Add/Update BlockType Media Folder List              
            RockMigrationHelper.UpdateBlockType("Media Folder List","List Media Folders","~/Blocks/Cms/MediaFolderList.ascx","CMS","02A91579-9355-45E7-A67A-56E998FB332A");
            // Add/Update BlockType Media Element Detail              
            RockMigrationHelper.UpdateBlockType("Media Element Detail","Edit details of a Media Element","~/Blocks/Cms/MediaElementDetail.ascx","CMS","881DC0D1-FF98-4A5E-827F-49DD5CD0BD32");

            // Add Blocks
            //------------------------
            // Add Block Media Account List to Page: Media Accounts, Site: Rock RMS              
            RockMigrationHelper.AddBlock( true, "07CB7BB5-1465-4E75-8DD4-28FA6EA48222".AsGuid(),null,"C2D29296-6A87-47A9-A753-EE4E9159C4C4".AsGuid(),"7537AB61-F80B-43B1-998B-1D2B03303B36".AsGuid(), "Media Account List","Main",@"",@"",0,"C38FB340-FD4B-4BDE-A306-FE9B75D71A85");   
            // Add Block Media Account Detail to Page: Media Account, Site: Rock RMS              
            RockMigrationHelper.AddBlock( true, "52548B49-6D09-467E-BEA9-04DD6F51637D".AsGuid(),null,"C2D29296-6A87-47A9-A753-EE4E9159C4C4".AsGuid(),"0361FFC9-F32F-4C97-98BD-9DFE5F4A777E".AsGuid(), "Media Account Detail","Main",@"",@"",0,"ABAD84DA-113F-40E5-9DBD-ADA72F5B95B8");   
            // Add Block Media Folder List to Page: Media Account, Site: Rock RMS              
            RockMigrationHelper.AddBlock( true, "52548B49-6D09-467E-BEA9-04DD6F51637D".AsGuid(),null,"C2D29296-6A87-47A9-A753-EE4E9159C4C4".AsGuid(),"02A91579-9355-45E7-A67A-56E998FB332A".AsGuid(), "Media Folder List","Main",@"",@"",1,"14A0B30E-8287-4791-8443-0FAAB80FB559");   
            // Add Block Media Folder Detail to Page: Media Folder, Site: Rock RMS              
            RockMigrationHelper.AddBlock( true, "65DE6218-2850-4924-AA55-6F6FB572E9A3".AsGuid(),null,"C2D29296-6A87-47A9-A753-EE4E9159C4C4".AsGuid(),"3C9D442B-D066-43FA-9380-98C60936992E".AsGuid(), "Media Folder Detail","Main",@"",@"",0,"37108EB5-2F1F-484F-BD2D-FEF8AD6DFC18");   
            // Add Block Media Element List to Page: Media Folder, Site: Rock RMS              
            RockMigrationHelper.AddBlock( true, "65DE6218-2850-4924-AA55-6F6FB572E9A3".AsGuid(),null,"C2D29296-6A87-47A9-A753-EE4E9159C4C4".AsGuid(),"28D6F57B-59D9-4DA6-A8DC-6DBD3E157554".AsGuid(), "Media Element List","Main",@"",@"",1,"AA71BC08-DB91-43E3-BBB7-A03C698D1184");   
            // Add Block Media Element Detail to Page: Media Element, Site: Rock RMS              
            RockMigrationHelper.AddBlock( true, "F1AB34EE-941F-41D6-9BA1-22348D09724C".AsGuid(),null,"C2D29296-6A87-47A9-A753-EE4E9159C4C4".AsGuid(),"881DC0D1-FF98-4A5E-827F-49DD5CD0BD32".AsGuid(), "Media Element Detail","Main",@"",@"",0,"104BF960-5167-4ADE-A26B-6F63762877E3");

            // Update Block Order
            //------------------------
            // update block order for pages with new blocks if the page,zone has multiple blocks
            // Update Order for Page: Media Account,  Zone: Main,  Block: Media Account Detail              
            Sql( @"UPDATE [Block] SET [Order] = 0 WHERE [Guid] = 'ABAD84DA-113F-40E5-9DBD-ADA72F5B95B8'"  );
            // Update Order for Page: Media Account,  Zone: Main,  Block: Media Folder List              
            Sql( @"UPDATE [Block] SET [Order] = 1 WHERE [Guid] = '14A0B30E-8287-4791-8443-0FAAB80FB559'"  );
            // Update Order for Page: Media Folder,  Zone: Main,  Block: Media Element List              
            Sql( @"UPDATE [Block] SET [Order] = 1 WHERE [Guid] = 'AA71BC08-DB91-43E3-BBB7-A03C698D1184'"  );
            // Update Order for Page: Media Folder,  Zone: Main,  Block: Media Folder Detail              
            Sql( @"UPDATE [Block] SET [Order] = 0 WHERE [Guid] = '37108EB5-2F1F-484F-BD2D-FEF8AD6DFC18'"  );


            // Add Block Attributes
            //------------------------
            // Attribute for BlockType: Media Account List:core.CustomActionsConfigs              
            RockMigrationHelper.AddOrUpdateBlockTypeAttribute( "7537AB61-F80B-43B1-998B-1D2B03303B36", "9C204CD0-1233-41C5-818A-C5DA439445AA", "core.CustomActionsConfigs", "core.CustomActionsConfigs", "core.CustomActionsConfigs", @"", 0, @"", "33BC90A5-952B-4081-A25D-0CCA87E8426B" );  
            // Attribute for BlockType: Media Account List:core.EnableDefaultWorkflowLauncher              
            RockMigrationHelper.AddOrUpdateBlockTypeAttribute( "7537AB61-F80B-43B1-998B-1D2B03303B36", "1EDAFDED-DFE6-4334-B019-6EECBA89E05A", "core.EnableDefaultWorkflowLauncher", "core.EnableDefaultWorkflowLauncher", "core.EnableDefaultWorkflowLauncher", @"", 0, @"True", "FA22299B-8B86-4657-9AA2-D2C9B2C28921" );  
            // Attribute for BlockType: Media Account List:Detail Page              
            RockMigrationHelper.AddOrUpdateBlockTypeAttribute( "7537AB61-F80B-43B1-998B-1D2B03303B36", "BD53F9C9-EBA9-4D3F-82EA-DE5DD34A8108", "Detail Page", "DetailPage", "Detail Page", @"", 0, @"", "D3C8A3CD-3E16-4244-BE6A-29C23662C065" );  
            // Attribute for BlockType: Media Element List:Detail Page              
            RockMigrationHelper.AddOrUpdateBlockTypeAttribute( "28D6F57B-59D9-4DA6-A8DC-6DBD3E157554", "BD53F9C9-EBA9-4D3F-82EA-DE5DD34A8108", "Detail Page", "DetailPage", "Detail Page", @"", 0, @"", "908960AB-4CFD-4DFD-A9B9-F60117DF4427" );  
            // Attribute for BlockType: Media Element List:core.EnableDefaultWorkflowLauncher              
            RockMigrationHelper.AddOrUpdateBlockTypeAttribute( "28D6F57B-59D9-4DA6-A8DC-6DBD3E157554", "1EDAFDED-DFE6-4334-B019-6EECBA89E05A", "core.EnableDefaultWorkflowLauncher", "core.EnableDefaultWorkflowLauncher", "core.EnableDefaultWorkflowLauncher", @"", 0, @"True", "3A13C9B6-F02C-4D72-8D41-F1BE0218C141" );  
            // Attribute for BlockType: Media Element List:core.CustomActionsConfigs              
            RockMigrationHelper.AddOrUpdateBlockTypeAttribute( "28D6F57B-59D9-4DA6-A8DC-6DBD3E157554", "9C204CD0-1233-41C5-818A-C5DA439445AA", "core.CustomActionsConfigs", "core.CustomActionsConfigs", "core.CustomActionsConfigs", @"", 0, @"", "67EFDE42-8E98-438A-85E3-89249B84912A" );  
            // Attribute for BlockType: Media Folder List:core.CustomActionsConfigs              
            RockMigrationHelper.AddOrUpdateBlockTypeAttribute( "02A91579-9355-45E7-A67A-56E998FB332A", "9C204CD0-1233-41C5-818A-C5DA439445AA", "core.CustomActionsConfigs", "core.CustomActionsConfigs", "core.CustomActionsConfigs", @"", 0, @"", "AE584E4E-4179-4D5C-A60A-0AACB7029024" );  
            // Attribute for BlockType: Media Folder List:core.EnableDefaultWorkflowLauncher              
            RockMigrationHelper.AddOrUpdateBlockTypeAttribute( "02A91579-9355-45E7-A67A-56E998FB332A", "1EDAFDED-DFE6-4334-B019-6EECBA89E05A", "core.EnableDefaultWorkflowLauncher", "core.EnableDefaultWorkflowLauncher", "core.EnableDefaultWorkflowLauncher", @"", 0, @"True", "9950DA6D-C175-4E07-8988-7FF1E05DD02D" );  
            // Attribute for BlockType: Media Folder List:Detail Page              
            RockMigrationHelper.AddOrUpdateBlockTypeAttribute( "02A91579-9355-45E7-A67A-56E998FB332A", "BD53F9C9-EBA9-4D3F-82EA-DE5DD34A8108", "Detail Page", "DetailPage", "Detail Page", @"", 0, @"", "2B01D2A0-A67B-4BD9-B196-96CD9B649BAD" );

            // Add Block Attribute Values
            //------------------------
            // Add Block Attribute Value              
            //   Block: Media Account List              
            //   BlockType: Media Account List              
            //   Block Location: Page=Media Accounts, Site=Rock RMS              
            //   Attribute: Detail Page              
            //   Attribute Value: 52548b49-6d09-467e-bea9-04dd6f51637d              
            RockMigrationHelper.AddBlockAttributeValue("C38FB340-FD4B-4BDE-A306-FE9B75D71A85","D3C8A3CD-3E16-4244-BE6A-29C23662C065",@"52548b49-6d09-467e-bea9-04dd6f51637d");  
            // Add Block Attribute Value              
            //   Block: Media Account List              
            //   BlockType: Media Account List              
            //   Block Location: Page=Media Accounts, Site=Rock RMS              
            //   Attribute: core.CustomGridEnableStickyHeaders              
            //   Attribute Value: False              
            RockMigrationHelper.AddBlockAttributeValue("C38FB340-FD4B-4BDE-A306-FE9B75D71A85","13A63AF3-9FDF-46E4-9660-260F301CEEB3",@"False");  
            // Add Block Attribute Value              
            //   Block: Media Account List              
            //   BlockType: Media Account List              
            //   Block Location: Page=Media Accounts, Site=Rock RMS              
            //   Attribute: core.EnableDefaultWorkflowLauncher              
            //   Attribute Value: True              
            RockMigrationHelper.AddBlockAttributeValue("C38FB340-FD4B-4BDE-A306-FE9B75D71A85","FA22299B-8B86-4657-9AA2-D2C9B2C28921",@"True");  
            // Add Block Attribute Value              
            //   Block: Media Folder List              
            //   BlockType: Media Folder List              
            //   Block Location: Page=Media Account, Site=Rock RMS              
            //   Attribute: Detail Page              
            //   Attribute Value: 65de6218-2850-4924-aa55-6f6fb572e9a3              
            RockMigrationHelper.AddBlockAttributeValue("14A0B30E-8287-4791-8443-0FAAB80FB559","2B01D2A0-A67B-4BD9-B196-96CD9B649BAD",@"65de6218-2850-4924-aa55-6f6fb572e9a3");  
            // Add Block Attribute Value              
            //   Block: Media Folder List              
            //   BlockType: Media Folder List              
            //   Block Location: Page=Media Account, Site=Rock RMS              
            //   Attribute: core.CustomGridEnableStickyHeaders              
            //   Attribute Value: False              
            RockMigrationHelper.AddBlockAttributeValue("14A0B30E-8287-4791-8443-0FAAB80FB559","637D9872-722A-4D67-BED2-ED039C615D9F",@"False");  
            // Add Block Attribute Value              
            //   Block: Media Folder List              
            //   BlockType: Media Folder List              
            //   Block Location: Page=Media Account, Site=Rock RMS              
            //   Attribute: core.EnableDefaultWorkflowLauncher              
            //   Attribute Value: True              
            RockMigrationHelper.AddBlockAttributeValue("14A0B30E-8287-4791-8443-0FAAB80FB559","9950DA6D-C175-4E07-8988-7FF1E05DD02D",@"True");  
            // Add Block Attribute Value              
            //   Block: Media Element List              
            //   BlockType: Media Element List              
            //   Block Location: Page=Media Folder, Site=Rock RMS              
            //   Attribute: Detail Page              
            //   Attribute Value: f1ab34ee-941f-41d6-9ba1-22348d09724c              
            RockMigrationHelper.AddBlockAttributeValue("AA71BC08-DB91-43E3-BBB7-A03C698D1184","908960AB-4CFD-4DFD-A9B9-F60117DF4427",@"f1ab34ee-941f-41d6-9ba1-22348d09724c");  
            // Add Block Attribute Value              
            //   Block: Media Element List              
            //   BlockType: Media Element List              
            //   Block Location: Page=Media Folder, Site=Rock RMS              
            //   Attribute: core.CustomGridEnableStickyHeaders              
            //   Attribute Value: False              
            RockMigrationHelper.AddBlockAttributeValue("AA71BC08-DB91-43E3-BBB7-A03C698D1184","7EB0DC76-BD62-4ADE-8158-CB30F16B510E",@"False");  
            // Add Block Attribute Value              
            //   Block: Media Element List              
            //   BlockType: Media Element List              
            //   Block Location: Page=Media Folder, Site=Rock RMS              
            //   Attribute: core.EnableDefaultWorkflowLauncher              
            //   Attribute Value: True              
            RockMigrationHelper.AddBlockAttributeValue("AA71BC08-DB91-43E3-BBB7-A03C698D1184","3A13C9B6-F02C-4D72-8D41-F1BE0218C141",@"True");  

        }

        /// <summary>
        /// Operations to be performed during the downgrade process.
        /// </summary>
        public override void Down()
        {
            DropForeignKey("dbo.MediaAccount", "ModifiedByPersonAliasId", "dbo.PersonAlias");
            DropForeignKey("dbo.MediaFolder", "ModifiedByPersonAliasId", "dbo.PersonAlias");
            DropForeignKey("dbo.MediaElement", "ModifiedByPersonAliasId", "dbo.PersonAlias");
            DropForeignKey("dbo.MediaElement", "MediaFolderId", "dbo.MediaFolder");
            DropForeignKey("dbo.MediaElement", "CreatedByPersonAliasId", "dbo.PersonAlias");
            DropForeignKey("dbo.MediaFolder", "MediaAccountId", "dbo.MediaAccount");
            DropForeignKey("dbo.MediaFolder", "CreatedByPersonAliasId", "dbo.PersonAlias");
            DropForeignKey("dbo.MediaFolder", "ContentChannelAttributeId", "dbo.Attribute");
            DropForeignKey("dbo.MediaFolder", "ContentChannelId", "dbo.ContentChannel");
            DropForeignKey("dbo.MediaAccount", "CreatedByPersonAliasId", "dbo.PersonAlias");
            DropForeignKey("dbo.MediaAccount", "ComponentEntityTypeId", "dbo.EntityType");
            DropIndex("dbo.MediaElement", new[] { "Guid" });
            DropIndex("dbo.MediaElement", new[] { "ModifiedByPersonAliasId" });
            DropIndex("dbo.MediaElement", new[] { "CreatedByPersonAliasId" });
            DropIndex("dbo.MediaElement", new[] { "MediaFolderId" });
            DropIndex("dbo.MediaFolder", new[] { "Guid" });
            DropIndex("dbo.MediaFolder", new[] { "ModifiedByPersonAliasId" });
            DropIndex("dbo.MediaFolder", new[] { "CreatedByPersonAliasId" });
            DropIndex("dbo.MediaFolder", new[] { "ContentChannelAttributeId" });
            DropIndex("dbo.MediaFolder", new[] { "ContentChannelId" });
            DropIndex("dbo.MediaFolder", new[] { "MediaAccountId" });
            DropIndex("dbo.MediaAccount", new[] { "Guid" });
            DropIndex("dbo.MediaAccount", new[] { "ModifiedByPersonAliasId" });
            DropIndex("dbo.MediaAccount", new[] { "CreatedByPersonAliasId" });
            DropIndex("dbo.MediaAccount", new[] { "ComponentEntityTypeId" });
            DropTable("dbo.MediaElement");
            DropTable("dbo.MediaFolder");
            DropTable("dbo.MediaAccount");

            // core.EnableDefaultWorkflowLauncher Attribute for BlockType: Media Folder List              
            RockMigrationHelper.DeleteAttribute("9950DA6D-C175-4E07-8988-7FF1E05DD02D");
            // core.CustomActionsConfigs Attribute for BlockType: Media Folder List              
            RockMigrationHelper.DeleteAttribute("AE584E4E-4179-4D5C-A60A-0AACB7029024");
            // Detail Page Attribute for BlockType: Media Folder List              
            RockMigrationHelper.DeleteAttribute("2B01D2A0-A67B-4BD9-B196-96CD9B649BAD");
            // core.EnableDefaultWorkflowLauncher Attribute for BlockType: Media Element List              
            RockMigrationHelper.DeleteAttribute("3A13C9B6-F02C-4D72-8D41-F1BE0218C141");
            // core.CustomActionsConfigs Attribute for BlockType: Media Element List              
            RockMigrationHelper.DeleteAttribute("67EFDE42-8E98-438A-85E3-89249B84912A");
            // Detail Page Attribute for BlockType: Media Element List              
            RockMigrationHelper.DeleteAttribute("908960AB-4CFD-4DFD-A9B9-F60117DF4427");
            // core.EnableDefaultWorkflowLauncher Attribute for BlockType: Media Account List              
            RockMigrationHelper.DeleteAttribute("FA22299B-8B86-4657-9AA2-D2C9B2C28921");
            // core.CustomActionsConfigs Attribute for BlockType: Media Account List              
            RockMigrationHelper.DeleteAttribute("33BC90A5-952B-4081-A25D-0CCA87E8426B");
            // Detail Page Attribute for BlockType: Media Account List              
            RockMigrationHelper.DeleteAttribute("D3C8A3CD-3E16-4244-BE6A-29C23662C065");
            
            // Remove Block: Media Element Detail, from Page: Media Element, Site: Rock RMS              
            RockMigrationHelper.DeleteBlock("104BF960-5167-4ADE-A26B-6F63762877E3");  
            // Remove Block: Media Element List, from Page: Media Folder, Site: Rock RMS              
            RockMigrationHelper.DeleteBlock("AA71BC08-DB91-43E3-BBB7-A03C698D1184");  
            // Remove Block: Media Folder Detail, from Page: Media Folder, Site: Rock RMS              
            RockMigrationHelper.DeleteBlock("37108EB5-2F1F-484F-BD2D-FEF8AD6DFC18");  
            // Remove Block: Media Folder List, from Page: Media Account, Site: Rock RMS              
            RockMigrationHelper.DeleteBlock("14A0B30E-8287-4791-8443-0FAAB80FB559");  
            // Remove Block: Media Account Detail, from Page: Media Account, Site: Rock RMS              
            RockMigrationHelper.DeleteBlock("ABAD84DA-113F-40E5-9DBD-ADA72F5B95B8");  
            // Remove Block: Media Account List, from Page: Media Accounts, Site: Rock RMS              
            RockMigrationHelper.DeleteBlock("C38FB340-FD4B-4BDE-A306-FE9B75D71A85");  
            
            // Delete BlockType Media Element Detail              
            RockMigrationHelper.DeleteBlockType("881DC0D1-FF98-4A5E-827F-49DD5CD0BD32"); // Media Element Detail  
            // Delete BlockType Media Folder List              
            RockMigrationHelper.DeleteBlockType("02A91579-9355-45E7-A67A-56E998FB332A"); // Media Folder List  
            // Delete BlockType Media Folder Detail              
            RockMigrationHelper.DeleteBlockType("3C9D442B-D066-43FA-9380-98C60936992E"); // Media Folder Detail  
            // Delete BlockType Media Element List              
            RockMigrationHelper.DeleteBlockType("28D6F57B-59D9-4DA6-A8DC-6DBD3E157554"); // Media Element List  
            // Delete BlockType Media Account List              
            RockMigrationHelper.DeleteBlockType("7537AB61-F80B-43B1-998B-1D2B03303B36"); // Media Account List  
            // Delete BlockType Media Account Detail              
            RockMigrationHelper.DeleteBlockType("0361FFC9-F32F-4C97-98BD-9DFE5F4A777E"); // Media Account Detail  
            
            // Delete Page Media Element from Site:Rock RMS              
            RockMigrationHelper.DeletePage("F1AB34EE-941F-41D6-9BA1-22348D09724C"); //  Page: Media Element, Layout: Full Width, Site: Rock RMS  
            // Delete Page Media Folder from Site:Rock RMS              
            RockMigrationHelper.DeletePage("65DE6218-2850-4924-AA55-6F6FB572E9A3"); //  Page: Media Folder, Layout: Full Width, Site: Rock RMS  
            // Delete Page Media Account from Site:Rock RMS              
            RockMigrationHelper.DeletePage("52548B49-6D09-467E-BEA9-04DD6F51637D"); //  Page: Media Account, Layout: Full Width, Site: Rock RMS  
            // Delete Page Media Accounts from Site:Rock RMS              
            RockMigrationHelper.DeletePage("07CB7BB5-1465-4E75-8DD4-28FA6EA48222"); //  Page: Media Accounts, Layout: Full Width, Site: Rock RMS  
        }
    }
}
