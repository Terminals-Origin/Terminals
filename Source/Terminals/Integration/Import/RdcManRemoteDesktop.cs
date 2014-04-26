using System;
using System.Xml.Linq;

namespace Terminals.Integration.Import
{
    internal class RdcManRemoteDesktop : RdcManSettings<RdcManRemoteDesktop>
    {
        internal string Size
        {
            get
            {
                Func<string> getParentValue = () => this.Parent.Size;
                Func<string> getElementValue = this.PropertiesElement.GetSize;
                return this.ResolveValue(getParentValue, getElementValue, "1024 x 768");
            }
        }

        internal int ColorDept
        {
            get
            {
                Func<int> getParentValue = () => this.Parent.ColorDept;
                Func<int> getElementValue = this.PropertiesElement.GetColorDepth;
                return this.ResolveValue(getParentValue, getElementValue, 16);
            }
        }

        internal bool FullScreen
        {
            get
            {
                Func<bool> getParentValue = () => this.Parent.FullScreen;
                Func<bool> getElementValue = this.PropertiesElement.GetFullScreen;
                return this.ResolveValue(getParentValue, getElementValue);
            }
        }

        internal bool SameSizeAsClientArea
        {
            get
            {
                Func<bool> getParentValue = () => this.Parent.SameSizeAsClientArea;
                Func<bool> getElementValue = this.PropertiesElement.GetSameSizeAsClientArea;
                return this.ResolveValue(getParentValue, getElementValue);
            }
        }

        public RdcManRemoteDesktop(XElement settingsElement, RdcManRemoteDesktop parent = null)
            : base(settingsElement, parent)
        {         
        }
    }
}