using System.Xml.Linq;

namespace Terminals.Integration.Import
{
    internal class LocalResources : RdcManSettings<LocalResources>
    {
        internal bool AudioRedirect
        {
            get
            {
                return this.Inherited ? this.Parent.AudioRedirect : this.PropertiesElement.GetAudioRedirection();
            }
        }

        internal int KeyboardHook
        {
            get { return this.Inherited ? this.Parent.KeyboardHook : this.PropertiesElement.GetKeyboardHook(); }
        }

        internal bool RedirectClipboard
        {
            get { return this.Inherited ? this.Parent.RedirectClipboard : this.PropertiesElement.GetRedirectClipboard(); }
        }

        internal bool RedirectDrives
        {
            get { return this.Inherited ? this.Parent.RedirectDrives : this.PropertiesElement.GetRedirectDrives(); }
        }

        internal bool RedirectPorts
        {
            get { return this.Inherited ? this.Parent.RedirectPorts : this.PropertiesElement.GetRedirectPorts(); }
        }

        internal bool RedirectPrinters
        {
            get { return this.Inherited ? this.Parent.RedirectPrinters : this.PropertiesElement.GetRedirectPrinters(); }
        }

        internal bool RedirectSmartCards
        {
            get { return this.Inherited ? this.Parent.RedirectSmartCards : this.PropertiesElement.GetRedirectSmartCards(); }
        }

        public LocalResources(XElement settingsElement, LocalResources parent = null)
            : base(settingsElement, parent)
        {
        }
    }
}