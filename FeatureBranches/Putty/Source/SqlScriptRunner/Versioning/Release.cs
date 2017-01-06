using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SqlScriptRunner.Versioning
{
    public class Release
    {
        public Version FromVersion { get; set; }
        public Version ToVersion { get; set; }
        public string Description { get; set; }
        public string SourcePath { get; set; }

        public SortedList<string, string> Scripts { get; set; }

        public SortedList<string, string> LoadScripts(string path, string pattern, bool recurse, string workingDirectory, Versioning.IParseVersions versionParser = null)
        {
            SortedList<string, string> s = ScriptRunner.ResolveScriptsFromPathAndVersion(path, pattern, recurse, workingDirectory, FromVersion,
                                                          ToVersion, versionParser);


            return s;
        }
    }
}
