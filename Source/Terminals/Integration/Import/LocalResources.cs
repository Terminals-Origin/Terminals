using System.Xml.Linq;

namespace Terminals.Integration.Import
{
    internal class LocalResources : RdcManSettings<LocalResources>
    {
        internal bool AudioRedirect
        {
            get { return this.PropertiesElement.GetAudioRedirection(); }
        }

        internal int KeyboardHook
        {
            get { return this.PropertiesElement.GetKeyboardHook(); }
        }

        internal bool RedirectClipboard
        {
            get { return this.PropertiesElement.GetRedirectClipboard(); }
        }

        internal bool RedirectDrives
        {
            get { return this.PropertiesElement.GetRedirectDrives(); }
        }

        internal bool RedirectPorts
        {
            get { return this.PropertiesElement.GetRedirectPorts(); }
        }

        internal bool RedirectPrinters
        {
            get { return this.PropertiesElement.GetRedirectPrinters(); }
        }

        internal bool RedirectSmartCards
        {
            get { return this.PropertiesElement.GetRedirectSmartCards(); }
        }

        public LocalResources(XElement settingsElement, LocalResources parent = null)
            : base(settingsElement, parent)
        {
        }
    }
}