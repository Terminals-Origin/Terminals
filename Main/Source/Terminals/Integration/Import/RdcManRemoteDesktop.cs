using System.Xml.Linq;

namespace Terminals.Integration.Import
{
    internal class RdcManRemoteDesktop : RdcManSettings<RdcManRemoteDesktop>
    {
        internal string Size
        {
            get
            {
                return this.PropertiesElement.GetSize();
            }
        }

        internal int ColorDept
        {
            get
            {
                return this.PropertiesElement.GetColorDepth();
            }
        }

        internal bool FullScreen
        {
            get
            {
                return this.PropertiesElement.GetFullScreen();
            }
        }

        public RdcManRemoteDesktop(XElement settingsElement, RdcManRemoteDesktop parent = null)
            : base(settingsElement, parent)
        {         
        }
    }
}