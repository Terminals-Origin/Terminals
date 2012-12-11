using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SqlScriptRunner.Versioning
{
    public interface IParseVersions
    {
        Version Parse(string Version);

    }
}