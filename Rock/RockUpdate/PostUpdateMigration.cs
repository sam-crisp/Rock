using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rock.RockUpdate
{
    // TODO Move this when we do Rock.Update.dll
    public abstract class PostUpdateMigration
    {
        public abstract bool IsComplete();

        public abstract bool MustCompleteBeforeUpdating( FileVersionInfo currentRockVersion, FileVersionInfo nextRockVersion );

        public abstract void Update( int? commandTimeoutSeconds );
    }
}
