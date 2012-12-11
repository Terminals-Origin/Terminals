using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Build.Framework;
using SqlScriptRunner.Logging;

namespace BuildTasks
{
    public class ScriptDeployTask : ITask, SqlScriptRunner.Arguments.IDeployArguments
    {
        public IBuildEngine BuildEngine { get; set; }
        public ITaskHost HostObject { get; set; }

        public ScriptDeployTask()
        {
            //defaults
            if (string.IsNullOrEmpty(DbProviderFactory)) DbProviderFactory = "System.Data.SqlClient";
            if (string.IsNullOrEmpty(ScriptProcessor)) ScriptProcessor = typeof(SqlScriptRunner.ScriptProcessing.SqlServerScriptProcessor).AssemblyQualifiedName;
            if (string.IsNullOrEmpty(LoggerType)) LoggerType = typeof(ConsoleLogger).AssemblyQualifiedName;

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
                Logger.Info("================================ScriptDeployTask : Start Execute");
                ImpactedFiles = SqlScriptRunner.ExecutionTasks.DeployTask.Deploy(this, Logger);
                Logger.Info("================================ScriptDeployTask : End Execute");

                return true;

            }
            catch (Exception e)
            {
                Logger.Fatal("", e);
                return false;
            }

        }



        [Required]
        public string ConnectionString { get; set; }
        public string DbProviderFactory { get; set; }

        [Required]
        public string ArtifactFile { get; set; }
        public string ScriptProcessor { get; set; }
        public string VersionParser { get; set; }

        [Output]
        public string[] ImpactedFiles { get; set; }

        public string LoggerType { get; set; }
        public ILog Logger { get; set; }
    }
}