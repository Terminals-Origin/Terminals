using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using Version = SqlScriptRunner.Versioning.Version;

namespace SqlScriptRunner
{
    public class ScriptRunner
    {
        public static SortedList<string, string> ResolveScriptsFromPathAndVersion(string path, string pattern, bool recurse, string workingDirectory, Versioning.Version source, Versioning.Version destination, Versioning.IParseVersions versionParser = null)
        {

            SortedList<string, string> files = new SortedList<string, string>();
            if (versionParser == null) versionParser = new Versioning.VersionDateParser();
            string usedPath = path;
            if (!System.IO.Directory.Exists(usedPath)) usedPath = System.IO.Path.Combine(workingDirectory, path);
            if (!System.IO.Directory.Exists(usedPath)) throw new ArgumentOutOfRangeException("path");
            foreach (var d in System.IO.Directory.GetDirectories(usedPath))
            {
                System.IO.DirectoryInfo dir = new DirectoryInfo(d);
                Versioning.Version dirVersion = versionParser.Parse(dir.Name);
                if (dirVersion >= source && dirVersion <= destination)
                {
                    var scripts = ResolveScriptsFromPath(dir.FullName, pattern, recurse, workingDirectory);
                    foreach (string key in scripts.Keys)
                    {
                        files.Add(key, scripts[key]);
                    }

                }
            }

            return files;
        }

        public static SortedList<string, string> ResolveScriptsFromPath(string path, string pattern, bool recurse, string workingDirectory)
        {
            if (!System.IO.Directory.Exists(path)) path = System.IO.Path.Combine(workingDirectory, path);
            if(!System.IO.Directory.Exists(path)) throw new ArgumentException("Unable to resolve the Path:" + path, "path");

            SearchOption option = SearchOption.TopDirectoryOnly;
            if(recurse) option = SearchOption.AllDirectories;
            return new SortedList<string, string>(System.IO.Directory.GetFiles(path, pattern, option).ToDictionary(s => s));
        }

        public ScriptRunner(string script = null, ScriptProcessing.IScriptProcessor processor = null)
        {
            Parameters = new System.Collections.Generic.Dictionary<string, string>();
            _script = script;
            this._processor = processor;
        }

        public string Filename { get; set; }
        private string _script;
        public System.Collections.Generic.IDictionary<string, string> Parameters { get; set; }
        private ScriptProcessing.IScriptProcessor _processor;

        public bool Execute(System.Data.IDbConnection connection, System.Data.IDbTransaction transaction = null)
        {
            if (_processor == null) _processor = new ScriptProcessing.SqlServerScriptProcessor();
            if (connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }
            IList<string> bits = _processor.ProcessScript(_script, Parameters);
            foreach (string bit in bits)
            {
                System.Data.IDbCommand cmd = connection.CreateCommand();
                if(transaction!=null) cmd.Transaction = transaction;
                cmd.CommandText = bit;
                cmd.ExecuteNonQuery();
            }

            return true;
        }

 
    }
}