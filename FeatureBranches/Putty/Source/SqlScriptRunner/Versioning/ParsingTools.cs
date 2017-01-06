using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SqlScriptRunner.Versioning
{
    public class ParsingTools
    {
        public static int ParseIndex(string[] ary, int index)
        {

            int value = 0;
            int.TryParse(ary[index], out value);
            return value;
        }
    }
}
