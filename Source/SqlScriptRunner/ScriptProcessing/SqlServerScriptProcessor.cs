using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SqlScriptRunner.ScriptProcessing
{
    public class SqlServerScriptProcessor : IScriptProcessor
    {
        public IList<string> ProcessScript(string Input, IDictionary<string, string> Parameters)
        {
            string[] list = System.Text.RegularExpressions.Regex.Split(Input.Trim(), @"GO\n|GO\r\n", RegexOptions.IgnoreCase);
            var final = new List<string>();
            foreach (var s in list)
            {
                var finalString = s;
                if (finalString.EndsWith("GO"))
                    finalString = finalString.Substring(0, finalString.Length - 2);

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
