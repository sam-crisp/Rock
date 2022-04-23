using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

using Rock;
using Rock.Data;

namespace Rock.CodeGeneration
{
    internal static class RockGuidCodeGenerator
    {
        private static Dictionary<string, string[]> RockGuidSearchFileNamesLookup = new Dictionary<string, string[]>();

        public static void EnsureRockGuidAttributes( string rootFolder )
        {
            var entityTypeDatabaseGuidLookup = GetDatabaseGuidLookup( rootFolder, "SELECT [Guid], [Name] FROM [EntityType]", "Name" );
            EnsureRockGuidAttributesForType<Rock.Data.IEntity, RockEntityTypeGuidAttribute>( rootFolder, entityTypeDatabaseGuidLookup );
            EnsureRockGuidAttributesForType<Rock.Blocks.IRockBlockType, RockEntityTypeGuidAttribute>( rootFolder, entityTypeDatabaseGuidLookup );

            EnsureRockGuidAttributesForType<Rock.Achievement.AchievementComponent, RockEntityTypeGuidAttribute>( rootFolder, entityTypeDatabaseGuidLookup );
            EnsureRockGuidAttributesForType<Rock.Badge.BadgeComponent, RockEntityTypeGuidAttribute>( rootFolder, entityTypeDatabaseGuidLookup );
            EnsureRockGuidAttributesForType<Rock.Reporting.DataFilterComponent, RockEntityTypeGuidAttribute>( rootFolder, entityTypeDatabaseGuidLookup );
            EnsureRockGuidAttributesForType<Rock.Reporting.DataSelectComponent, RockEntityTypeGuidAttribute>( rootFolder, entityTypeDatabaseGuidLookup );
            EnsureRockGuidAttributesForType<Rock.Reporting.DataTransformComponent, RockEntityTypeGuidAttribute>( rootFolder, entityTypeDatabaseGuidLookup );
            EnsureRockGuidAttributesForType<Rock.Workflow.ActionComponent, RockEntityTypeGuidAttribute>( rootFolder, entityTypeDatabaseGuidLookup );
            EnsureRockGuidAttributesForType<Rock.Field.FieldType, RockFieldTypeGuidAttribute>( rootFolder, GetDatabaseGuidLookup( rootFolder, $"SELECT [Guid], [Class] FROM [FieldType]", "Class" ) );

            string rockBlockLookupSql = @"SELECT bt.Guid
    , et.Name [Class]
FROM BlockType bt
JOIN EntityType et
    ON bt.EntityTypeId IS NOT NULL
        AND et.Id = bt.EntityTypeId
";
            EnsureRockGuidAttributesForType<Rock.Blocks.IRockBlockType, RockBlockTypeGuidAttribute>( rootFolder, GetDatabaseGuidLookup( rootFolder, rockBlockLookupSql, "Class" ) );

            // Entity Type blocks
            //EnsureRockGuidAttributesForType<Rock.Blocks.RockBlockType>( rootFolder, "BlockType", "EntityTypeId" );

            // WebForm blocks
            // EnsureRockGuidAttributesForType<Rock.Web.UI.RockBlock>( Path.Combine( rootFolder, "RockWeb"), "BlockType", "Path" );


            //EnsureRockGuidAttributesForType<Rock.Rest.ApiControllerBase>( rootFolder, "EntityType", "Class" );

        }

        private static void EnsureRockGuidAttributesForType<T, GA>( string rootFolder, Lazy<Dictionary<string, Guid>> databaseGuidLookup )
            where T : class
            where GA : RockGuidAttribute

        {
            var rockAssembly = typeof( T ).Assembly;
            var systemGuidTypeFields = rockAssembly.GetTypes()
                .Where( a => a.Namespace == "Rock.SystemGuid" )
                    .SelectMany( a =>
                         a.GetFields()
                             .Select( s => new { Type = a, Field = s, GuidValue = s.GetValue( null ) as string } ) ).ToList();

            var systemGuidTypeFieldsUnique = systemGuidTypeFields
                .GroupBy( x => x.Field.GetValue( null ) )
                .Select( a => new
                {
                    GuidValue = a.Key,

                    // It is possible that SystemGuids might share the same value, probably because of Obsolete ones. So
                    // just in case there are SystemGuids that share the same value, prefer the ones that are not obsolete, and then take the first one
                    Value = a.OrderBy( x => x.Field.GetCustomAttribute<System.ObsoleteAttribute>() == null ? 0 : 1 ).FirstOrDefault()
                } );

            var systemGuidLookup = systemGuidTypeFieldsUnique.ToDictionary( k => k.GuidValue, v => $"{v.Value.Type.FullName}.{v.Value.Field.Name}" );

            var types = Reflection.FindTypes( typeof( T ) ).Values.OrderBy( a => a.FullName ).ToList();

            types = types.Where( a => a.GetCustomAttribute<GA>() == null ).ToList();
            if ( !types.Any() )
            {
                return;
            }

            var processedTypes = new HashSet<Type>();
            /*UpdateProgressText( $"Rock Guid Attribute - {typeof( T ).FullName}" );
            progressBar1.Maximum = types.Count();
            progressBar1.Value = 0;
            */
            var nameSpaces = types.Select( a => a.Namespace ).Distinct().ToArray();

            Dictionary<Type, List<string>> possibleClassDeclarationsCache = new Dictionary<Type, List<string>>();

            foreach ( var fileName in GetRockGuidSearchFileNames( rootFolder ) )
            {
                types = types.Where( a => !processedTypes.Contains( a ) ).ToList();

                if ( !types.Any() )
                {
                    return;
                }

                var sourceFileText = File.ReadAllText( fileName );

                var fileHasNameSpaces = nameSpaces.Any( x => sourceFileText.Contains( $"namespace {x}" ) );
                if ( !fileHasNameSpaces )
                {
                    continue;
                }

                var sourceFileLines = File.ReadAllLines( fileName );
                bool fileUsingsIncludeRockGuidNameSpace = sourceFileText.Contains( $"using {typeof( GA ).Namespace};" );

                foreach ( var type in types )
                {
                    var possibleClassDeclarations = possibleClassDeclarationsCache.GetValueOrNull( type );
                    if ( possibleClassDeclarations == null )
                    {
                        possibleClassDeclarations = new List<string>();
                        possibleClassDeclarations.Add( $"public class {type.Name} : " );
                        possibleClassDeclarations.Add( $"public partial class {type.Name} : " );
                        if ( type.IsSealed )
                        {
                            possibleClassDeclarations.Add( $"public sealed class {type.Name} : " );
                        }

                        if ( !type.IsPublic )
                        {
                            possibleClassDeclarations.Add( $"internal class {type.Name} : " );
                            possibleClassDeclarations.Add( $"internal sealed class {type.Name} : " );
                        }

                        possibleClassDeclarationsCache.Add( type, possibleClassDeclarations );
                    }

                    var fileMightHave = possibleClassDeclarations.Any( d => sourceFileText.Contains( d ) );
                    if ( !fileMightHave )
                    {
                        continue;
                    }

                    var sourceFileLine = sourceFileLines.Where( line => possibleClassDeclarations.Any( d => line.Contains( d ) ) ).FirstOrDefault();
                    if ( sourceFileLine.IsNullOrWhiteSpace() )
                    {
                        continue;
                    }

                    var databaseRockGuidValue = databaseGuidLookup.Value.GetValueOrNull( type.FullName )?.ToString().ToUpper();
                    var rockGuidAttributeValue = type.GetCustomAttribute<GA>()?.Guid.ToString().ToUpper();
                    string databaseRockGuidSystemGuidName = databaseRockGuidValue != null ? systemGuidLookup.GetValueOrNull( databaseRockGuidValue ) : null;
                    string rockGuidDeclaration;
                    var attributeTypeName = typeof( GA ).Name.ReplaceIfEndsWith( "Attribute", string.Empty );
                    var attributeTypeFullName = typeof( GA ).FullName.ReplaceIfEndsWith( "Attribute", string.Empty );
                    if ( fileUsingsIncludeRockGuidNameSpace )
                    {
                        rockGuidDeclaration = attributeTypeName;
                    }
                    else
                    {
                        rockGuidDeclaration = attributeTypeFullName;
                    }

                    if ( rockGuidAttributeValue == null )
                    {
                        string guidLine;
                        if ( databaseRockGuidSystemGuidName.IsNotNullOrWhiteSpace() )
                        {
                            guidLine = $"    [{rockGuidDeclaration}( {databaseRockGuidSystemGuidName} )]";
                        }
                        else
                        {
                            var guidValue = databaseRockGuidSystemGuidName ?? databaseRockGuidValue ?? Guid.NewGuid().ToString().ToUpper();
                            guidLine = $"    [{rockGuidDeclaration}( \"{guidValue}\")]";
                        }

                        var alreadyCodeGeneratedButNotCompiled = sourceFileText.Contains( guidLine );
                        if ( !alreadyCodeGeneratedButNotCompiled )
                        {
                            sourceFileText = sourceFileText.Replace( sourceFileLine, $"{guidLine}{Environment.NewLine}{ sourceFileLine} " );
                            File.WriteAllText( fileName, sourceFileText );
                        }
                    }
                    else
                    {
                        if ( databaseRockGuidValue.IsNotNullOrWhiteSpace() && rockGuidAttributeValue != databaseRockGuidValue )
                        {
                            sourceFileText = sourceFileText.Replace( $"[{attributeTypeFullName}(\"{rockGuidAttributeValue}\")]", $"{rockGuidDeclaration}(\"{databaseRockGuidSystemGuidName ?? databaseRockGuidValue}\")]" );
                            sourceFileText = sourceFileText.Replace( $"[{attributeTypeName}(\"{rockGuidAttributeValue}\")]", $"[{rockGuidDeclaration}(\"{databaseRockGuidSystemGuidName ?? databaseRockGuidValue}\")]" );
                        }
                    }

                    //progressBar1.Value++;
                    processedTypes.Add( type );
                    break;
                }
            }

            //progressBar1.Maximum = 0;
            //progressBar1.Value = 0;

            foreach ( var unprocessedType in types.Where( a => !processedTypes.Contains( a ) ).ToList() )
            {
                Debug.WriteLine( $"Couldn't find source code for {unprocessedType.FullName}" );
            }
        }

        private static Lazy<Dictionary<string, Guid>> GetDatabaseGuidLookup( string rootFolder, string sql, string classField )
        {
            return new Lazy<Dictionary<string, Guid>>( () =>
            {
                SqlConnection sqlconn = CodeGenHelpers.GetSqlConnection( rootFolder );
                sqlconn.Open();
                var qry = sqlconn.CreateCommand();
                qry.CommandType = System.Data.CommandType.Text;
                qry.CommandText = sql;
                DataSet dataSet = new DataSet();
                new SqlDataAdapter( qry ).Fill( dataSet );
                var databaseGuidLookup = dataSet.Tables[0].Rows.OfType<DataRow>().ToList().ToDictionary( k => k.Field<string>( classField ), v => v.Field<Guid>( "Guid" ) );
                return databaseGuidLookup;
            }
            );
        }

        private static string[] GetRockGuidSearchFileNames( string rootFolder )
        {
            var result = RockGuidSearchFileNamesLookup.GetValueOrNull( rootFolder );
            if ( result == null )
            {
                var searchDirectory = rootFolder.EnsureTrailingBackslash();
                var sourceFileNames = Directory.EnumerateFiles( searchDirectory, "*.cs", SearchOption.AllDirectories );

                sourceFileNames = sourceFileNames.Where( a => !a.Contains( ".localhistory" ) );
                sourceFileNames = sourceFileNames.Where( a => !a.Contains( "\\CodeGenerated\\" ) );
                result = sourceFileNames.OrderBy( a => a.Contains( @"\Rock\Rock\" ) ? 0 : 1 ).ToArray();
                RockGuidSearchFileNamesLookup.Add( rootFolder, result );
            }

            return result;
        }
    }
}