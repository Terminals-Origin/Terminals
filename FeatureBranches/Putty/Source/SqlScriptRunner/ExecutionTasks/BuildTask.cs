using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data;

namespace SqlScriptRunner.ExecutionTasks
{
    public class BuildTask
    {
        public static string[] Build(Arguments.IBuildArguments buildArguments, Logging.ILog log = null)
        {
            if (log == null) log = new Logging.NoLogging();
            if (string.IsNullOrEmpty(buildArguments.ScriptPath))
                buildArguments.ScriptPath = System.Environment.CurrentDirectory;

            if (string.IsNullOrEmpty(buildArguments.DbProviderFactory))
                buildArguments.DbProviderFactory = "System.Data.SqlClient";

            if (string.IsNullOrEmpty(buildArguments.ScriptProcessor))
                buildArguments.ScriptProcessor = typeof(SqlScriptRunner.ScriptProcessing.SqlServerScriptProcessor).FullName;

            if (string.IsNullOrEmpty(buildArguments.VersionParser))
                buildArguments.VersionParser = typeof(SqlScriptRunner.Versioning.VersionDateParser).AssemblyQualifiedName;


            Type t = Type.GetType(buildArguments.VersionParser);

            SqlScriptRunner.Versioning.IParseVersions versionParser =
                (Activator.CreateInstance(t) as
                 SqlScriptRunner.Versioning.IParseVersions);


            SqlScriptRunner.Versioning.Version minVersion = null;
            SqlScriptRunner.Versioning.Version maxVersion = null;

            if (string.IsNullOrEmpty(buildArguments.MinimumVersion))
            {
                minVersion = SqlScriptRunner.Versioning.Version.Min;
            }
            else
            {
                minVersion = versionParser.Parse(buildArguments.MinimumVersion);
            }

            if (string.IsNullOrEmpty(buildArguments.MaximumVersion))
            {
                maxVersion = SqlScriptRunner.Versioning.Version.Max;
            }
            else
            {
                maxVersion = versionParser.Parse(buildArguments.MaximumVersion);
            }
            log.Info("--------------------------------");
            log.Info(string.Format("Min:{0}, Max:{1}, ScriptPath:{2}, Transactional:{4}, DryRun:{5}\r\nConnectionString:{3}", minVersion, maxVersion, buildArguments.ScriptPath, buildArguments.ConnectionString, buildArguments.Transactional, buildArguments.DryRun));
            log.Info("--------------------------------");
            DbConnection connection = null;

            if (!buildArguments.DryRun)
            {


                //make sure we can connect to the database
                DbProviderFactory factory = DbProviderFactories.GetFactory(buildArguments.DbProviderFactory);
                connection = factory.CreateConnection();
                if (connection == null)
                {
                    throw new ArgumentException(
                        "Could not create a connection to the database, via the Provider Factory:" +
                        buildArguments.DbProviderFactory);
                }
                else
                {
                    connection.ConnectionString = buildArguments.ConnectionString;
                    connection.Open();
                }
            }

            SortedList<string, string> Files = SqlScriptRunner.ScriptRunner.ResolveScriptsFromPathAndVersion(buildArguments.ScriptPath, buildArguments.ScriptPattern, buildArguments.Recurse, System.Environment.CurrentDirectory, minVersion, maxVersion, versionParser);
            log.Info(string.Format("Resolved:{0} files.", Files.Count));

            foreach (var file in Files.Keys)
            {
                log.Info(file);
                if (!buildArguments.DryRun)
                {
                    try
                    {

                        log.Info("Executing");
                        string script = System.IO.File.ReadAllText(Files[file]);
                        SqlScriptRunner.ScriptRunner runner = new ScriptRunner(script, null);
                        if (buildArguments.Transactional)
                        {
                            if (connection.State == ConnectionState.Closed) connection.Open();
                            System.Data.IDbTransaction transaction = null;
                            if (buildArguments.Transactional) transaction = connection.BeginTransaction();
                            try
                            {

                                runner.Execute(connection, transaction);
                                if (buildArguments.Transactional) transaction.Commit();
                                log.Info("Success:" + file);
                            }
                            catch (Exception e)
                            {
                                log.Info("Fail [In Trx:" + buildArguments.Transactional + "]:" + file);
                                log.Fatal(e);
                                if (buildArguments.Transactional) transaction.Rollback();
                                throw;
                            }
                        }
                        else
                        {
                            runner.Execute(connection);
                        }
                    }
                    catch (Exception e)
                    {
                        if (buildArguments.BreakOnError)
                        {
                            throw;
                        }
                        else
                        {
                            log.Debug("There was an error with a script, since BreakOnError is false, we will continue.File:" + file, e);
                        }
                    }
                }
            }
            log.Info("Done Executing");
            return (from f in Files select f.Value).ToArray();

        }
    }
}
