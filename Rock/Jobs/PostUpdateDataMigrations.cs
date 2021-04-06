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
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Quartz;

using Rock.Attribute;
using Rock.Data;
using Rock.Model;

namespace Rock.Jobs
{
    /// <summary>
    /// </summary>
    /// <seealso cref="Quartz.IJob" />
    [DisallowConcurrentExecution]
    [DisplayName( "Rock Update Helper" )]
    [Description( "This job will run any post data migrations that haven't completed yet" )]

    [IntegerField(
        "Command Timeout",
        AttributeKey.CommandTimeout,
        Description = "Maximum amount of time (in seconds) to wait for each SQL command to complete.",
        IsRequired = false,
        DefaultIntegerValue = 60 * 60 )]

    public class PostUpdateDataMigrations : IJob
    {
        private static class AttributeKey
        {
            public const string CommandTimeout = "CommandTimeout";
        }

        /// <summary>
        /// Executes the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        public void Execute( IJobExecutionContext context )
        {
            JobDataMap dataMap = context.JobDetail.JobDataMap;

            var jobId = context.GetJobId();
            var rockContext = new RockContext();
            var postUpdateDataMigrationsJob = new ServiceJobService( rockContext ).Get( jobId );

            // get the configured timeout, or default to 60 minutes if it is blank
            var commandTimeoutSeconds = dataMap.GetString( AttributeKey.CommandTimeout ).AsIntegerOrNull() ?? 3600;

            List<string> updatesRemaining = new List<string>();

            var postUpdateTypes = Reflection.FindTypes( typeof( RockUpdate.PostUpdateMigration ) ).Values.OrderBy( a => a.Name ).ToList();
            foreach ( var postUpdateType in postUpdateTypes )
            {
                var postUpdate = Activator.CreateInstance( postUpdateType ) as RockUpdate.PostUpdateMigration;
                if ( !postUpdate.IsComplete() )
                {
                    var postUpdateName = Reflection.GetDescription( postUpdateType ) ?? postUpdateType.Name.SplitCase();
                    context.UpdateLastStatusMessage( $"Running { postUpdateName }" );
                    postUpdate.Update( commandTimeoutSeconds );
                    if ( postUpdate.IsComplete() )
                    {
                        context.UpdateLastStatusMessage( $"Completed { postUpdateName }" );
                    }
                    else
                    {
                        updatesRemaining.Add( postUpdateName );
                    }
                }
            }

            if ( !updatesRemaining.Any() )
            {
                context.UpdateLastStatusMessage( "All Updates completed" );
            }
            else
            {
                context.UpdateLastStatusMessage( $"{updatesRemaining.Count()} Updates remaining" );
            }
        }

    }
}
