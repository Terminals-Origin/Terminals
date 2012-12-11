using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ICSharpCode.SharpZipLib.Zip;
using SqlScriptRunner.Arguments;

namespace SqlScriptRunner.ExecutionTasks
{
    public class PackageTask
    {
        public static string PackageFile = "Database.Migrations.zip";

        public static string[] Package(Arguments.IPackageArguments packageArguments, Logging.ILog log = null)
        {
            if (log == null) log = new Logging.NoLogging();

            Arguments.BuildArguments ba = new BuildArguments();
            ba.ScriptPath = packageArguments.ScriptPath;
            ba.ScriptPattern = packageArguments.ScriptPattern;
            ba.Recurse = packageArguments.Recurse;
            ba.MinimumVersion = packageArguments.MinimumVersion;
            ba.MaximumVersion = packageArguments.MaximumVersion;
            ba.DryRun = true;
            ba.VersionParser = packageArguments.VersionParser;

            string[] files = BuildTask.Build(ba, log);

            string tempFolder = System.IO.Path.GetTempPath();
            string newFolder = System.Guid.NewGuid().ToString();
            string ourFolder = System.IO.Path.Combine(tempFolder, newFolder);
            string ourZip = System.IO.Path.Combine(tempFolder, newFolder + ".zip");
            System.IO.Directory.CreateDirectory(ourFolder);


            //create copies of our deployable files
            foreach (var file in files)
            {
                System.IO.FileInfo fi = new FileInfo(file);
                string dir = fi.Directory.FullName.Replace(packageArguments.ScriptPath, "");
                string newTempDir = System.IO.Path.Combine(ourFolder, dir);
                if (!System.IO.Directory.Exists(newTempDir)) System.IO.Directory.CreateDirectory(newTempDir);
                System.IO.File.Copy(file, System.IO.Path.Combine(newTempDir, fi.Name));
            }

            string DeploySample = "SqlScriptRunner.Resources.Deploy.sample.msbuild";
            string deploySampleFile = System.IO.Path.Combine(ourFolder, "Deploy.sample.msbuild");

            using (System.IO.Stream stm = typeof(PackageTask).Assembly.GetManifestResourceStream(DeploySample))
            {
                using (System.IO.StreamReader rdr = new StreamReader(stm))
                {
                    string contents = rdr.ReadToEnd();
                    if (!string.IsNullOrEmpty(contents))
                    {
                        System.IO.FileInfo fi = new FileInfo(packageArguments.ArtifactFile);
                        contents = contents.Replace("[PACKAGEFILENAME]", fi.Name);
                        System.IO.File.WriteAllText(deploySampleFile, contents);
                    }
                }
            }

            //zip them up!
            ICSharpCode.SharpZipLib.Zip.FastZip fz = new FastZip();
            fz.CreateZip(ourZip, ourFolder, true, null);

            string path = ourZip;
            if (!packageArguments.DryRun)
            {
                //if (!System.IO.Directory.Exists(packageArguments.ArtifactFolder)) System.IO.Directory.CreateDirectory(packageArguments.ArtifactFolder);
                //path = System.IO.Path.Combine(packageArguments.ArtifactFolder, PackageFile);
                path = packageArguments.ArtifactFile;
                if (System.IO.File.Exists(path)) System.IO.File.Delete(path);
                System.IO.File.Copy(ourZip, path);
            }

            return new string[] { path };
        }
    }

}