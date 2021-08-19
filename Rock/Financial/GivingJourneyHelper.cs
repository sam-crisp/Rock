using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Rock.Data;
using Rock.Model;
using Rock.SystemKey;
using Rock.Utility.Settings.GivingAnalytics;
using Rock.Web.Cache;

namespace Rock.Financial
{
    /// <summary>
    /// Class GivingJourneyHelper.
    /// </summary>
    internal static class GivingJourneyHelper
    {
        /// <summary>
        /// Processes the giving journeys.
        /// </summary>
        internal static void ProcessGivingJourneys()
        {
            var givingAnalyticsSetting = GetGivingAnalyticsSettings();

            var rockContext = new RockContext();
            var personService = new PersonService( rockContext );
            var personQuery = personService.Queryable();

            var personAliasService = new PersonAliasService( rockContext );
            var personAliasQuery = personAliasService.Queryable();
            var financialTransactionService = new FinancialTransactionService( rockContext );
            var financialTransactionGivingAnalyticsQuery = financialTransactionService.GetGivingAnalyticsSourceTransactionQuery();

            rockContext.SqlLogging( true );

            /* Get Non-Giver GivingIds */
            var nonGiverGivingIdsQuery = personQuery.Where( p =>
                    !financialTransactionGivingAnalyticsQuery.Any( ft => personAliasQuery.Any( pa => pa.Id == ft.AuthorizedPersonAliasId && pa.Person.GivingId == p.GivingId ) ) );

            var nonGiverGivingIdsList = nonGiverGivingIdsQuery.Select( a => a.GivingId ).Distinct().ToList();

            /* Get TransactionDateList for each GivingId in the system */
            var transactionDateTimes = financialTransactionGivingAnalyticsQuery.Select( a => new
            {
                GivingId = personAliasQuery.Where( pa => pa.Id == a.AuthorizedPersonAliasId ).Select( pa => pa.Person.GivingId ).FirstOrDefault(),
                a.TransactionDateTime
            } ).Where( a => a.GivingId != null ).ToList();

            var transactionDateTimesByGivingId = transactionDateTimes
                    .GroupBy( g => g.GivingId )
                    .Select( s => new
                    {
                        GivingId = s.Key,
                        TransactionDateTimeList = s.Select( x => x.TransactionDateTime ).ToList()
                    } ).ToDictionary( k => k.GivingId, v => v.TransactionDateTimeList );

            /* Get the current Journey-related attribute values for everybody in the system */
            var previousJourneyStageAttributeId = AttributeCache.GetId( Rock.SystemGuid.Attribute.PERSON_GIVING_PREVIOUS_GIVING_JOURNEY_STAGE.AsGuid() );
            var currentJourneyStageAttributeId = AttributeCache.GetId( Rock.SystemGuid.Attribute.PERSON_GIVING_CURRENT_GIVING_JOURNEY_STAGE.AsGuid() );
            var journeyStageChangeDateAttributeId = AttributeCache.GetId( Rock.SystemGuid.Attribute.PERSON_GIVING_GIVING_JOURNEY_STAGE_CHANGE_DATE.AsGuid() );

            List<int> journeyStageAttributeIds = new List<int>();
            if ( previousJourneyStageAttributeId.HasValue )
            {
                journeyStageAttributeIds.Add( previousJourneyStageAttributeId.Value );
            }

            if ( currentJourneyStageAttributeId.HasValue )
            {
                journeyStageAttributeIds.Add( currentJourneyStageAttributeId.Value );
            }

            if ( journeyStageChangeDateAttributeId.HasValue )
            {
                journeyStageAttributeIds.Add( journeyStageChangeDateAttributeId.Value );
            }

            var personCurrentJourneyAttributeValues = new AttributeValueService( rockContext ).Queryable()
                .Where( av => journeyStageAttributeIds.Contains( av.AttributeId ) && av.EntityId.HasValue )
                .Join(
                    personQuery,
                    av => av.EntityId.Value,
                    p => p.Id,
                    ( av, p ) => new
                    {
                        AttributeId = av.AttributeId,
                        AttributeValue = av.Value,
                        PersonGivingId = p.GivingId,
                        PersonId = p.Id
                    } )
                .GroupBy( a => a.PersonGivingId )
                .Select( a => new
                {
                    GivingId = a.Key,
                    AttributeValues = a.ToList()
                } ).ToDictionary( k => k.GivingId, v => v.AttributeValues );

            var givingJourneySettings = givingAnalyticsSetting.GivingJourneySettings;
            var currentDate = RockDateTime.Today;

            var formerGiverGivingIds = new List<string>();
            var lapsedGiverGivingIds = new List<string>();
            var newGiverGivingIds = new List<string>();
            var occasionalGiverGivingIds = new List<string>();
            var consistentGiverGivingIds = new List<string>();

            var noneOfTheAboveGiverGivingIds = new List<string>();

            foreach ( var givingIdTransactions in transactionDateTimesByGivingId )
            {
                var givingId = givingIdTransactions.Key;
                var transactionDateList = givingIdTransactions.Value.Where( a => a.HasValue ).Select( a => a.Value ).ToList();
                var mostRecentTransactionDateTime = transactionDateList.Max();
                var firstTransactionDateTime = transactionDateList.Min();
                var daysBetweenList = GetBetweenDatesDays( transactionDateList );
                var medianDaysBetween = GetMedian( daysBetweenList );
                var daysSinceMostRecentTransaction = ( currentDate - mostRecentTransactionDateTime ).TotalDays;
                var daysSinceFirstTransaction = ( currentDate - firstTransactionDateTime ).TotalDays;

                if ( IsFormerGiver( givingJourneySettings, medianDaysBetween, daysSinceMostRecentTransaction ) )
                {
                    formerGiverGivingIds.Add( givingId );
                    continue;
                }

                if ( IsLapsedGiver( givingJourneySettings, medianDaysBetween, daysSinceMostRecentTransaction ) )
                {
                    lapsedGiverGivingIds.Add( givingId );
                    continue;
                }

                if ( IsNewGiver( givingJourneySettings, transactionDateList.Count, daysSinceFirstTransaction ) )
                {
                    newGiverGivingIds.Add( givingId );
                    continue;
                }

                if ( IsOccasionalGiver( givingJourneySettings, medianDaysBetween ) )
                {
                    occasionalGiverGivingIds.Add( givingId );
                    continue;
                }

                if ( IsConsistentGiver( givingJourneySettings, medianDaysBetween ) )
                {
                    consistentGiverGivingIds.Add( givingId );
                    continue;
                }

                // if they are non of the above, then add them to the non of the list
                noneOfTheAboveGiverGivingIds.Add( givingId );
            }

            Debug.WriteLine( $@"
FormerGiverCount: {formerGiverGivingIds.Count}
LapsedGiverCount: {lapsedGiverGivingIds.Count}
NewGiverCount: {newGiverGivingIds.Count}
OccasionalGiverCount: {occasionalGiverGivingIds.Count}
ConsistentGiverCount: {consistentGiverGivingIds.Count}
NonGiverCount: {nonGiverGivingIdsList.Count}
NoneOfTheAboveCount: {noneOfTheAboveGiverGivingIds.Count}
" );

            // TODO Update Attribute Values...

        }

        /// <summary>
        /// Determines whether [is consistent giver] [the specified giving journey settings].
        /// </summary>
        /// <param name="givingJourneySettings">The giving journey settings.</param>
        /// <param name="medianDaysBetween">The median days between.</param>
        /// <returns><c>true</c> if [is consistent giver] [the specified giving journey settings]; otherwise, <c>false</c>.</returns>
        private static bool IsConsistentGiver( GivingJourneySettings givingJourneySettings, int? medianDaysBetween )
        {
            if ( !medianDaysBetween.HasValue )
            {
                return false;
            }

            if ( !givingJourneySettings.ConsistentGiverMedianLessThanDays.HasValue )
            {
                // not configured
                return false;
            }

            if ( medianDaysBetween < givingJourneySettings.ConsistentGiverMedianLessThanDays )
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Determines whether [is occasional giver] [the specified giving journey settings].
        /// </summary>
        /// <param name="givingJourneySettings">The giving journey settings.</param>
        /// <param name="medianDaysBetween">The median days between.</param>
        /// <returns><c>true</c> if [is occasional giver] [the specified giving journey settings]; otherwise, <c>false</c>.</returns>
        private static bool IsOccasionalGiver( GivingJourneySettings givingJourneySettings, int? medianDaysBetween )
        {
            if ( !medianDaysBetween.HasValue )
            {
                return false;
            }

            if ( !givingJourneySettings.OccasionalGiverMedianFrequencyDaysMinimum.HasValue || !givingJourneySettings.OccasionalGiverMedianFrequencyDaysMaximum.HasValue )
            {
                // not configured
                return false;
            }

            var medianDaysMin = givingJourneySettings.OccasionalGiverMedianFrequencyDaysMinimum.Value;
            var medianDaysMax = givingJourneySettings.OccasionalGiverMedianFrequencyDaysMaximum.Value;

            if ( medianDaysBetween.Value >= medianDaysMin && medianDaysBetween.Value <= medianDaysMax )
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Determines whether [is former giver] [the specified giving journey settings].
        /// </summary>
        /// <param name="givingJourneySettings">The giving journey settings.</param>
        /// <param name="medianDaysBetween">The median days between.</param>
        /// <param name="daysSinceMostRecentTransaction">The days since most recent transaction.</param>
        /// <returns><c>true</c> if [is former giver] [the specified giving journey settings]; otherwise, <c>false</c>.</returns>
        private static bool IsFormerGiver( GivingJourneySettings givingJourneySettings, int? medianDaysBetween, double daysSinceMostRecentTransaction )
        {
            bool isFormerGiver = false;

            if ( givingJourneySettings.FormerGiverNoContributionInTheLastDays.HasValue )
            {
                isFormerGiver = daysSinceMostRecentTransaction >= givingJourneySettings.FormerGiverNoContributionInTheLastDays.Value;

                if ( givingJourneySettings.FormerGiverMedianFrequencyLessThanDays.HasValue && medianDaysBetween.HasValue )
                {
                    isFormerGiver = isFormerGiver && ( medianDaysBetween < givingJourneySettings.FormerGiverMedianFrequencyLessThanDays.Value );
                }
            }

            return isFormerGiver;
        }

        /// <summary>
        /// Determines whether [is lapsed giver] [the specified giving journey settings].
        /// </summary>
        /// <param name="givingJourneySettings">The giving journey settings.</param>
        /// <param name="medianDaysBetween">The median days between.</param>
        /// <param name="daysSinceMostRecentTransaction">The days since most recent transaction.</param>
        /// <returns><c>true</c> if [is lapsed giver] [the specified giving journey settings]; otherwise, <c>false</c>.</returns>
        private static bool IsLapsedGiver( GivingJourneySettings givingJourneySettings, int? medianDaysBetween, double daysSinceMostRecentTransaction )
        {
            bool isLapsedGiver = false;

            if ( givingJourneySettings.LapsedGiverNoContributionInTheLastDays.HasValue )
            {
                isLapsedGiver = daysSinceMostRecentTransaction > givingJourneySettings.LapsedGiverNoContributionInTheLastDays.Value;

                if ( givingJourneySettings.LapsedGiverMedianFrequencyLessThanDays.HasValue && medianDaysBetween.HasValue )
                {
                    isLapsedGiver = isLapsedGiver && ( medianDaysBetween < givingJourneySettings.LapsedGiverMedianFrequencyLessThanDays.Value );
                }
            }

            return isLapsedGiver;
        }

        /// <summary>
        /// Determines whether [is new giver] [the specified giving journey settings].
        /// </summary>
        /// <param name="givingJourneySettings">The giving journey settings.</param>
        /// <param name="transactionCount">The transaction count.</param>
        /// <param name="daysSinceFirstTransaction">The days since first transaction.</param>
        /// <returns><c>true</c> if [is new giver] [the specified giving journey settings]; otherwise, <c>false</c>.</returns>
        private static bool IsNewGiver( GivingJourneySettings givingJourneySettings, int transactionCount, double daysSinceFirstTransaction )
        {
            if ( !givingJourneySettings.NewGiverContributionCountBetweenMinimum.HasValue || !givingJourneySettings.NewGiverContributionCountBetweenMaximum.HasValue || !givingJourneySettings.NewGiverFirstGiftInTheLastDays.HasValue )
            {
                // not configured
                return false;
            }

            if ( daysSinceFirstTransaction > givingJourneySettings.NewGiverFirstGiftInTheLastDays.Value )
            {
                // gave more than NewGiverFirstGiftInTheLastDays ago
                return false;
            }

            var minCount = givingJourneySettings.NewGiverContributionCountBetweenMinimum.Value;
            var maxCount = givingJourneySettings.NewGiverContributionCountBetweenMaximum.Value;

            if ( transactionCount >= minCount && transactionCount <= maxCount )
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Gets the median.
        /// From https://stackoverflow.com/a/5275324/1755417
        /// </summary>
        /// <param name="valueList">The value list.</param>
        /// <returns>System.Nullable&lt;System.Int32&gt;.</returns>
        public static int? GetMedian( IEnumerable<int> valueList )
        {
            var sortedValuesArray = valueList.ToArray();
            Array.Sort( sortedValuesArray );

            int count = sortedValuesArray.Length;
            if ( count == 0 )
            {
                return null;
            }
            else if ( count % 2 == 0 )
            {
                // count is even, average two middle elements
                int medianValue1 = sortedValuesArray[( count / 2 ) - 1];
                int medianValue2 = sortedValuesArray[count / 2];
                var averageOfMiddleElements = ( medianValue1 + medianValue2 ) / 2m;
                return ( int ) averageOfMiddleElements;
            }
            else
            {
                // count is odd, return the middle element
                return sortedValuesArray[count / 2];
            }
        }

        /// <summary>
        /// Gets the between dates days.
        /// </summary>
        /// <param name="transactionDateList">The transaction date list.</param>
        /// <returns>List&lt;System.Int32&gt;.</returns>
        private static List<int> GetBetweenDatesDays( List<DateTime> transactionDateList )
        {
            var daysSinceLastTransaction = new List<int>();

            if ( !transactionDateList.Any() )
            {
                return daysSinceLastTransaction;
            }

            var transactionDateListOrderByDate = transactionDateList.OrderBy( a => a ).ToArray();

            var previousTransactionDate = transactionDateListOrderByDate[0];

            foreach ( var transactionDate in transactionDateListOrderByDate )
            {
                var totalDaysDiff = ( transactionDate - previousTransactionDate ).TotalDays;
                var daysSince = ( int ) Math.Round( totalDaysDiff, 0 );

                previousTransactionDate = transactionDate;

                if ( daysSince == 0 )
                {
                    // if they gave more than one time in a day, only count the daysSince for one of them
                    continue;
                }

                daysSinceLastTransaction.Add( daysSince );
            }

            return daysSinceLastTransaction;
        }

        /// <summary>
        /// Gets the giving analytics settings.
        /// </summary>
        /// <returns></returns>
        private static GivingAnalyticsSetting GetGivingAnalyticsSettings()
        {
            var settings = Rock.Web.SystemSettings
                .GetValue( SystemSetting.GIVING_ANALYTICS_CONFIGURATION )
                .FromJsonOrNull<GivingAnalyticsSetting>() ?? new GivingAnalyticsSetting();

            settings.TransactionTypeGuids = settings.TransactionTypeGuids ?? new List<Guid>();
            settings.GivingAnalytics = settings.GivingAnalytics ?? new Rock.Utility.Settings.GivingAnalytics.GivingAnalytics();
            settings.GivingJourneySettings = settings.GivingJourneySettings ?? new GivingJourneySettings();

            return settings;
        }
    }
}
