using System;
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

        internal bool Inherited
        {
            get
            {
                return this.PropertiesElement.Inherits();
            }
        }

        protected RdcManSettings(XElement element, TSettings parent)
        {
            this.PropertiesElement = element;
            this.Parent = parent;
        }

        protected TReturnValue ResolveValue<TReturnValue>(Func<TReturnValue> getParentValue,
            Func<TReturnValue> getElementValue, TReturnValue defalutValue = default(TReturnValue))
        {
            if (this.Inherited)
                return this.GetParentOrDefault(getParentValue, defalutValue);

            return getElementValue();
        }

        private TReturnValue GetParentOrDefault<TReturnValue>(Func<TReturnValue> getParentValue,
            TReturnValue defaultValue)
        {
            return this.HasParent ? getParentValue() : defaultValue;
        }
    }
}