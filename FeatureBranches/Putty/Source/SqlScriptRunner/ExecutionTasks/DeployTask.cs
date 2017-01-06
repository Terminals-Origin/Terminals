using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.SharpZipLib.Zip;
using SqlScriptRunner.Arguments;

namespace SqlScriptRunner.ExecutionTasks
{
    public class DeployTask
    {
        public static string[] Deploy(Arguments.IDeployArguments deployArguments, Logging.ILog log = null)
        {
            if (log == null) log = new Logging.NoLogging();

            if (System.IO.File.Exists(deployArguments.ArtifactFile))
            {
                string packageFile = deployArguments.ArtifactFile;//System.IO.Path.Combine(deployArguments.ArtifactFolder, PackageTask.PackageFile);
                if (System.IO.File.Exists(packageFile))
                {
                    string tempFolder = System.IO.Path.GetTempPath();
                    string newFolder = System.Guid.NewGuid().ToString();
                    string ourFolder = System.IO.Path.Combine(tempFolder, newFolder);
                    System.IO.Directory.CreateDirectory(ourFolder);
                    ICSharpCode.SharpZipLib.Zip.FastZip fz = new FastZip();

                    fz.ExtractZip(packageFile, ourFolder, null);

                    Arguments.BuildArguments ba = new BuildArguments();
                    ba.ConnectionString = deployArguments.ConnectionString;
                    ba.DbProviderFactory = deployArguments.DbProviderFactory;
                    ba.ScriptProcessor = deployArguments.ScriptProcessor;
                    ba.VersionParser = deployArguments.VersionParser;
                    ba.DryRun = false;
                    ba.BreakOnError = true;
                    ba.MaximumVersion = "99999.99999.99999";
                    ba.MinimumVersion = "0.0.0";
                    ba.Recurse = true;
                    ba.ScriptPath = ourFolder;
                    ba.ScriptPattern = "*.*";
                    ba.Transactional = true;
                    

                    return BuildTask.Build(ba, log);

                }
            }

            return null;
        }
    }
}