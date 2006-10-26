using System;
using System.Collections.Generic;
using System.Text;

namespace Terminals
{
    public enum DesktopSize { x640=0,x800,x1024,FitToWindow,FullScreen,AutoScale };
    public enum Colors { Bits8 = 0, Bit16, Bits24 };

    internal class ServerFavoriteOld
    {
        public ServerFavoriteOld()
        {
        }

        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private string serverName;

        public string ServerName
        {
            get { return serverName; }
            set { serverName = value; }
        }

        private string domainName;

        public string DomainName
        {
            get { return domainName; }
            set { domainName = value; }
        }

        private string userName;

        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        private string password;

        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        private bool connectToConsole;

        public bool ConnectToConsole
        {
            get { return connectToConsole; }
            set { connectToConsole = value; }
        }

        private DesktopSize desktopSize;

        public DesktopSize DesktopSize
        {
            get { return desktopSize; }
            set { desktopSize = value; }
        }

        private Colors colors;

        public Colors Colors
        {
            get { return colors; }
            set { colors = value; }
        }

        private bool showOnToolbar;

        public bool ShowOnToolbar
        {
            get { return showOnToolbar; }
            set { showOnToolbar = value; }
        }

        private bool hidden;

        public bool Hidden
        {
            get { return hidden; }
            set { hidden = value; }
        }
    }
}
