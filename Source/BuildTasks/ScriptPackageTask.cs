using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Build.Framework;
using SqlScriptRunner.Logging;

namespace BuildTasks
{
    public class ScriptPackageTask : ITask, SqlScriptRunner.Arguments.IPackageArguments
    {
        public IBuildEngine BuildEngine { get; set; }
        public ITaskHost HostObject { get; set; }
        public ScriptPackageTask()
        {
            //defaults
            if (string.IsNullOrEmpty(ScriptPattern)) ScriptPattern = "*.sql";
            Recurse = true;
            DryRun = false;
            MinimumVersion = "0.0.0";
            MaximumVersion = "9999.9999.9999";

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
                Logger.Info("================================ScriptPackageTask : Start Execute");
                ImpactedFiles = SqlScriptRunner.ExecutionTasks.PackageTask.Package(this, Logger);
                foreach (var file in ImpactedFiles)
                {
                    Logger.Info(file);
                }
                Logger.Info("================================ScriptPackageTask : End Execute");

                return true;

            }
            catch (Exception e)
            {
                Logger.Fatal("", e);
                return false;
            }


        }

        [Required]
        public string ScriptPath { get; set; }
        public string ScriptPattern { get; set; }
        public bool Recurse { get; set; }
        public string MinimumVersion { get; set; }
        public string MaximumVersion { get; set; }
        [Required]
        public string ArtifactFile { get; set; }
        public bool DryRun { get; set; }

        public string VersionParser { get; set; }

        [Output]
        public string[] ImpactedFiles { get; set; }

        public string LoggerType { get; set; }
        public SqlScriptRunner.Logging.ILog Logger { get; set; }
    }
}