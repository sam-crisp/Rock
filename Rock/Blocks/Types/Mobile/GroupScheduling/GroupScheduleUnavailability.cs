using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rock.Attribute;
using Rock.Data;
using Rock.Model;
using Rock.Utility;

namespace Rock.Blocks.Types.Mobile.GroupScheduling
{
    /// <summary>
    /// The mobile block adaptation of GroupScheduleToolboxV2
    /// </summary>
    /// <seealso cref="Rock.Blocks.RockMobileBlockType" />

    [DisplayName( "Schedule Unavailability" )]
    [Category( "Mobile > Group Scheduling" )]
    [Description( "Allows a user to schedule the dates that they are unavailable to serve." )]
    [IconCssClass( "fa fa-list" )]

    #region Block Attributes

    [BlockTemplateField( "Schedule Unavailability Template",
        Description = "The template used to render the scheduling data.",
        TemplateBlockValueGuid = SystemGuid.DefinedValue.BLOCK_TEMPLATE_MOBILE_GROUP_SCHEDULE_UNAVAILABILITY,
        DefaultValue = "1E41A521-B584-4F07-922B-A49253B4AAEF",
        IsRequired = true,
        Key = AttributeKey.TypeTemplate,
        Order = 0 )]

    [BooleanField( "Description required?",
        Description = "Whether or not the user is required to input a description.",
        Key = AttributeKey.IsDescriptionRequired,
        IsRequired = true,
        DefaultBooleanValue = false,
        Order = 1 )]

    #endregion

    public class GroupScheduleUnavailability : RockMobileBlockType
    {

        #region Block Attributes

        private static class AttributeKey
        {
            public const string TypeTemplate = "TypeTemplate";
            public const string IsDescriptionRequired = "IsDescriptionRequired";
        }

        /// <summary>
        /// Gets the type template.
        /// </summary>
        /// <value>
        /// The type template.
        /// </value>
        protected string TypeTemplate => Field.Types.BlockTemplateFieldType.GetTemplateContent( GetAttributeValue( AttributeKey.TypeTemplate ) );

        /// <summary>
        /// Gets the current person ID, or 0 if unable to.
        /// </summary>
        public int CurrentPersonId
        {
            get => RequestContext.CurrentPerson?.Id ?? 0;
        }

        /// <summary>
        /// 
        /// </summary>
        protected bool DescriptionRequired => GetAttributeValue( AttributeKey.IsDescriptionRequired ).AsBoolean();

        #endregion

        #region IRockMobileBlockType Implementation

        /// <summary>
        /// Gets the required mobile application binary interface version.
        /// </summary>
        /// <value>
        /// The required mobile application binary interface version.
        /// </value>
        public override int RequiredMobileAbiVersion => 3;

        /// <summary>
        /// Gets the class name of the mobile block to use during rendering on the device.
        /// </summary>
        /// <value>
        /// The class name of the mobile block to use during rendering on the device
        /// </value>
        public override string MobileBlockType => "Rock.Mobile.Blocks.GroupScheduling.GroupScheduleUnavailability";

        /// <summary>
        /// Gets the mobile configuration.
        /// </summary>
        /// <returns></returns>
        public override object GetMobileConfigurationValues()
        {
            return new
            {
            };
        }

        #endregion

        #region Methods

        private GetContentViewModel GetScheduleContent()
        {
            var rockContext = new RockContext();

            // The dictionary of merge fields
            var mergeFields = RequestContext.GetCommonMergeFields();

            // Get the current user's family members
            var familyMemberAliasIds = new PersonService( rockContext )
                .GetFamilyMembers( this.CurrentPersonId, true )
                .SelectMany( m => m.Person.Aliases )
                .Select( a => a.Id ).ToList();

            var currentDateTime = RockDateTime.Now.Date;

            var personScheduleExclusionService = new PersonScheduleExclusionService( rockContext );

            // Get the schedule exclusions
            var personScheduleExclusions = personScheduleExclusionService
                .Queryable( "PersonAlias.Person" )
                .AsNoTracking()
                .Where( e => familyMemberAliasIds.Contains( e.PersonAliasId.Value ) )
                .Where( e => e.StartDate >= currentDateTime || e.EndDate >= currentDateTime )
                .OrderBy( e => e.StartDate )
                .ThenBy( e => e.EndDate )
                .Select( e => new GroupScheduleRowInfo
                {
                    Title = e.Title,
                    Guid = e.Guid,
                    Id = e.Id,
                    OccurrenceStartDate = DbFunctions.TruncateTime( e.StartDate ).Value,
                    OccurrenceEndDate = DbFunctions.TruncateTime( e.EndDate ).Value,
                    Group = e.Group,
                    PersonAlias = e.PersonAlias,
                    GroupScheduleType = GroupScheduleType.Unavailable
                } );

            var groupService = new GroupService( rockContext );
            // get groups that the selected person is an active member of and have SchedulingEnabled and have at least one location with a schedule
            var groups = groupService
                .Queryable()
                .AsNoTracking()
                .Where( x => x.Members.Any( m => m.PersonId == this.CurrentPersonId && m.IsArchived == false && m.GroupMemberStatus == GroupMemberStatus.Active ) )
                .Where( x => x.IsActive == true && x.IsArchived == false
                    && x.GroupType.IsSchedulingEnabled == true
                    && x.DisableScheduling == false
                    && x.DisableScheduleToolboxAccess == false )
                .Where( x => x.GroupLocations.Any( gl => gl.Schedules.Any() ) )
                .OrderBy( x => new { x.Order, x.Name } )
                .AsNoTracking()
                .ToList();

            // A list of the group names and guids
            List<GroupNameAndGuid> groupsList = new List<GroupNameAndGuid>();

            // Every result from our query
            foreach ( var group in groups )
            {
                // Add it to the groups list with the necessary information
                var groupInformation = new GroupNameAndGuid
                {
                    Name = group.Name,
                    Guid = group.Guid
                };

                groupsList.Add( groupInformation );
            }

            // Getting the family members
            var personService = new PersonService( rockContext );

            var familyMembersQuery = personService.GetFamilyMembers( this.CurrentPersonId, false )
                .AsNoTracking()
                .ToList();

            // A list of family member names and guids
            List<FamilyMemberNameAndGuid> familyMembersList = new List<FamilyMemberNameAndGuid>();

            foreach ( var familyMember in familyMembersQuery )
            {
                // Add it to the family members list with the necessary information
                var familyMemberInformation = new FamilyMemberNameAndGuid
                {
                    Name = familyMember.Person.FullName,
                    Guid = familyMember.Guid
                };

                familyMembersList.Add( familyMemberInformation );
            }

            // Add in our list of schedule exclusions
            mergeFields.AddOrReplace( "ScheduleExclusionsList", personScheduleExclusions );

            // Pass those in as content
            var content = TypeTemplate.ResolveMergeFields( mergeFields );

            // Return all of the necessary information
            return new GetContentViewModel
            {
                Content = content,
                GroupInformation = groupsList,
                FamilyMembersInformation = familyMembersList,
                IsDescriptionRequired = DescriptionRequired
            };
        }

        #endregion

        #region Block Actions

        /// <summary>
        /// Deletes a scheduled unavailability request
        /// </summary>
        /// <returns></returns>
        [BlockAction]
        public BlockActionResult DeleteScheduledUnavailability( Guid? attendanceGuid )
        {
            // If the parameter is empty
            if ( attendanceGuid == null )
            {
                return ActionBadRequest( "Failed to get the specified schedule exclusion. Please try again." );
            }

            var rockContext = new RockContext();

            // The schedule exclusion service
            var scheduleExclusionService = new PersonScheduleExclusionService( rockContext );

            // Get the specific scheduled exclusion from the Guid
            var scheduleExclusion = scheduleExclusionService.GetNoTracking( attendanceGuid.Value );

            if ( scheduleExclusion != null )
            {
                // Get the person schedule exclusion
                var personScheduleExclusion = scheduleExclusionService.Get( scheduleExclusion.Id );

                if ( personScheduleExclusion != null )
                {
                    // Find all of the children
                    var scheduleExclusionChildren = scheduleExclusionService.Queryable().Where( x => x.ParentPersonScheduleExclusionId == personScheduleExclusion.Id );

                    foreach ( var scheduleExclusionChild in scheduleExclusionChildren )
                    {
                        scheduleExclusionChild.ParentPersonScheduleExclusionId = null;
                    }

                    // Delete the exclusion
                    scheduleExclusionService.Delete( personScheduleExclusion );

                    rockContext.SaveChanges();
                }
            }

            return ActionOk();
        }

        /// <summary>
        /// Gets the scheduling information to be displayed.
        /// </summary>
        /// <returns>A response that describes the result of the operation.</returns>
        [BlockAction]
        public BlockActionResult GetContent()
        {
            // Here, we are going to get the schedule content and return it in a way that mobile can read easily.
            return ActionOk( GetScheduleContent() );
        }

        /// <summary>
        /// Sets the schedule unavailability.
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="description"></param>
        /// <param name="groupGuid"></param>
        /// <param name="personGuids"></param>
        /// <returns></returns>
        [BlockAction]
        public BlockActionResult AddScheduleUnavailability( DateTime startDate, DateTime endDate, string description, Guid? groupGuid, IEnumerable<string> personGuids )
        {
            // If we didn't receive any Guids
            if ( personGuids == null )
            {
                return ActionBadRequest();
            }

            int? parentId = null;
            // Loop through each Guid we need to schedule the blackout for.
            foreach ( var Guid in personGuids )
            {
                using ( var rockContext = new RockContext() )
                {
                    // First, attempt to get the person using the group member service
                    var person = new GroupMemberService( rockContext ).GetNoTracking( Guid.AsGuid() );

                    // If we are unable to get that person, try with the PersonService (means that the submitter was the same as the person requesting the blackout dates).
                    int? primaryAliasId = null;
                    if ( person == null )
                    {
                        primaryAliasId = new PersonService( rockContext ).GetNoTracking( Guid.AsGuid() ).PrimaryAliasId;
                    }

                    // We did get the person, so set the primary alias id
                    else
                    {
                        primaryAliasId = person.Person.PrimaryAliasId;
                    }

                    int? groupId = null;

                    // If there was a specified group, if "null" it assumes all groups.
                    if ( groupGuid.HasValue )
                    {
                        groupId = new GroupService( rockContext ).Get( groupGuid.Value ).Id;
                    }


                    // Create the exclusion
                    var personScheduleExclusion = new PersonScheduleExclusion
                    {
                        PersonAliasId = primaryAliasId,
                        StartDate = startDate.Date,
                        EndDate = endDate.Date,
                        GroupId = groupId,
                        Title = description,
                        ParentPersonScheduleExclusionId = parentId
                    };

                    if ( parentId == null )
                    {
                        parentId = personScheduleExclusion.ParentPersonScheduleExclusionId;
                    }

                    // Submit the exclusion and save the changes
                    new PersonScheduleExclusionService( rockContext ).Add( personScheduleExclusion );
                    rockContext.SaveChanges();
                }
            }
            return ActionOk();
        }
        #endregion

        #region Helper Classes

        /// <summary>
        /// Gets the content view model (XAML)
        /// </summary>
        private class GetContentViewModel
        {
            // The XAML in reference
            public string Content { get; set; }

            /// <summary>
            /// A list containing group member names and Guids
            /// </summary>
            public List<GroupNameAndGuid> GroupInformation { get; set; }

            /// <summary>
            /// A list containing family member's names and Guids
            /// </summary>
            public List<FamilyMemberNameAndGuid> FamilyMembersInformation { get; set; }

            public bool IsDescriptionRequired { get; set; }
        }

        private class GroupNameAndGuid
        {
            /// <summary>
            /// The group member Guid
            /// </summary>
            public Guid Guid { get; set; }

            /// <summary>
            /// The group member name
            /// </summary>
            public string Name { get; set; }
        }

        private class FamilyMemberNameAndGuid
        {
            /// <summary>
            /// The family member Guid
            /// </summary>
            public Guid Guid { get; set; }

            /// <summary>
            /// The family member name
            /// </summary>
            public string Name { get; set; }
        }

        private class GroupScheduleRowInfo : RockDynamic
        {
            /// <summary>
            /// Gets or sets the Guid
            /// </summary>
            /// <value>
            /// The Guid
            /// </value>
            public Guid Guid { get; set; }

            /// <summary>
            /// Gets or sets the identifier.
            /// </summary>
            /// <value>
            /// The identifier.
            /// </value>
            public int Id { get; set; }

            /// <summary>
            /// Gets or sets the date of the Attendance. Only the date is used.
            /// </summary>
            /// <value>
            /// A <see cref="System.DateTime"/> representing the start date and time/check in date and time.
            /// </value>
            public DateTime OccurrenceStartDate { get; set; }

            /// <summary>
            /// Gets or sets the end date of the Attendance. Only the date is used.
            /// </summary>
            /// <value>
            /// A <see cref="System.DateTime"/> representing the end date and time/check in date and time.
            /// </value>
            public DateTime OccurrenceEndDate { get; set; }

            /// <summary>
            /// Gets or sets the <see cref="Rock.Model.Group"/>.
            /// </summary>
            /// <value>
            /// The <see cref="Rock.Model.Group"/>.
            /// </value>
            public Group Group { get; set; }

            /// <summary>
            /// Gets or sets the <see cref="Rock.Model.Location"/> where the Person attended.
            /// </summary>
            /// <value>
            /// The <see cref="Rock.Model.Location"/> where the <see cref="Rock.Model.Person"/> attended.
            /// </value>
            public Location Location { get; set; }

            /// <summary>
            /// Gets or sets the <see cref="Rock.Model.Schedule"/>.
            /// </summary>
            /// <value>
            /// The schedule.
            /// </value>
            public Schedule Schedule { get; set; }

            /// <summary>
            /// Gets or sets the GroupScheduleType.
            /// </summary>
            /// <value>
            /// The GroupScheduleType.
            /// </value>
            public GroupScheduleType GroupScheduleType { get; set; }

            /// <summary>
            /// Gets or sets the person alias identifier of the Person that this exclusion is for
            /// </summary>
            /// <value>
            /// The person alias identifier.
            /// </value>
            public PersonAlias PersonAlias { get; set; }

            /// <summary>
            /// The title, or description of the schedule exclusion
            /// </summary>
            /// <value>
            /// The title.
            /// </value>
            public string Title { get; set; }
        }


        /// <summary>
        /// The group schedule type enum
        /// </summary>
        private enum GroupScheduleType
        {
            /// <summary>
            /// Pending
            /// </summary>
            Pending = 0,

            /// <summary>
            /// Upcoming
            /// </summary>
            Upcoming = 1,

            /// <summary>
            /// Unavailable
            /// </summary>
            Unavailable = 2
        }

        #endregion
    }
}
