using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;

namespace SqlScriptRunner.Tests
{
    public class Mother
    {
        public static string ScriptsRootDirectory()
        {
            return System.IO.Path.Combine(ResolveEnvironmentDirectory(), "Scripts");
        }
        public static string ResolveEnvironmentDirectory()
        {
            System.IO.DirectoryInfo dir = new DirectoryInfo(System.Environment.CurrentDirectory);
            return dir.Parent.Parent.FullName;
        }

        private static string dbFileName = null;
        static string connectionStringTemplate = "Data Source={0};Version=3;Pooling=False;Max Pool Size=100;";

        public static string DatabaseFileName()
        {
            if(dbFileName==null) dbFileName = System.IO.Path.Combine(System.IO.Path.GetTempPath(), System.Guid.NewGuid().ToString());
            return dbFileName;
        }
        public static string ConnectionString()
        {
            return string.Format(connectionStringTemplate, dbFileName);
        }

        static System.Random rnd = new Random();

         public static string CreateTableInsertDataDropTable()
         {
             System.Text.StringBuilder sb = new StringBuilder();
             sb.Append(Mother.AddTable());
             sb.Append("\n\n");
             sb.Append(Mother.InsertData());
             sb.Append("\n\n");
             sb.Append(DropTable());
             sb.Append("\n\n");
             return sb.ToString();
         }

        public static string TemplatedInsert()
        {
            return string.Format("insert into t (y, z) values(YVALUE,ZVALUE);");
        }
        public static int TemplateInteger()
        {
            return rnd.Next();
        }
        public static string AddTable()
        {
            return "CREATE TABLE t(x INTEGER PRIMARY KEY ASC, y, z);";
        }
        public static string SelectData()
        {
            return "SELECT x,y,z FROM t;";
        }
        public static string SelectDataByYValue(long yValue)
        {
            return "SELECT x,y,z FROM t where y = " + yValue + ";";
        }

        public static string InsertData()
        {
            return string.Format("insert into t (y, z) values({0},{1});", rnd.Next(), rnd.Next());
        }
        public static string DeleteData()
        {
            return "DELETE FROM t;";
        }

        public static string DropTable()
        {
            return "drop table if exists t;";
        }
        public static string MalformedScript()
        {
            return "this is not valid";
        }

        public static void DropTable(string connectionString)
        {

            ScriptRunner s = new ScriptRunner(Mother.DropTable(), new ScriptProcessing.SqliteScriptProcessor());
            using (var connection = new SQLiteConnection(connectionString))
            {
                var success = s.Execute(connection);                
            }
        }
        public static void DeleteDatabase(string Filename)
        {
            if (System.IO.File.Exists(Filename)) System.IO.File.Delete(Filename);
        }
    }
}
