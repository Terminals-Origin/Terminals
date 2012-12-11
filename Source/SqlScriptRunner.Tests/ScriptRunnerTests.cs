using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using NUnit.Framework;


namespace SqlScriptRunner.Tests
{
    [TestFixture]
    public class ScriptRunnerTests
    {
        [Test]
        [ExpectedException(typeof(SQLiteException))]
        public void Transaction_Test()
        {
            
            //Add a table, insert some data, and then crash.
            //this *should* roll back the transaction entirely
            string query = Mother.AddTable();
            query += "\n";
            query += Mother.InsertData();
            query += "\n";
            query += Mother.MalformedScript();

            var scriptRunner = new SqlScriptRunner.ScriptRunner(query, new ScriptProcessing.SqliteScriptProcessor());
            
            using (var connection = new SQLiteConnection(Mother.ConnectionString()))
            {
                connection.Open();
                using(var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var success = scriptRunner.Execute(connection, transaction);
                        if(success) transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();                        
                    }
                }                
            }

            //Now that the transaction has failed, lets assert
            //make sure that the data is actually in the DB now...
            using (var connection = new SQLiteConnection(Mother.ConnectionString()))
            {
                connection.Open();
                System.Data.IDbCommand cmd = connection.CreateCommand();
                cmd.CommandText = Mother.SelectData();
                //this should throw our sqliteexception
                var data = cmd.ExecuteReader();
            }

        }

        [Test]
        public void Explicit_Script_From_Full_Path()
        {

            SortedList<string, string> files = ScriptRunner.ResolveScriptsFromPath(Mother.ScriptsRootDirectory(), "Script1.sql", false, Mother.ResolveEnvironmentDirectory());
            Assert.IsTrue(files.Count==1);            

        }
        [Test]
        public void Explicit_Script_From_Relative_Path()
        {

            SortedList<string, string> files = ScriptRunner.ResolveScriptsFromPath("Scripts", "Script1.sql", false, Mother.ResolveEnvironmentDirectory());
            Assert.IsTrue(files.Count == 1);

        }
        [Test]
        public void Explicit_Script_From_WildCard_FileName()
        {

            SortedList<string, string> files = ScriptRunner.ResolveScriptsFromPath(Mother.ScriptsRootDirectory(), "*.sql", false, Mother.ResolveEnvironmentDirectory());
            Assert.IsTrue(files.Count == 3);

        }
        [Test]
        public void Explicit_Script_From_WildCard_FileName_Recursive()
        {

            SortedList<string, string> files = ScriptRunner.ResolveScriptsFromPath(Mother.ScriptsRootDirectory(), "*.sql", true, Mother.ResolveEnvironmentDirectory());
            Assert.IsTrue(files.Count == 5);

        }
        [Test]
        public void ConstructorTest()
        {
            var ScriptRunner = new SqlScriptRunner.ScriptRunner(Mother.AddTable(), new ScriptProcessing.SqliteScriptProcessor());

        }

        [Test]
        public void ExecuteAggregateScript()
        {
            var ScriptRunner = new SqlScriptRunner.ScriptRunner(Mother.CreateTableInsertDataDropTable(),
                                                                    new ScriptProcessing.SqliteScriptProcessor());

            using (var connection = new SQLiteConnection(Mother.ConnectionString()))
            {
                var success = ScriptRunner.Execute(connection);
                Assert.IsTrue(success);
            }
            Mother.DropTable(Mother.ConnectionString());
        }

        [Test]
        public void TestGOProcessor_SimpleReplace()
        {
            var p = new ScriptProcessing.SqlServerScriptProcessor();
            string test = "A;\nGO\n";
            var d = new Dictionary<string, string>();
            d.Add("A", "B");
            IList<string> reponse = p.ProcessScript(test, d);

            Assert.IsTrue(reponse[0] == "B;\n");
        }
        [Test]
        public void TestGOProcessor_Breaker()
        {
            var p = new ScriptProcessing.SqlServerScriptProcessor();
            string test = "AGO\r\nA\r\nGO\nA\nGO\n\nA\n  GO\nA\nGO\n  ";
            var d = new Dictionary<string, string>();
            d.Add("A", "B");
            IList<string> reponse = p.ProcessScript(test, d);
            foreach (string s in reponse)
            {
                Assert.IsTrue(s.Trim() == "B");
            }

        }
        [Test]
        public void TestSemiColonProcessor_SimpleReplace()
        {
            var p = new ScriptProcessing.SqliteScriptProcessor();
            string test = "A;\n";
            var d = new Dictionary<string, string>();
            d.Add("A", "B");
            IList<string> reponse = p.ProcessScript(test, d);

            Assert.IsTrue(reponse[0]=="B;");
        }

        [Test]
        public void TestSemiColonProcessor_Breaker()
        {
            var p = new ScriptProcessing.SqliteScriptProcessor();
            string test = "A;\r\nA;\r\nA;\nA;\nA;\n  A;\n  A;\n A;\nA;\n A;\n";
            var d = new Dictionary<string, string>();
            d.Add("A", "B");
            IList<string> reponse = p.ProcessScript(test, d);
            foreach (string s in reponse)
            {
                Assert.IsTrue(s.Trim() == "B;");    
            }
            
        }
        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            Mother.DeleteDatabase(Mother.DatabaseFileName());
            SQLiteConnection.CreateFile(Mother.DatabaseFileName());
            Mother.DropTable(Mother.ConnectionString());
        }

        [Test]
        public void AddTableTest()
        {
            var ScriptRunner = new SqlScriptRunner.ScriptRunner(Mother.AddTable(), new ScriptProcessing.SqliteScriptProcessor());

            using (var connection = new SQLiteConnection(Mother.ConnectionString()))
            {
                var success = ScriptRunner.Execute(connection);
                Assert.IsTrue(success);
            }
            Mother.DropTable(Mother.ConnectionString());

        }
        [Test]
        public void AddTableInsertDataTest()
        {
            var ScriptRunner = new SqlScriptRunner.ScriptRunner(Mother.AddTable(), new ScriptProcessing.SqliteScriptProcessor());

            using (var connection = new SQLiteConnection(Mother.ConnectionString()))
            {
                var success = ScriptRunner.Execute(connection);

                if (success)
                {
                    ScriptRunner = new ScriptRunner(Mother.InsertData());
                    bool s = ScriptRunner.Execute(connection);
                    Assert.IsTrue(s);

                    //make sure that the data is actually in the DB now...
                    System.Data.IDbCommand cmd = connection.CreateCommand();
                    cmd.CommandText = Mother.SelectData();
                    var data = cmd.ExecuteReader();
                    Assert.IsNotNull(data);
                    Assert.IsTrue(data.FieldCount > 0);
                    data.Read();
                    Assert.IsTrue(data[0] != null);
                    Assert.IsNotNull(data[0]);
                }

            }
            Mother.DropTable(Mother.ConnectionString());
        }

        [Test]
        public void AddTableInsertDataByTransactionTest()
        {
            var ScriptRunner = new SqlScriptRunner.ScriptRunner(Mother.AddTable(),
                                                                    new ScriptProcessing.SqliteScriptProcessor());

            using (var connection = new SQLiteConnection(Mother.ConnectionString()))
            {
                var success = false;
                connection.Open();
                using (var t = connection.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    try
                    {
                        success = ScriptRunner.Execute(t.Connection, t);
                        t.Commit();
                    }
                    catch (Exception)
                    {
                        t.Rollback();
                        throw;
                    }

                }
                if (success)
                {
                    ScriptRunner = new ScriptRunner(Mother.InsertData());
                    bool s = ScriptRunner.Execute(connection);
                    Assert.IsTrue(s);

                    //make sure that the data is actually in the DB now...
                    System.Data.IDbCommand cmd = connection.CreateCommand();
                    cmd.CommandText = Mother.SelectData();
                    var data = cmd.ExecuteReader();
                    Assert.IsNotNull(data);
                    Assert.IsTrue(data.FieldCount > 0);
                    data.Read();
                    Assert.IsTrue(data[0] != null);
                    Assert.IsNotNull(data[0]);
                }
            }

            Mother.DropTable(Mother.ConnectionString());
        }

        [Test]
        public void AddTableInsertDataTemplatedTest()
        {
            var ScriptRunner = new SqlScriptRunner.ScriptRunner(Mother.AddTable(), new ScriptProcessing.SqliteScriptProcessor());
            ScriptRunner.Parameters = new Dictionary<string, string>();

            using (var connection = new SQLiteConnection(Mother.ConnectionString()))
            {
                var success = ScriptRunner.Execute(connection);

                if (success)
                {
                    ScriptRunner = new ScriptRunner(Mother.TemplatedInsert(), new ScriptProcessing.SqliteScriptProcessor());

                    long yValue = Mother.TemplateInteger();
                    long zValue = Mother.TemplateInteger();
                    ScriptRunner.Parameters.Add("ZVALUE", zValue.ToString());
                    ScriptRunner.Parameters.Add("YVALUE", yValue.ToString());


                    bool s = ScriptRunner.Execute(connection);
                    Assert.IsTrue(s);

                    //make sure that the data is actually in the DB now...
                    System.Data.IDbCommand cmd = connection.CreateCommand();
                    cmd.CommandText = Mother.SelectDataByYValue(yValue);

                    var data = cmd.ExecuteReader();
                    Assert.IsNotNull(data);
                    Assert.IsTrue(data.FieldCount > 0);
                    data.Read();
                    long zValueDB = (long)data[2];
                    Assert.AreEqual(zValueDB, zValue);

                }

            }
            Mother.DropTable(Mother.ConnectionString());

        }


        [Test]
        [ExpectedException(typeof(System.Data.SQLite.SQLiteException))]
        public void BadScript()
        {
            var ScriptRunner = new SqlScriptRunner.ScriptRunner(Mother.MalformedScript(), new ScriptProcessing.SqliteScriptProcessor());

            using (var connection = new SQLiteConnection(Mother.ConnectionString()))
            {
                var success = ScriptRunner.Execute(connection);
                Assert.IsFalse(success);
            }
        }

        [TestFixtureTearDown]
        public void FixtureTearDown()
        {
            Mother.DeleteDatabase(Mother.DatabaseFileName());
        }
    }
}