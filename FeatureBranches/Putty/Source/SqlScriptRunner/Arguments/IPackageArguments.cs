using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SqlScriptRunner.Arguments
{
    public interface IPackageArguments
    {

        string ScriptPath { get; set; }
        string ScriptPattern { get; set; }
        bool Recurse { get; set; }
        string MinimumVersion { get; set; }
        string MaximumVersion { get; set; }
        string ArtifactFile { get; set; }
        string VersionParser { get; set; }
        bool DryRun { get; set; }


    }
}