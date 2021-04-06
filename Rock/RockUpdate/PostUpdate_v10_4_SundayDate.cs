using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rock.RockUpdate
{
    public class PostUpdate_v10_4_SundayDate : PostUpdateMigration
    {
        public override bool IsComplete()
        {
            var postUpdateKey
            bool isComplete = Rock.Web.SystemSettings.GetValue( PostUpdateMigrationKey.V10_4_SundayDate_Completed ).AsBoolean();
            return isComplete;
        }

        /// <summary>
        /// Returns true if this update needs to complete prior to updating to the next update of Rock.
        /// </summary>
        /// <param name="currentRockVersion">The current rock version.</param>
        /// <param name="nextRockVersion">The next rock version.</param>
        /// <returns></returns>
        public override bool MustCompleteBeforeUpdating( FileVersionInfo currentRockVersion, FileVersionInfo nextRockVersion )
        {
            return true;

            /*
            if ( nextRockVersion.FileMajorPart > 1 )
            {
                // must complete if next Rock version is 2.0.0.0 or newer
                return true;
            }

            if ( nextRockVersion.FileMinorPart > 10 )
            {
                // must complete if next Rock version is 1.11.0.0 or newer
                return true;
            }

            if ( nextRockVersion.FileMinorPart == 10 && nextRockVersion.FileBuildPart > 4 )
            {
                // must complete if next Rock version is 1.10.5.0 or newer
                return true;
            }

            return false;
            */
        }

        public override void Update( int? commandTimeoutSeconds )
        {
            var dateKeyCompleted = Rock.Web.SystemSettings.GetValue( PostUpdateMigrationKey.V10_4_DateKey_Completed ).AsBoolean();
            if ( !dateKeyCompleted)
            {
                // if the V10_4_DateKey_Completed hasn't completed yet, don't run this
                return;
            }

            using ( var rockContext = new Rock.Data.RockContext() )
            {
                rockContext.Database.CommandTimeout = commandTimeoutSeconds ?? 3600;
                rockContext.Database.ExecuteSqlCommand( @"
UPDATE FinancialTransaction
SET SundayDate = dbo.ufnUtility_GetSundayDate(TransactionDateTime)
WHERE SundayDate IS NULL
	OR SundayDate != dbo.ufnUtility_GetSundayDate(TransactionDateTime)
" );

                rockContext.Database.ExecuteSqlCommand( @"
UPDATE AttendanceOccurrence
SET SundayDate = dbo.ufnUtility_GetSundayDate(OccurrenceDate)
WHERE SundayDate IS NULL
	OR SundayDate != dbo.ufnUtility_GetSundayDate(OccurrenceDate)
" );
            }

            Rock.Web.SystemSettings.SetValue( PostUpdateMigrationKey.V10_4_SundayDate_Completed, true.ToTrueFalse() );
        }
    }
}
