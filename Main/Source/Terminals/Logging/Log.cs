using System;
using System.Collections.Generic;
using System.Text;
using log4net;

namespace Terminals{

    public class Logging
    {

        public static readonly ILog Log = LogManager.GetLogger("Terminals");
    }
}
