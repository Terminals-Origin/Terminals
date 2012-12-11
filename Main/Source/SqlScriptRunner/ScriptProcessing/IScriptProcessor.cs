using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SqlScriptRunner.ScriptProcessing
{
    public interface IScriptProcessor
    {
        IList<string> ProcessScript(string Input, IDictionary<string, string> Parameters);
    }
}
