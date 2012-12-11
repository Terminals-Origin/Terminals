using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SqlScriptRunner.ScriptProcessing
{
    public class SqliteScriptProcessor : IScriptProcessor
    {
        public IList<string> ProcessScript(string Input, IDictionary<string, string> Parameters)
        {
            string[] list = System.Text.RegularExpressions.Regex.Split(Input.Trim(), @";\n|;\r\n");
            var final = new List<string>();
            foreach (var s in list)
            {
                var finalString = s;
                if(!finalString.EndsWith(";")) finalString += ";";

                if (Parameters != null && Parameters.Count > 0)
                {
                    foreach (var pKey in Parameters.Keys)
                    {
                        finalString = (finalString.Replace(pKey, Parameters[pKey]));
                    }
                }
                if (!String.IsNullOrEmpty(finalString)) final.Add(finalString);
            }
            return final;

        }
    }
}