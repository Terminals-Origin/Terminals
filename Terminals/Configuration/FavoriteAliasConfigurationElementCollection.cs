using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace Terminals
{

    public class FavoriteAliasConfigurationElementCollection : ConfigurationElementCollection
    {
        public FavoriteAliasConfigurationElementCollection()
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
            return new FavoriteAliasConfigurationElement();
        }


        protected override ConfigurationElement CreateNewElement(string elementName)
        {
            return new FavoriteAliasConfigurationElement();
        }

        protected override Object GetElementKey(ConfigurationElement element)
        {
            return ((FavoriteAliasConfigurationElement)element).Name;
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

        public FavoriteAliasConfigurationElement this[int index]
        {
            get
            {
                return (FavoriteAliasConfigurationElement)BaseGet(index);
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

        new public FavoriteAliasConfigurationElement this[string Name]
        {
            get
            {
                return (FavoriteAliasConfigurationElement)BaseGet(Name);
            }
        }

        public int IndexOf(FavoriteAliasConfigurationElement item)
        {
            return BaseIndexOf(item);
        }

        public FavoriteAliasConfigurationElement ItemByName(string name)
        {
            return (FavoriteAliasConfigurationElement)BaseGet(name);
        }

        public void Add(FavoriteAliasConfigurationElement item)
        {
            BaseAdd(item);
        }

        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }

        public void Remove(FavoriteAliasConfigurationElement item)
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
