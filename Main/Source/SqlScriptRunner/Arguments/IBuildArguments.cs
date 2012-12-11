using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SqlScriptRunner.Arguments
{
    public interface IBuildArguments
    {

        string ConnectionString { get; set; }
        string DbProviderFactory { get; set; }
        string ScriptPath { get; set; }
        string ScriptPattern { get; set; }
        bool Transactional { get; set; }
        string ScriptProcessor { get; set; }
        bool Recurse { get; set; }
        bool DryRun { get; set; }
        bool BreakOnError { get; set; }
        string MinimumVersion { get; set; }
        string MaximumVersion { get; set; }
        string VersionParser { get; set; }


    }
}
