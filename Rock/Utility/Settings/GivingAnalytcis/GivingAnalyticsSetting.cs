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
using System.Collections.Generic;

namespace Rock.Utility.Settings.GivingAnalytics
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GivingAnalyticsSetting"/> class.
    /// </summary>
    public class GivingAnalyticsSetting
    {
        /*
         * 2021-06-30 BJW
         * There used to be a constructor here that would set GivingAnalytics and Alerting to new instances so
         * that null values wouldn't be an issue. This was causing problems with the JSON serialization and
         * deserialization. Particularly, lists of values were appended rather than being replaced when
         * deserializing the value from the attribute value. So it was possible to get a list of days of the
         * week that contained 10+ values for instance. Same issue with Giver Bins list.
         */

        /// <summary>
        /// Gets or sets the giving analytics.
        /// </summary>
        /// <value>
        /// The giving analytics.
        /// </value>
        public GivingAnalytics GivingAnalytics { get; set; }

        /// <summary>
        /// Gets or sets the giving journey settings.
        /// </summary>
        /// <value>The giving journey settings.</value>
        public GivingJourneySettings GivingJourneySettings { get; set; }

        /// <summary>
        /// Gets or sets the alerting.
        /// </summary>
        /// <value>
        /// The alerting.
        /// </value>
        public Alerting Alerting { get; set; }

        /// <summary>
        /// Gets or sets the transaction type guids (defined value guids) that will be included in
        /// classification and alerting.
        /// </summary>
        /// <value>
        /// The transaction type guids.
        /// </value>
        public List<Guid> TransactionTypeGuids { get; set; }

        /// <summary>
        /// Gets or sets the financial account guids. If empty, then we assume all tax deductible accounts are used.
        /// </summary>
        /// <value>
        /// The financial account guids.
        /// </value>
        public List<Guid> FinancialAccountGuids { get; set; }

        /// <summary>
        /// Gets or sets the are child accounts included. This defaults to false.
        /// </summary>
        /// <value>
        /// The are child accounts included.
        /// </value>
        public bool? AreChildAccountsIncluded { get; set; }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GivingAnalyticsSetting"/> class.
    /// </summary>
    public class GivingAnalytics
    {
        /// <summary>
        /// Gets or sets a value indicating whether this instance is enabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is enabled; otherwise, <c>false</c>.
        /// </value>
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// Gets or sets the days of the week to run the classifications for each giver.
        /// </summary>
        /// <value>
        /// The days of the week to run the analytics for each giver.
        /// </value>
        public List<DayOfWeek> GiverAnalyticsRunDays { get; set; }

        /// <summary>
        /// Gets or sets the date and time that the giving analytics Job last completed successfully.
        /// </summary>
        /// <value>
        /// A <see cref="System.DateTime"/> representing the date and time of the last time that the giving analytics Job completed successfully
        /// </value>
        public DateTime? GivingAnalyticsLastRunDateTime { get; set; }

        /// <summary>
        /// Gets or sets the giver bins.
        /// </summary>
        /// <value>
        /// The giver bins.
        /// </value>
        public List<GiverBin> GiverBins { get; set; }
    }

    /// <summary>
    /// Giving Journey Settings
    /// </summary>
    public class GivingJourneySettings
    {
        /// <summary>
        /// Gets or sets the days to update giving journeys.
        /// </summary>
        /// <value>The days to update giving journeys.</value>
        public List<DayOfWeek> DaysToUpdateGivingJourneys { get; set; } = new List<DayOfWeek> { DayOfWeek.Tuesday };

        /// <summary>
        /// Gets or sets the former giver no contribution in the last days.
        /// </summary>
        /// <value>The former giver no contribution in the last days.</value>
        public int? FormerGiverNoContributionInTheLastDays { get; set; } = 375;

        /// <summary>
        /// Gets or sets the former giver median frequency less than days.
        /// </summary>
        /// <value>The former giver median frequency less than days.</value>
        public int? FormerGiverMedianFrequencyLessThanDays { get; set; } = 320;

        /// <summary>
        /// Gets or sets the lapsed giver no contribution in the last days.
        /// </summary>
        /// <value>The lapsed giver no contribution in the last days.</value>
        public int? LapsedGiverNoContributionInTheLastDays { get; set; } = 150;

        /// <summary>
        /// Gets or sets the lapsed giver median frequency less than days.
        /// </summary>
        /// <value>The lapsed giver median frequency less than days.</value>
        public int? LapsedGiverMedianFrequencyLessThanDays { get; set; } = 100;

        /// <summary>
        /// Gets or sets the first time giver contribution count between minimum.
        /// </summary>
        /// <value>The first time giver contribution count between minimum.</value>
        public int? NewGiverContributionCountBetweenMinimum { get; set; } = 1;

        /// <summary>
        /// Gets or sets the first time giver contribution count between maximum.
        /// </summary>
        /// <value>The first time giver contribution count between maximum.</value>
        public int? NewGiverContributionCountBetweenMaximum { get; set; } = 5;

        /// <summary>
        /// Gets or sets the first time giver first gift in the last days.
        /// </summary>
        /// <value>The first time giver first gift in the last days.</value>
        public int? NewGiverFirstGiftInTheLastDays { get; set; } = 150;

        /// <summary>
        /// Gets or sets the occasional giver median frequency days minimum.
        /// </summary>
        /// <value>The occasional giver median frequency days minimum.</value>
        public int? OccasionalGiverMedianFrequencyDaysMinimum { get; set; } = 33;

        /// <summary>
        /// Gets or sets the occasional giver median frequency days maximum.
        /// </summary>
        /// <value>The occasional giver median frequency days maximum.</value>
        public int? OccasionalGiverMedianFrequencyDaysMaximum { get; set; } = 94;

        /// <summary>
        /// Gets or sets the consistent giver median less than days.
        /// </summary>
        /// <value>The consistent giver median less than days.</value>
        public int? ConsistentGiverMedianLessThanDays { get; set; } = 32;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Alerting"/> class.
    /// </summary>
    public class Alerting
    {
        /// <summary>
        /// Gets or sets the global repeat prevention duration in days.
        /// </summary>
        /// <value>
        /// The global repeat prevention duration.
        /// </value>
        public int? GlobalRepeatPreventionDurationDays { get; set; }

        /// <summary>
        /// Gets or sets the gratitude repeat prevention duration in days.
        /// </summary>
        /// <value>
        /// The gratitude repeat prevention duration.
        /// </value>
        public int? GratitudeRepeatPreventionDurationDays { get; set; }

        /// <summary>
        /// Gets or sets the follow-up repeat prevention duration in days.
        /// </summary>
        /// <value>
        /// The follow-up repeat prevention duration.
        /// </value>
        public int? FollowupRepeatPreventionDurationDays { get; set; }
    }

    /// <summary>
    /// Information About the Giver Bin
    /// </summary>
    public class GiverBin
    {
        /// <summary>
        /// Gets or sets the lower range.
        /// </summary>
        /// <value>
        /// The lower range.
        /// </value>
        public decimal? LowerLimit { get; set; }
    }

    /// <summary>
    /// Enum GivingJourneyStage
    /// </summary>
    public enum GivingJourneyStage
    {
        /// <summary>
        /// Non-Giver
        /// </summary>
        NonGiver = 0,

        /// <summary>
        /// New giver.
        /// </summary>
        NewGiver = 1,

        /// <summary>
        /// Occasional giver
        /// </summary>
        OccasionalGiver = 2,

        /// <summary>
        /// Consistent giver
        /// </summary>
        ConsistentGiver = 3,

        /// <summary>
        /// Lapsed giver
        /// </summary>
        LapsedGiver = 4,

        /// <summary>
        /// Former giver
        /// </summary>
        FormerGiver = 5
    }
}
