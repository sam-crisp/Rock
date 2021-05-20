using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quartz;
using Rock.Attribute;
using Rock.Data;
using Rock.Model;
using Rock.Web.Cache;

namespace Rock.Jobs
{
    /// <summary>
    /// This job updates the existing step to map with step program completion if valid.
    /// </summary>
    /// <seealso cref="Quartz.IJob" />
    [DisplayName( "Update Step Program Completion" )]
    [Description( "This job updates the existing step to map with step program completion if valid." )]

    [DisallowConcurrentExecution]
    [IntegerField(
        "Max Records Per Run",
        Description = "The maximum number of records to run per run.",
        IsRequired = false,
        DefaultIntegerValue = AttributeDefaults.MaxRecordsPerRun,
        Order = 0,
        Key = AttributeKey.MaxRecordsPerRun )]
    [IntegerField(
        "Command Timeout",
        Description = "Maximum amount of time (in seconds) to wait for the SQL Query to complete. Leave blank to use the default for this job (3600). Note, it could take several minutes, so you might want to set it at 3600 (60 minutes) or higher",
        IsRequired = false,
        DefaultIntegerValue = AttributeDefaults.CommandTimeout,
        Category = "General",
        Order = 1,
        Key = AttributeKey.CommandTimeout )]
    public class UpdateStepProgramCompletion : IJob
    {
        #region Keys

        /// <summary>
        /// Attribute Keys
        /// </summary>
        private static class AttributeKey
        {
            /// <summary>
            /// The max records per run
            /// </summary>
            public const string MaxRecordsPerRun = "MaxRecordsPerRun";

            /// <summary>
            /// The command timeout
            /// </summary>
            public const string CommandTimeout = "CommandTimeout";
        }

        /// <summary>
        /// Attribute value defaults
        /// </summary>
        private static class AttributeDefaults
        {
            /// <summary>
            /// The command timeout
            /// </summary>
            public const int CommandTimeout = 60 * 60;

            /// <summary>
            /// The maximum records per run
            /// </summary>
            public const int MaxRecordsPerRun = 100;
        }

        #endregion Keys

        private const string _entitySetGuid = "495BF2AF-931B-495E-B88B-AE1C5E451C32";

        /// <summary>
        /// Executes the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <exception cref="System.NotImplementedException"></exception>

        public void Execute( IJobExecutionContext context )
        {
            JobDataMap dataMap = context.JobDetail.JobDataMap;

            int maxRecordsPerRun = dataMap.GetString( AttributeKey.MaxRecordsPerRun ).AsIntegerOrNull() ?? AttributeDefaults.MaxRecordsPerRun;
            var commandTimeout = dataMap.GetString( AttributeKey.CommandTimeout ).AsIntegerOrNull() ?? AttributeDefaults.CommandTimeout;

            var rockContext = new RockContext();
            var stepService = new StepService( rockContext );
            var entitySetQry = GetEntitySetIdQuery( rockContext );

            var personIdQry = stepService
                .Queryable()
                .Where( a => a.CompletedDateTime.HasValue && !a.StepProgramCompletionId.HasValue && !entitySetQry.Contains( a.PersonAlias.PersonId ) )
                .Select( a => a.PersonAlias.PersonId )
                .Distinct()
                .Take( maxRecordsPerRun + 1 );

            bool anyRemaining = personIdQry.Count() > maxRecordsPerRun;
            var personIds = personIdQry.Take( maxRecordsPerRun ).ToList();
            var stepTypeService = new StepTypeService( rockContext );

            var stepProgramStepTypeMappings = stepTypeService
                .Queryable()
                .Where( a => a.IsActive )
                .GroupBy( a => a.StepProgramId )
                .ToDictionary( a => a.Key, b => b.Select( c => c.Id ).ToList() );

            var steps = new StepService( rockContext )
                    .Queryable( "PersonAlias" )
                    .AsNoTracking()
                    .Where( a => personIdQry.Contains( a.PersonAlias.PersonId ) && !a.StepProgramCompletionId.HasValue && a.CompletedDateTime.HasValue )
                    .ToList();

            foreach ( var personId in personIds.Take( maxRecordsPerRun ) )
            {
                var personSteps = steps.Where( a => a.PersonAlias.PersonId == personId );
                if ( !personSteps.Any() )
                {
                    continue;
                }

                foreach ( var stepProgramId in stepProgramStepTypeMappings.Keys )
                {
                    var stepTypeIds = stepProgramStepTypeMappings[stepProgramId];
                    var stepsByProgram = personSteps.Where( a => stepTypeIds.Contains( a.StepTypeId ) ).OrderBy( a => a.CompletedDateTime ).ToList();

                    if ( !stepsByProgram.Any() )
                    {
                        continue;
                    }

                    while ( stepsByProgram.Any() && stepTypeIds.All( a => stepsByProgram.Any( b => b.StepTypeId == a ) ) )
                    {
                        var stepSet = new List<Step>();
                        foreach ( var stepTypeId in stepTypeIds )
                        {
                            var step = stepsByProgram.Where( a => a.StepTypeId == stepTypeId ).FirstOrDefault();
                            if ( step == null )
                            {
                                continue;
                            }

                            stepSet.Add( step );
                            stepsByProgram.RemoveAll( a => a.Id == step.Id );
                        }

                        var personAliasId = stepSet.Select( a => a.PersonAliasId ).FirstOrDefault();
                        StepService.UpdateStepProgramCompletion( stepSet, personAliasId, stepProgramId );
                    }
                }
               
            }

            CreateEntitySet( personIds, rockContext );

            if ( !anyRemaining )
            {
                DeleteEntitySet( rockContext );

                // delete job if there are no un-migrated history rows  left
                var jobId = context.GetJobId();
                var jobService = new ServiceJobService( rockContext );
                var job = jobService.Get( jobId );
                if ( job != null )
                {
                    jobService.Delete( job );
                    rockContext.SaveChanges();
                    return;
                }
            }
        }

        #region Helper Methods

        /// <summary>
        /// Creates the entity set.
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <param name="rockContext">The rock context.</param>
        /// <returns></returns>
        private void CreateEntitySet( List<int> ids, RockContext rockContext )
        {
            var service = new EntitySetService( rockContext );
            var entitySet = service.Get( _entitySetGuid.AsGuid() );
            if ( entitySet == null )
            {
                entitySet = new EntitySet();
                entitySet.EntityTypeId = EntityTypeCache.Get<Rock.Model.Person>().Id;
                entitySet.ExpireDateTime = RockDateTime.Now.AddDays( 1 );
                entitySet.Guid = _entitySetGuid.AsGuid();
                service.Add( entitySet );
                rockContext.SaveChanges();
            }

            if ( ids != null && ids.Any() )
            {
                List<EntitySetItem> entitySetItems = new List<EntitySetItem>();

                foreach ( var id in ids )
                {
                    var item = new EntitySetItem();
                    item.EntitySetId = entitySet.Id;
                    item.EntityId = id;
                    entitySetItems.Add( item );
                }

                rockContext.BulkInsert( entitySetItems );
            }
        }

        /// <summary>
        /// Creates the entity set.
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <param name="rockContext">The rock context.</param>
        /// <returns></returns>
        private void DeleteEntitySet( RockContext rockContext )
        {
            var service = new EntitySetService( rockContext );
            var entitySet = service.Get( _entitySetGuid.AsGuid() );

            if ( entitySet != null )
            {
                var entitySetItemQry = new EntitySetItemService( rockContext )
                    .Queryable()
                    .AsNoTracking()
                    .Where( i => i.EntitySetId == entitySet.Id );
                rockContext.BulkDelete( entitySetItemQry );

                service.Delete( entitySet );
            }
        }

        /// <summary>
        /// Get the entity set identifier query.
        /// </summary>
        /// <param name="rockContext">The rock context.</param>
        /// <returns></returns>
        private IQueryable<int> GetEntitySetIdQuery( RockContext rockContext )
        {
            var service = new EntitySetService( rockContext );
            var entitySet = service.Get( _entitySetGuid.AsGuid() );
            if ( entitySet == null )
            {
                entitySet = new EntitySet();
                entitySet.EntityTypeId = EntityTypeCache.Get<Rock.Model.Person>().Id;
                entitySet.ExpireDateTime = RockDateTime.Now.AddDays( 30 );
                entitySet.Guid = _entitySetGuid.AsGuid();
                service.Add( entitySet );
                rockContext.SaveChanges();
            }

            return new EntitySetItemService( rockContext )
                .Queryable().AsNoTracking()
                .Where( i => i.EntitySetId == entitySet.Id )
                .Select( i => i.EntityId );
        }

        #endregion Helper Methods
    }
}
