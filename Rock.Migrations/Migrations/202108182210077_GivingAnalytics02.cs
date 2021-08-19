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
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    /// <summary>
    ///
    /// </summary>
    public partial class GivingAnalytics02 : Rock.Migrations.RockMigration
    {
        /// <summary>
        /// Operations to be performed during the upgrade process.
        /// </summary>
        public override void Up()
        {

            var givingAnalyticsCategory = new List<string>() { SystemGuid.Category.PERSON_ATTRIBUTES_GIVING_ANALYTICS };

            RockMigrationHelper.AddOrUpdatePersonAttributeByGuid(
                SystemGuid.FieldType.SINGLE_SELECT,
                givingAnalyticsCategory,
                "Previous Journey Giving Stage",
                "",
                "PreviousJourneyGivingStage",
                "",
                "",
                2010,
                "",
                SystemGuid.Attribute.PERSON_GIVING_PREVIOUS_GIVING_JOURNEY_STAGE );


            RockMigrationHelper.AddAttributeQualifier( SystemGuid.Attribute.PERSON_GIVING_PREVIOUS_GIVING_JOURNEY_STAGE, "fieldtype", "ddl", "831BF3D6-3873-4C03-852F-0FC58AD883F6" );
            RockMigrationHelper.AddAttributeQualifier( SystemGuid.Attribute.PERSON_GIVING_PREVIOUS_GIVING_JOURNEY_STAGE, "repeatColumns", "", "0C9C5A69-E8F8-447F-9A40-CB9FB5D2C6E4" );
            RockMigrationHelper.AddAttributeQualifier( SystemGuid.Attribute.PERSON_GIVING_PREVIOUS_GIVING_JOURNEY_STAGE, "values", "0^Non-Giver, 1^New Giver, 2^Occasional Giver, 3^Consistent Giver, 4^Lapsed Giver, 5^Former Giver", "4B61627F-3B3A-4150-9F79-01CB023439FC" );

            RockMigrationHelper.AddOrUpdatePersonAttributeByGuid(
                SystemGuid.FieldType.SINGLE_SELECT,
                givingAnalyticsCategory,
                "Current Journey Giving Stage",
                "",
                "CurrentJourneyGivingStage",
                "",
                "",
                2011,
                "",
                SystemGuid.Attribute.PERSON_GIVING_CURRENT_GIVING_JOURNEY_STAGE );

            RockMigrationHelper.AddAttributeQualifier( SystemGuid.Attribute.PERSON_GIVING_CURRENT_GIVING_JOURNEY_STAGE, "fieldtype", "ddl", "69BF55DD-2331-4112-9594-95C38F5A713B" );
            RockMigrationHelper.AddAttributeQualifier( SystemGuid.Attribute.PERSON_GIVING_CURRENT_GIVING_JOURNEY_STAGE, "repeatColumns", "", "C30349DC-3AF9-4C6A-A815-67B1BE2C9D91" );
            RockMigrationHelper.AddAttributeQualifier( SystemGuid.Attribute.PERSON_GIVING_CURRENT_GIVING_JOURNEY_STAGE, "values", "0^Non-Giver, 1^New Giver, 2^Occasional Giver, 3^Consistent Giver, 4^Lapsed Giver, 5^Former Giver", "1A9213FC-B567-4793-AF57-89F4C443FF02" );

            RockMigrationHelper.AddOrUpdatePersonAttributeByGuid(
                SystemGuid.FieldType.DATE,
                givingAnalyticsCategory,
                "Journey Giving Stage Change Date",
                "",
                "JourneyGivingStageChangeDate",
                "",
                "",
                2012,
                "",
                SystemGuid.Attribute.PERSON_GIVING_GIVING_JOURNEY_STAGE_CHANGE_DATE );

            RockMigrationHelper.AddAttributeQualifier( SystemGuid.Attribute.PERSON_GIVING_GIVING_JOURNEY_STAGE_CHANGE_DATE, "datePickerControlType", "Date Picker", "7C3E6647-E3BD-4B31-9170-3E2C77B042CD" );
            RockMigrationHelper.AddAttributeQualifier( SystemGuid.Attribute.PERSON_GIVING_GIVING_JOURNEY_STAGE_CHANGE_DATE, "displayCurrentOption", "False", "CFD9E6C2-568B-41E8-98BD-E6C5E7FC3FAE" );
            RockMigrationHelper.AddAttributeQualifier( SystemGuid.Attribute.PERSON_GIVING_GIVING_JOURNEY_STAGE_CHANGE_DATE, "displayDiff", "False", "68F54381-86EB-4779-BF1C-4EE70A26284E" );
            RockMigrationHelper.AddAttributeQualifier( SystemGuid.Attribute.PERSON_GIVING_GIVING_JOURNEY_STAGE_CHANGE_DATE, "format", "", "0DDC2021-CC42-4994-8AC7-0A861773A231" );
            RockMigrationHelper.AddAttributeQualifier( SystemGuid.Attribute.PERSON_GIVING_GIVING_JOURNEY_STAGE_CHANGE_DATE, "futureYearCount", "", "5B0BDD4C-0261-49C7-A61B-4FB089BB1F48" );
        }

        /// <summary>
        /// Operations to be performed during the downgrade process.
        /// </summary>
        public override void Down()
        {
        }
    }
}
