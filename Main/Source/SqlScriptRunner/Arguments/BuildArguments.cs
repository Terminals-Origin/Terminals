using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SqlScriptRunner.Arguments
{
    public class BuildArguments : Arguments.IBuildArguments
    {
        public string ConnectionString { get; set; }
        public string DbProviderFactory { get; set; }
        public string ScriptPath { get; set; }
        public string ScriptPattern { get; set; }
        public bool Transactional { get; set; }
        public string ScriptProcessor { get; set; }
        public bool Recurse { get; set; }
        public bool DryRun { get; set; }
        public bool BreakOnError { get; set; }
        public string MinimumVersion { get; set; }
        public string MaximumVersion { get; set; }
        public string VersionParser { get; set; }
    }
}
