using System.Xml.Linq;

namespace Terminals.Integration.Import
{
    internal class RdcManRemoteDesktop : RdcManSettings<RdcManRemoteDesktop>
    {
        internal string Size
        {
            get
            {
                return this.Inherited ? this.Parent.Size : this.PropertiesElement.GetSize();
            }
        }

        internal int ColorDept
        {
            get
            {
                return this.Inherited ? this.Parent.ColorDept : this.PropertiesElement.GetColorDepth();
            }
        }

        internal bool FullScreen
        {
            get
            {
                return this.Inherited ? this.Parent.FullScreen : this.PropertiesElement.GetFullScreen();
            }
        }

        public RdcManRemoteDesktop(XElement settingsElement, RdcManRemoteDesktop parent = null)
            : base(settingsElement, parent)
        {         
        }
    }
}