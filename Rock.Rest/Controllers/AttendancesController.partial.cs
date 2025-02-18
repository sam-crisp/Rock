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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using Rock.Chart;
using Rock.Data;
using Rock.Model;
using Rock.Rest.Filters;

namespace Rock.Rest.Controllers
{
    /// <summary>
    ///
    /// </summary>
    [RockGuid( "4fb7e1c9-b57e-4fa4-bdd6-61fd81746ac4" )]
    public partial class AttendancesController
    {
        /// <summary>
        /// Gets the chart data.
        /// </summary>
        /// <returns></returns>
        [Authenticate, Secured]
        [System.Web.Http.Route( "api/Attendances/GetChartData" )]
        [RockGuid( "7611d15b-55cd-4d84-96b5-1a11340d8d8a" )]
        public IEnumerable<IChartData> GetChartData( ChartGroupBy groupBy = ChartGroupBy.Week, AttendanceGraphBy graphBy = AttendanceGraphBy.Total, DateTime? startDate = null, DateTime? endDate = null, string groupIds = null, string campusIds = null, string scheduleIds = null, int? dataViewId = null )
        {
            return new AttendanceService( new RockContext() ).GetChartData( groupBy, graphBy, startDate, endDate, groupIds, campusIds, dataViewId, scheduleIds );
        }

        /// <summary>
        /// Adds an attendance. If the AttendanceOccurrence record does not exist it is created. If the Attendance record already exists then it is updated.
        /// </summary>
        /// <param name="groupId">The group identifier.</param>
        /// <param name="locationId">The location identifier.</param>
        /// <param name="scheduleId">The schedule identifier.</param>
        /// <param name="occurrenceDate">The occurrence date.</param>
        /// <param name="personId">The person identifier. If provided it is used to get the primary PersonAliasId and takes precedence over "personAliasId"</param>
        /// <param name="personAliasId">The person alias identifier. Is not used if a "personId" is provided.</param>
        /// <returns>Attendance</returns>
        [Authenticate, Secured]
        [System.Web.Http.Route( "api/Attendances/AddAttendance" )]
        [HttpPut]
        [RockGuid( "68ff77eb-7a7c-4763-a090-0f917cc2e033" )]
        public Attendance AddAttendance( int groupId, int locationId, int scheduleId, DateTime occurrenceDate, int? personId = null, int? personAliasId = null )
        {
            using ( var rockContext = new RockContext() )
            {
                // If personId is provided set the personAliasId to the primary alias of the person.
                if ( personId != null )
                {
                    personAliasId = new PersonService( rockContext ).Get( personId.Value ).PrimaryAliasId;
                }

                if ( personAliasId == null )
                {
                    var response = new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        Content = new StringContent( "PersonId or PersonAliasId is required." )
                    };

                    throw new HttpResponseException( response );
                }

                var attendance = new AttendanceService( rockContext ).AddOrUpdate( personAliasId.Value, occurrenceDate, groupId, locationId, scheduleId, null );
                rockContext.SaveChanges();
                return attendance;
            }
        }

        #region Group Scheduler Related

        /// <summary>
        /// Gets a list of available the scheduler resources (people) based on the options specified in schedulerResourceParameters
        /// </summary>
        /// <param name="schedulerResourceParameters">The scheduler resource parameters.</param>
        /// <returns></returns>
        [Authenticate, Secured]
        [System.Web.Http.Route( "api/Attendances/GetSchedulerResources" )]
        [HttpPost]
        [RockGuid( "ddbddbe0-7032-49b2-82e7-08dba28f1fc1" )]
        public IEnumerable<SchedulerResource> GetSchedulerResources( [FromBody] SchedulerResourceParameters schedulerResourceParameters )
        {
            var rockContext = new RockContext();
            var attendanceService = new AttendanceService( rockContext );
            return attendanceService.GetSchedulerResources( schedulerResourceParameters );
        }

        /// <summary>
        /// Gets an individual scheduler resource.
        /// </summary>
        /// <param name="schedulerResourceParameters">The scheduler resource parameters.</param>
        /// <param name="personId">The person identifier.</param>
        /// <returns></returns>
        [Authenticate, Secured]
        [System.Web.Http.Route( "api/Attendances/GetSchedulerResource" )]
        [HttpPost]
        [RockGuid( "082bc954-89ca-4d73-a971-72446b84e92c" )]
        public SchedulerResource GetSchedulerResource( [FromBody] SchedulerResourceParameters schedulerResourceParameters, int personId )
        {
            var rockContext = new RockContext();
            var attendanceService = new AttendanceService( rockContext );
            schedulerResourceParameters.LimitToPersonId = personId;
            var result = attendanceService.GetSchedulerResources( schedulerResourceParameters ).FirstOrDefault();
            return result;
        }

        /// <summary>
        /// Gets a list of scheduled attendances ( people that are scheduled ) for an attendance occurrence
        /// </summary>
        /// <param name="attendanceOccurrenceId">The attendance occurrence identifier.</param>
        /// <returns></returns>
        [Authenticate, Secured]
        [System.Web.Http.Route( "api/Attendances/GetAttendingSchedulerResources" )]
        [HttpGet]
        [RockGuid( "cde17f35-da0a-47d9-80ef-994def9fb946" )]
        public IEnumerable<SchedulerResourceAttend> GetAttendingSchedulerResources( int attendanceOccurrenceId )
        {
            var rockContext = new RockContext();
            var attendanceService = new AttendanceService( rockContext );

            return attendanceService.GetAttendingSchedulerResources( attendanceOccurrenceId );
        }

        /// <summary>
        /// Updates attendance record to indicate person is not pending, or confirmed, or declined
        /// </summary>
        /// <param name="attendanceId">The attendance identifier.</param>
        [Authenticate, Secured]
        [System.Web.Http.Route( "api/Attendances/ScheduledPersonRemove" )]
        [HttpPut]
        [RockGuid( "5621a903-ef18-49d7-91de-73e27e5d2b5a" )]
        public void ScheduledPersonRemove( int attendanceId )
        {
            var rockContext = new RockContext();
            var attendanceService = new AttendanceService( rockContext );

            attendanceService.ScheduledPersonRemove( attendanceId );
            rockContext.SaveChanges();
        }

        /// <summary>
        /// Determines whether this the person can be scheduled for the specified occurrence
        /// </summary>
        /// <param name="personId">The person identifier.</param>
        /// <param name="attendanceOccurrenceId">The attendance occurrence identifier.</param>
        /// <param name="fromAttendanceOccurrenceId">From attendance occurrence identifier.</param>
        /// <returns></returns>
        [Authenticate, Secured]
        [HttpGet]
        [System.Web.Http.Route( "api/Attendances/CanSchedulePerson" )]
        [RockGuid( "3b23b14a-9d00-4d73-95bf-c6c22c1d07f8" )]
        public virtual HttpResponseMessage CanSchedulePerson( int personId, int attendanceOccurrenceId, int? fromAttendanceOccurrenceId = null )
        {
            var rockContext = new RockContext();
            var attendanceService = new AttendanceService( rockContext );

            var attendanceOccurrenceInfo = new AttendanceOccurrenceService( rockContext ).GetSelect( attendanceOccurrenceId, s => new
            {
                s.ScheduleId,
                s.OccurrenceDate
            } );

            if ( attendanceOccurrenceInfo == null )
            {
                return new HttpResponseMessage( HttpStatusCode.NotFound );
            }

            var scheduleId = attendanceOccurrenceInfo.ScheduleId;
            var occurrenceDate = attendanceOccurrenceInfo.OccurrenceDate;

            // Only count occurrences with active locations and active schedules as conflicting.
            var conflictingScheduledAttendancesQuery = attendanceService.Queryable().WhereDeducedIsActive();

            if ( fromAttendanceOccurrenceId.HasValue )
            {
                // if person was dragged from one occurrence to another, we can exclude both source and target occurrences when detecting conflicts
                conflictingScheduledAttendancesQuery = conflictingScheduledAttendancesQuery
                .Where( c => ( c.OccurrenceId != attendanceOccurrenceId ) && ( c.OccurrenceId != fromAttendanceOccurrenceId.Value ) );
            }
            else
            {
                // if person was dragged from resources to an occurrence, we can exclude the target occurrences when detecting conflicts
                conflictingScheduledAttendancesQuery = conflictingScheduledAttendancesQuery
                .Where( c => c.OccurrenceId != attendanceOccurrenceId );
            }

            // a conflict would be if the same person is requested/scheduled for another attendance within the same ScheduleId/Date
            conflictingScheduledAttendancesQuery = conflictingScheduledAttendancesQuery.Where( c =>
                c.PersonAlias.PersonId == personId
                && ( c.RequestedToAttend == true || c.ScheduledToAttend == true )
                && c.Occurrence.ScheduleId == scheduleId
                && c.Occurrence.OccurrenceDate == occurrenceDate );

            if ( conflictingScheduledAttendancesQuery.Any() )
            {
                var person = new PersonService( rockContext ).Get( personId );
                var firstConflict = conflictingScheduledAttendancesQuery.Select( s => new
                {
                    GroupName = s.Occurrence.Group.Name,
                    LocationName = s.Occurrence.Location.Name
                } ).FirstOrDefault();

                return ControllerContext.Request.CreateErrorResponse(
                        HttpStatusCode.BadRequest,
                        $"{person} cannot be scheduled due a scheduling conflict with {firstConflict?.GroupName} in the {firstConflict?.LocationName}." );
            }

            return new HttpResponseMessage( HttpStatusCode.OK );
        }

        /// <summary>
        /// Schedules a person to an attendance and marks them as pending
        /// </summary>
        /// <param name="personId">The person identifier.</param>
        /// <param name="attendanceOccurrenceId">The attendance occurrence identifier.</param>
        [Authenticate, Secured]
        [System.Web.Http.Route( "api/Attendances/ScheduledPersonAddPending" )]
        [HttpPut]
        [RockGuid( "12ab7295-caea-4308-a63c-934e7afd8605" )]
        public Attendance ScheduledPersonAddPending( int personId, int attendanceOccurrenceId )
        {
            var rockContext = new RockContext();
            var attendanceService = new AttendanceService( rockContext );

            var currentPersonAlias = this.GetPersonAlias();

            var result = attendanceService.ScheduledPersonAddPending( personId, attendanceOccurrenceId, currentPersonAlias );
            rockContext.SaveChanges();

            return result;
        }

        /// <summary>
        /// Schedules a person to an attendance and immediately marks them as confirmed
        /// </summary>
        /// <param name="personId">The person identifier.</param>
        /// <param name="attendanceOccurrenceId">The attendance occurrence identifier.</param>
        /// <returns></returns>
        [Authenticate, Secured]
        [System.Web.Http.Route( "api/Attendances/ScheduledPersonAddConfirmed" )]
        [HttpPut]
        [RockGuid( "5517065d-0c65-4fd4-95cc-c676aaba64fe" )]
        public Attendance ScheduledPersonAddConfirmed( int personId, int attendanceOccurrenceId )
        {
            var rockContext = new RockContext();
            var attendanceService = new AttendanceService( rockContext );

            var currentPersonAlias = this.GetPersonAlias();

            var result = attendanceService.ScheduledPersonAddPending( personId, attendanceOccurrenceId, currentPersonAlias );
            rockContext.SaveChanges();
            var attendanceId = result.Id;

            attendanceService.ScheduledPersonConfirm( attendanceId );

            rockContext.SaveChanges();

            return result;
        }

        /// <summary>
        /// Sets a person's status to pending for the scheduled attendance
        /// </summary>
        /// <param name="attendanceId">The attendance identifier.</param>
        [Authenticate, Secured]
        [System.Web.Http.Route( "api/Attendances/ScheduledPersonPending" )]
        [HttpPut]
        [RockGuid( "27304d4e-58f7-4b27-861d-e505ff010579" )]
        public void ScheduledPersonPending( int attendanceId )
        {
            var rockContext = new RockContext();
            var attendanceService = new AttendanceService( rockContext );
            attendanceService.ScheduledPersonPending( attendanceId );

            rockContext.SaveChanges();
        }

        /// <summary>
        /// Confirms a person for a scheduled attendance
        /// </summary>
        /// <param name="attendanceId">The attendance identifier.</param>
        [Authenticate, Secured]
        [System.Web.Http.Route( "api/Attendances/ScheduledPersonConfirm" )]
        [HttpPut]
        [RockGuid( "35e8d73c-5783-40ab-b874-cd4a63d14caa" )]
        public void ScheduledPersonConfirm( int attendanceId )
        {
            var rockContext = new RockContext();
            var attendanceService = new AttendanceService( rockContext );
            attendanceService.ScheduledPersonConfirm( attendanceId );

            rockContext.SaveChanges();
        }

        /// <summary>
        /// Set person as declined for a scheduled attendance
        /// </summary>
        /// <param name="attendanceId">The attendance identifier.</param>
        [Authenticate, Secured]
        [System.Web.Http.Route( "api/Attendances/ScheduledPersonDecline" )]
        [HttpPut]
        [RockGuid( "f9c2e4cb-a3b5-4caf-a678-0c543774f736" )]
        public void ScheduledPersonDecline( int attendanceId )
        {
            var rockContext = new RockContext();
            var attendanceService = new AttendanceService( rockContext );
            attendanceService.ScheduledPersonDecline( attendanceId, null );

            rockContext.SaveChanges();
        }

        /// <summary>
        /// Sends (or Re-sends) a confirmation email to the person in the specified scheduled attendance record
        /// </summary>
        /// <param name="attendanceId">The attendance identifier.</param>
        [Authenticate, Secured]
        [System.Web.Http.Route( "api/Attendances/ScheduledPersonSendConfirmationEmail" )]
        [HttpPut]
        [Obsolete( "Use ScheduledPersonSendConfirmationCommunication instead." )]
        [RockObsolete( "1.13" )]
        [RockGuid( "8531ed24-d82a-4ffb-8f5c-3848fcc04a44" )]
        public void ScheduledPersonSendConfirmationEmail( int attendanceId )
        {
            var rockContext = new RockContext();
            var attendanceService = new AttendanceService( rockContext );
            var sendConfirmationAttendancesQuery = attendanceService.Queryable().Where( a => a.Id == attendanceId );
            List<string> errorMessages;
            attendanceService.SendScheduleConfirmationSystemEmails( sendConfirmationAttendancesQuery, out errorMessages );
            rockContext.SaveChanges();
        }

        /// <summary>
        /// Sends (or Re-sends) a confirmation email to the person in the specified scheduled attendance record
        /// </summary>
        /// <param name="attendanceId">The attendance identifier.</param>
        [Authenticate, Secured]
        [System.Web.Http.Route( "api/Attendances/ScheduledPersonSendConfirmationCommunication" )]
        [HttpPut]
        [RockGuid( "b126e3e7-5b54-4a91-8214-2006bb4d3deb" )]
        public void ScheduledPersonSendConfirmationCommunication( int attendanceId )
        {
            var rockContext = new RockContext();
            var attendanceService = new AttendanceService( rockContext );
            var sendConfirmationAttendancesQuery = attendanceService.Queryable().Where( a => a.Id == attendanceId );

            attendanceService.SendScheduleConfirmationCommunication( sendConfirmationAttendancesQuery );

            rockContext.SaveChanges();
        }

        #endregion Group Scheduler Related

        #region RSVP Related

        /// <summary>
        /// This method is deprecated and should not be used as it is subject to the limits of the maximum URL length of the browser.
        /// Use the method which accepts the list of Person Ids in the body of the request, instead.
        /// </summary>
        /// <param name="occurrenceId">The ID of the AttendanceOccurrence record.</param>
        /// <param name="personIds">A comma-delimited list of Person IDs.</param>
        [Authenticate, Secured]
        [System.Web.Http.Route( "api/Attendances/RegisterRSVPRecipients" )]
        [HttpPost]
        [Obsolete( "Use the method which accepts a List<string> parameter instead." )]
        [RockObsolete( "1.10.4" )]
        [RockGuid( "5a9f9da7-d669-4d0d-8914-1dc1f21bb0fa" )]
        public void RegisterRSVPRecipients( int occurrenceId, string personIds )
        {
            new AttendanceService( new RockContext() )
                .RegisterRSVPRecipients( occurrenceId, personIds );
        }

        /// <summary>
        /// Creates attendance records if they don't exist for a designated occurrence and list of person IDs.
        /// </summary>
        /// <param name="occurrenceId">The ID of the AttendanceOccurrence record.</param>
        /// <param name="personIds">A list of Person IDs.</param>
        [Authenticate, Secured]
        [System.Web.Http.Route( "api/Attendances/RegisterRSVPRecipients" )]
        [HttpPost]
        [RockGuid( "e3c50a1c-df4e-4828-a0d0-a0bed27edbe1" )]
        public void RegisterRSVPRecipients( int occurrenceId, [FromBody] List<string> personIds )
        {
            var personIdList = personIds.Select( int.Parse ).ToList();
            new AttendanceService( new RockContext() )
                .RegisterRSVPRecipients( occurrenceId, personIdList );
        }

        #endregion RSVP Related

        #region Import related

        /// <summary>
        /// Import Attendance Records using BulkInsert
        /// </summary>
        /// <remarks>
        /// For best performance, limit to 1000 records at a time.
        /// Either the PersonId or PersonAliasId value can be specified, but at least one is required.
        /// </remarks>
        /// <param name="attendancesImport">The Attendances to bulk import.</param>
        [Authenticate, Secured]
        [HttpPost]
        [System.Web.Http.Route( "api/Attendances/Import" )]
        [RockGuid( "19fb3aa7-5e40-4186-aa25-e4978ffcefc4" )]
        public void AttendanceImport( Rock.BulkImport.AttendancesImport attendancesImport )
        {
            if ( attendancesImport == null )
            {
                var response = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent( "AttendancesImport data is required" )
                };

                throw new HttpResponseException( response );
            }

            AttendanceService.BulkAttendanceImport( attendancesImport );
        }

        #endregion Import related
    }
}
