using System.Xml.Linq;

namespace Terminals.Integration.Import
{
    internal abstract class RdcManSettings<TSettings> where TSettings : class
    {
        protected TSettings Parent { get; private set; }

        protected XElement PropertiesElement { get; private set; }

        protected bool HasParent
        {
            get { return this.Parent != null; }
        }

        protected RdcManSettings(XElement element, TSettings parent)
        {
            this.PropertiesElement = element;
            this.Parent = parent;
        }
    }
}