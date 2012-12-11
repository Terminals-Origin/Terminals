using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SqlScriptRunner.Arguments
{
    public interface IDeployArguments
    {

        string ConnectionString { get; set; }
        string DbProviderFactory { get; set; }
        string ArtifactFile { get; set; }
        string ScriptProcessor { get; set; }
        string VersionParser { get; set; }

        //string ScriptPattern { get; set; } hard code to *.*
        //bool Transactional { get; set; } hard code to true
        //bool Recurse { get; set; }  hard code to true
        //bool BreakOnError { get; set; } hard code to true
        //string MinimumVersion { get; set; } hard code to 0.0.0
        //string MaximumVersion { get; set; } hard code to 9999.9999.9999


    }
}
