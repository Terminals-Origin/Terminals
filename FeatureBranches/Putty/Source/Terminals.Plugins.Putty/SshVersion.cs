using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Terminals.Plugins.Putty
{
    public enum SshVersion : byte
    {
        SshNegotiate = 0,
        SshVersion1 = 1,
        SshVersion2 = 2
    }
}
