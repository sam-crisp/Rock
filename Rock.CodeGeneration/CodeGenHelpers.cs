using System.Data.SqlClient;
using System.IO;
using System.Xml;

namespace Rock.CodeGeneration
{
    internal static class CodeGenHelpers
    {
        public static SqlConnection GetSqlConnection( string rootFolder )
        {
            var file = new FileInfo( Path.Combine( rootFolder, @"RockWeb\web.ConnectionStrings.config" ) );
            if ( !file.Exists )
            {
                return null;
            }

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load( file.FullName );
            XmlNode root = xmlDoc.DocumentElement;
            XmlNode node = root.SelectNodes( "add[@name = \"RockContext\"]" )[0];
            SqlConnection sqlconn = new SqlConnection( node.Attributes["connectionString"].Value );
            return sqlconn;
        }
    }
}