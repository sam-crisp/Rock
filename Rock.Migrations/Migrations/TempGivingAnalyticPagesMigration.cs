using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Rock.Model;

namespace Rock.Migrations.Migrations
{
    class TempGivingAnalyticPagesMigration //: RockMigration
    {

        public void Up()
        {

            // Sql( $"UPDATE [Page] SET [DisplayInNavWhen] = {( int ) DisplayInNavWhen.WhenAllowed} WHERE [Guid] = '{SystemGuid.Page.GIVING_ALERTS}';" );

            
        }
    }
}
