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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;

using Rock;
using Rock.Data;
using Rock.Model;
using Rock.Web.Cache;

using Ical.Net;
using Ical.Net.Serialization.iCalendar.Serializers;
using Ical.Net.DataTypes;
using Calendar = Ical.Net.Calendar;

using RestSharp.Extensions;
using System.Globalization;

namespace RockWeb
{
    /// <summary>
    /// Summary description for GetEventCalendarFeed
    /// </summary>
    public class GetEventCalendarFeed : IHttpHandler
    {
        private HttpRequest request;
        private HttpResponse response;
        private string interactionDeviceType;

        /// <summary>
        /// Gets a value indicating whether another request can use the <see cref="T:System.Web.IHttpHandler" /> instance.
        /// </summary>
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Processes the request.
        /// </summary>
        /// <param name="httpContext">The HTTP context.</param>
        public void ProcessRequest( HttpContext httpContext )
        {
            try
            {
                request = httpContext.Request;
                response = httpContext.Response;
                interactionDeviceType = InteractionDeviceType.GetClientType( request.UserAgent );

                if ( !ValidateSecurity( httpContext ) )
                {
                    return;
                }

                RockContext rockContext = new RockContext();
                CalendarProps calendarProps = ValidateRequestData( httpContext );

                if ( calendarProps == null )
                {
                    return;
                }

                var icalendar = CreateICalendar( calendarProps );

                var serializer = new CalendarSerializer();
                string s = serializer.SerializeToString( icalendar );

                response.Clear();
                response.ClearHeaders();
                response.ClearContent();
                response.AddHeader( "content-disposition", string.Format( "attachment; filename={0}_ical.ics", RockDateTime.Now.ToString( "yyyy-MM-dd_hhmmss" ) ) );
                response.ContentType = "text/calendar";
                response.Write( s );
            }
            catch (Exception ex)
            {
                ExceptionLogService.LogException( ex, httpContext );
                SendBadRequest( httpContext );
            }
        }

        /// <summary>
        /// Creates the iCalendar object and populates it with events
        /// </summary>
        /// <param name="calendarProps">The calendar props.</param>
        /// <returns></returns>
        private Calendar CreateICalendar( CalendarProps calendarProps )
        {
            // Get a list of Rock Calendar Events filtered by calendarProps
            List<EventItem> eventItems = GetEventItems( calendarProps );

            // Create the iCalendar.
            // Allow ICal to create the VTimeZone object from the local time zone to ensure that we get the correct name and daylight saving offset.
            var icalendar = new Calendar();

            var vtz = VTimeZone.FromLocalTimeZone();

            icalendar.AddTimeZone( vtz );

            var timeZoneId = vtz.TzId;

            // Create each of the events for the calendar(s)
            foreach ( EventItem eventItem in eventItems )
            {
                foreach ( EventItemOccurrence occurrence in eventItem.EventItemOccurrences )
                {
                    if ( occurrence.Schedule == null )
                    {
                        continue;
                    }

                    var serializer = new CalendarSerializer();
                    var ical = (CalendarCollection)serializer.Deserialize( occurrence.Schedule.iCalendarContent.ToStreamReader() );

                    foreach ( var icalEvent in ical[0].Events )
                    {
                        // We get all of the schedule info from Schedule.iCalendarContent
                        var ievent = icalEvent.Copy<Ical.Net.Event>();

                        ievent.Summary = !string.IsNullOrEmpty( eventItem.Name ) ? eventItem.Name : string.Empty;
                        ievent.Location = !string.IsNullOrEmpty( occurrence.Location ) ? occurrence.Location : string.Empty;

                        // Create the list of exceptions.
                        // Exceptions must meet RFC 5545 iCalendar specifications to be correctly processed by third-party calendar applications
                        // such as Microsoft Outlook and Google Calendar. Specifically, an exception must have exactly the same start time
                        // and time zone as the template event, and the time zone must be expressed as an IANA name.
                        // The most recent version of iCal.Net (v2.3.5) that supports .NET framework v4.5.2 has some inconsistencies in the
                        // iCalendar serialization process, so we need to force the Start, End and Exception dates to render in exactly the same format.
                        ievent.Start = new CalDateTime( icalEvent.Start.Value, timeZoneId );
                        ievent.Start.IsUniversalTime = false;

                        ievent.End = new CalDateTime( icalEvent.End.Value, timeZoneId );
                        ievent.End.IsUniversalTime = false;

                        var eventStartTime = new TimeSpan( ievent.DtStart.Hour, ievent.DtStart.Minute, ievent.DtStart.Second );

                        var newExceptionDatesList = new List<Ical.Net.Interfaces.DataTypes.IPeriodList>();

                        foreach ( var exceptionDateList in ievent.ExceptionDates )
                        {
                            var newDateList = new PeriodList() { TzId = timeZoneId };

                            foreach ( var exceptionDate in exceptionDateList )
                            {
                                var newDateTime = exceptionDate.StartTime.HasTime ? exceptionDate.StartTime.Value : exceptionDate.StartTime.Value.Add( eventStartTime );

                                newDateTime = new DateTime( newDateTime.Year, newDateTime.Month, newDateTime.Day, newDateTime.Hour, newDateTime.Minute, newDateTime.Second, newDateTime.Millisecond, DateTimeKind.Local );

                                var newDate = new CalDateTime( newDateTime );
                                newDate.IsUniversalTime = false;

                                newDateList.Add( newDate );
                            }

                            newExceptionDatesList.Add( newDateList );
                        }

                        ievent.ExceptionDates = newExceptionDatesList;

                        // If one-time recurrence dates exist, create a placeholder recurrence rule to ensure that the iCalendar file
                        // can be correctly imported by Outlook.
                        // Fixes Issue #4112. Refer https://github.com/SparkDevNetwork/Rock/issues/4112
                        if ( ievent.RecurrenceRules.Count == 0
                             && ievent.RecurrenceDates.Count > 0 )
                        {
                            ievent.RecurrenceRules.Add( new RecurrencePattern( "FREQ=DAILY;COUNT=1" ) );
                        }

                        // Rock has more descriptions than iCal so lets concatenate them
                        string description = CreateEventDescription( eventItem, occurrence );

                        // Don't set the description prop for outlook to force it to use the X-ALT-DESC property which can have markup.
                        if ( interactionDeviceType != "Outlook" )
                        {
                            ievent.Description = description.ConvertBrToCrLf()
                                                                .Replace( "</P>", "" )
                                                                .Replace( "</p>", "" )
                                                                .Replace( "<P>", Environment.NewLine )
                                                                .Replace( "<p>", Environment.NewLine )
                                                                .Replace( "&nbsp;", " " )
                                                                .SanitizeHtml();
                        }

                        // HTML version of the description for outlook
                        ievent.AddProperty( "X-ALT-DESC;FMTTYPE=text/html", "<html>" + description + "</html>" );

                        // classification: "PUBLIC", "PRIVATE", "CONFIDENTIAL"
                        ievent.Class = "PUBLIC";

                        if ( !string.IsNullOrEmpty( eventItem.DetailsUrl ) )
                        {
                            Uri result;
                            if ( Uri.TryCreate( eventItem.DetailsUrl, UriKind.Absolute, out result ) )
                            {
                                ievent.Url = result;
                            }
                            else if ( Uri.TryCreate( "http://" + eventItem.DetailsUrl, UriKind.Absolute, out result ) )
                            {
                                ievent.Url = result;
                            }
                        }

                        // add contact info if it exists
                        if ( occurrence.ContactPersonAlias != null )
                        {
                            ievent.Organizer = new Organizer( string.Format( "MAILTO:{0}", occurrence.ContactPersonAlias.Person.Email ) );
                            ievent.Organizer.CommonName = occurrence.ContactPersonAlias.Person.FullName;

                            // Outlook doesn't seems to use Contacts or Comments
                            string contactName = !string.IsNullOrEmpty( occurrence.ContactPersonAlias.Person.FullName ) ? "Name: " + occurrence.ContactPersonAlias.Person.FullName : string.Empty;
                            string contactEmail = !string.IsNullOrEmpty( occurrence.ContactEmail ) ? ", Email: " + occurrence.ContactEmail : string.Empty;
                            string contactPhone = !string.IsNullOrEmpty( occurrence.ContactPhone ) ? ", Phone: " + occurrence.ContactPhone : string.Empty;
                            string contactInfo = contactName + contactEmail + contactPhone;

                            ievent.Contacts.Add( contactInfo );
                            ievent.Comments.Add( contactInfo );
                        }

                        // TODO: categories - comma delimited list of whatever, might use audience
                        foreach ( var a in eventItem.EventItemAudiences )
                        {
                            ievent.Categories.Add( a.DefinedValue.Value );
                        }

                        //// No attachments for now.
                        ////if ( eventItem.PhotoId != null )
                        ////{
                        ////    // The DDay Attachment obj doesn't allow you to name the attachment. Nice huh? So just add prop manually...
                        ////    ievent.AddProperty( "ATTACH;VALUE=BINARY;ENCODING=BASE64;X-FILENAME=\"" + eventItem.Photo.FileName + "\"", Convert.ToBase64String( eventItem.Photo.ContentStream.ReadBytesToEnd().ToArray() ) );
                        ////}

                        icalendar.Events.Add( ievent );
                    }
                }
            }

            return icalendar;
        }

        /// <summary>
        /// Creates the event description from the lava template. Default is used if one is not specified in the request.
        /// </summary>
        /// <param name="eventItem">The event item.</param>
        /// <param name="occurrence">The occurrence.</param>
        /// <returns></returns>
        private string CreateEventDescription( EventItem eventItem, EventItemOccurrence occurrence )
        {
            // get the lava template
            int templateDefinedValueId = 0;
            var iCalTemplateDefinedValue = DefinedValueCache.Get( Rock.SystemGuid.DefinedValue.DEFAULT_ICAL_DESCRIPTION );

            if ( request.QueryString["templateid"] != null )
            {
                int.TryParse( request.QueryString["templateid"], out templateDefinedValueId );
                if ( templateDefinedValueId > 0 )
                {
                    iCalTemplateDefinedValue = DefinedValueCache.Get( templateDefinedValueId );
                }
            }

            var mergeFields = Rock.Lava.LavaHelper.GetCommonMergeFields( null );
            mergeFields.Add( "EventItem", eventItem );
            mergeFields.Add( "EventItemOccurrence", occurrence );

            return iCalTemplateDefinedValue.GetAttributeValue( "Template" ).ResolveMergeFields( mergeFields );
        }

        /// <summary>
        /// Uses the filter information in the CalendarProps object to get a list of events
        /// </summary>
        /// <param name="calendarProps">The calendar props.</param>
        /// <returns></returns>
        private List<EventItem> GetEventItems( CalendarProps calendarProps )
        {
            RockContext rockContext = new RockContext();

            EventCalendarItemService eventCalendarItemService = new EventCalendarItemService( rockContext );
            var eventIdsForCalendar = eventCalendarItemService
                .Queryable()
                .Where( i => i.EventCalendarId == calendarProps.CalendarId )
                .Select( i => i.EventItemId )
                .ToList();

            EventItemService eventItemService = new EventItemService( rockContext );
            var eventQueryable = eventItemService
                .Queryable( "EventItemAudiences, EventItemOccurrences.Schedule" )
                .Where( e => eventIdsForCalendar.Contains( e.Id ) )
                .Where( e => e.EventItemOccurrences.Any( o => o.Schedule.EffectiveStartDate <= calendarProps.EndDate && calendarProps.StartDate <= ( o.Schedule.EffectiveEndDate == null ? o.Schedule.EffectiveStartDate : o.Schedule.EffectiveEndDate ) ) )
                .Where( e => e.IsActive == true )
                .Where( e => e.IsApproved );

            // For Campus
            if ( calendarProps.CampusIds.Any() )
            {
                eventQueryable = eventQueryable.Where( e => e.EventItemOccurrences.Any( c => !c.CampusId.HasValue || calendarProps.CampusIds.Contains( c.CampusId.Value ) ) );
            }

            // For Audience
            if ( calendarProps.AudienceIds.Any() )
            {
                eventQueryable = eventQueryable.Where( e => e.EventItemAudiences.Any( c => calendarProps.AudienceIds.Contains( c.DefinedValueId ) ) );
            }

            return eventQueryable.ToList();
        }

        /// <summary>
        /// Sends the not authorized response
        /// </summary>
        /// <param name="httpContext">The HTTP context.</param>
        private void SendNotAuthorized( HttpContext httpContext )
        {
            httpContext.Response.StatusCode = HttpStatusCode.Forbidden.ConvertToInt();
            httpContext.Response.StatusDescription = "Not authorized to view calendar.";
            httpContext.ApplicationInstance.CompleteRequest();
        }

        /// <summary>
        /// Sends the bad request response
        /// </summary>
        /// <param name="httpContext">The HTTP context.</param>
        /// <param name="addlInfo">The addl information.</param>
        private void SendBadRequest( HttpContext httpContext, string addlInfo = "" )
        {
            httpContext.Response.StatusCode = HttpStatusCode.BadRequest.ConvertToInt();
            httpContext.Response.StatusDescription = "Request is invalid or malformed. " + addlInfo;
            httpContext.ApplicationInstance.CompleteRequest();
        }

        /// <summary>
        /// Ensure the current user is authorized to view the calendar. If all are allowed then current user is not evaluated.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        private bool ValidateSecurity( HttpContext context )
        {
            int calendarId;
            if ( request.QueryString["calendarid"] == null || !int.TryParse( request.QueryString["calendarId"], out calendarId ) )
            {
                SendNotAuthorized( context );
                return false;
            }

            RockContext rockContext = new RockContext();
            EventCalendarService eventCalendarService = new EventCalendarService( rockContext );
            EventCalendar eventCalendar = eventCalendarService.Get( calendarId );

            if ( eventCalendar == null )
            {
                SendBadRequest( context );
                return false;
            }


            // Need to replace CurrentUser with the result of a person token, in the meantime this will always create a null person unless directly downloadng the ical when logged into the site
            UserLogin currentUser = new UserLoginService( rockContext ).GetByUserName( UserLogin.GetCurrentUserName() );
            Person currentPerson = currentUser != null ? currentUser.Person : null;
            var isAuthorized = eventCalendar.IsAuthorized( Rock.Security.Authorization.VIEW, currentPerson );
            
            if ( isAuthorized )
            {
                return true;
            }

            SendNotAuthorized( context );
            return false;
        }

        /// <summary>
        /// Validates the request data.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        private CalendarProps ValidateRequestData( HttpContext context )
        {
            CalendarProps calendarProps = new CalendarProps();

            // Security check made sure the calendar ID is good so no need to check it again.
            calendarProps.CalendarId = int.Parse( request.QueryString["calendarid"] );

            string campusIdQueryString = request.QueryString["campusids"] != null ? request.QueryString["campusids"] : string.Empty;
            calendarProps.CampusIds = ParseIds( campusIdQueryString );

            string audienceIdQueryString = request.QueryString["audienceids"] != null ? request.QueryString["audienceids"] : string.Empty;
            calendarProps.AudienceIds = ParseIds( audienceIdQueryString );

            string startDate = request.QueryString["startdate"];
            if ( !string.IsNullOrWhiteSpace( startDate ) )
            {
                calendarProps.StartDate = DateTime.ParseExact( startDate, "yyyyMMdd", CultureInfo.InvariantCulture );
            }

            string endDate = request.QueryString["enddate"];
            if ( !string.IsNullOrWhiteSpace( endDate ) )
            {
                calendarProps.EndDate = DateTime.ParseExact( endDate, "yyyyMMdd", CultureInfo.InvariantCulture );
            }

            return calendarProps;
        }

        /// <summary>
        /// Parses a query string for a list of Ids
        /// </summary>
        /// <returns></returns>
        private List<int> ParseIds( string queryParamemter )
        {
            List<string> stringIdList = new List<string>();
            List<int> intIdList = new List<int>();

            if ( queryParamemter.IsNotNullOrWhiteSpace() )
            {
                stringIdList = queryParamemter.Split( ',' ).ToList();

                foreach ( string stringId in stringIdList )
                {
                    int intId;
                    if ( int.TryParse( stringId, out intId ) )
                    {
                        intIdList.Add( intId );
                    }
                }
            }

            return intIdList;
        }

        /// <summary>
        /// CalendarId is required. CampusIds, AudienceIds, Startdate, and Enddate are optional.
        /// StartDate defaults to the current date, EndDate defaults to the currentDate + 2 months.
        /// </summary>
        private class CalendarProps
        {
            private DateTime? _startDate;
            private DateTime? _endDate;

            /// <summary>
            /// Gets or sets the calendar id.
            /// </summary>
            /// <value>
            /// The calendar identifier.
            /// </value>
            public int CalendarId { get; set; }

            /// <summary>
            /// Gets or sets the campus ids. Leave empty to return all campuses
            /// </summary>
            /// <value>
            /// The campus ids.
            /// </value>
            public List<int> CampusIds { get; set; }

            /// <summary>
            /// Gets or sets the audience ids list. leave empty to return all audiences
            /// </summary>
            /// <value>
            /// The audience ids.
            /// </value>
            public List<int> AudienceIds { get; set; }

            /// <summary>
            /// Gets or sets the start date. if not explicitly set returns current date
            /// </summary>
            /// <value>
            /// The start date.
            /// </value>
            public DateTime StartDate
            {
                get
                {
                    return _startDate ?? RockDateTime.Now.AddMonths( -3 ).Date;
                }

                set
                {
                    _startDate = value;
                }
            }

            /// <summary>
            /// Gets or sets the end date. If not explicitly set returns two months from current date.
            /// </summary>
            /// <value>
            /// The end date.
            /// </value>
            public DateTime EndDate
            {
                get
                {
                    return _endDate ?? RockDateTime.Now.AddMonths( 12 ).Date;
                }

                set
                {
                    _endDate = value;
                }
            }
        }
    }
}