using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace Terminals
{


    public class DockBarConfigurationElementCollection : ConfigurationElementCollection
    {
        public DockBarConfigurationElementCollection()
        {
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.AddRemoveClearMap;
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new DockBarConfigurationElement();
        }


        protected override ConfigurationElement CreateNewElement(string elementName)
        {
            return new DockBarConfigurationElement();
        }

        protected override Object GetElementKey(ConfigurationElement element)
        {
            return ((DockBarConfigurationElement)element).Name;
        }

        public new string AddElementName
        {
            get
            {
                return base.AddElementName;
            }
            set
            {
                base.AddElementName = value;
            }
        }

        public new string ClearElementName
        {
            get
            {
                return base.ClearElementName;
            }
            set
            {
                base.AddElementName = value;
            }
        }

        public new string RemoveElementName
        {
            get
            {
                return base.RemoveElementName;
            }
        }

        public new int Count
        {
            get
            {
                return base.Count;
            }
        }

        public DockBarConfigurationElement this[int index]
        {
            get
            {
                return (DockBarConfigurationElement)BaseGet(index);
            }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        new public DockBarConfigurationElement this[string Name]
        {
            get
            {
                return (DockBarConfigurationElement)BaseGet(Name);
            }
        }

        public int IndexOf(DockBarConfigurationElement item)
        {
            return BaseIndexOf(item);
        }

        public DockBarConfigurationElement ItemByName(string name)
        {
            return (DockBarConfigurationElement)BaseGet(name);
        }

        public void Add(DockBarConfigurationElement item)
        {
            BaseAdd(item);
        }

        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }

        public void Remove(MRUItemConfigurationElement item)
        {
            if (BaseIndexOf(item) >= 0)
                BaseRemove(item.Name);
        }

        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }

        public void Remove(string name)
        {
            BaseRemove(name);
        }

        public void Clear()
        {
            BaseClear();
        }
    }

}
