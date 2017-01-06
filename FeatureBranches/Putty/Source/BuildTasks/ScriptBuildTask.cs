using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Build.Framework;
using SqlScriptRunner.Logging;

namespace BuildTasks
{
    /// <summary>
    /// A build task typically for development environments
    /// To keep local developer & qa machines in sync with the repo
    /// That is, to automatically upgrade/migrate the dev's local DB with what is in the repo
    /// </summary>
    public class ScriptBuildTask : ITask, SqlScriptRunner.Arguments.IBuildArguments
    {
        public IBuildEngine BuildEngine { get; set; }
        public ITaskHost HostObject { get; set; }

        public ScriptBuildTask()
        {
            //defaults
            if (string.IsNullOrEmpty(ScriptPattern)) ScriptPattern = "*.sql";
            if (string.IsNullOrEmpty(DbProviderFactory)) DbProviderFactory = "System.Data.SqlClient";
            if (string.IsNullOrEmpty(ScriptProcessor)) ScriptProcessor = typeof(SqlScriptRunner.ScriptProcessing.SqlServerScriptProcessor).AssemblyQualifiedName;
            if (string.IsNullOrEmpty(VersionParser)) VersionParser = typeof(SqlScriptRunner.Versioning.VersionDateParser).AssemblyQualifiedName;
            
            Recurse = true;            
            Transactional = true;
            BreakOnError = true;
            
        }

        public bool Execute()
        {
            try
            {
                if (string.IsNullOrEmpty(LoggerType))
                {
                    LoggerType = typeof(MSBuildLogger).AssemblyQualifiedName;
                    Logger = new MSBuildLogger(this.BuildEngine);
                }
                else
                {
                    Logger = (System.Activator.CreateInstance(Type.GetType(LoggerType)) as ILog);
                }


                Logger.Info("================================ScriptBuildTask : Start Execute");
                ImpactedFiles = SqlScriptRunner.ExecutionTasks.BuildTask.Build(this, Logger);
                Logger.Info("================================ScriptBuildTask : End Execute");
            }
            catch (Exception e)
            {
                Logger.Fatal("", e);
                return false;
            }
            return true;
        }

        [Required]
        public string ConnectionString { get; set; }
        public string DbProviderFactory { get; set; }
        [Required]
        public string ScriptPath { get; set; }
        public string ScriptPattern { get; set; }
        public bool Transactional { get; set; }
        public string ScriptProcessor { get; set; }
        public bool Recurse { get; set; }
        public bool DryRun { get; set; }
        public bool BreakOnError { get; set; }
        public string MaximumVersion { get; set; }
        public string MinimumVersion { get; set; }
        public string VersionParser { get; set; }

        [Output]
        public string[] ImpactedFiles{ get; set; }

        public string LoggerType { get; set; }
        public ILog Logger { get; set; }

    }
}