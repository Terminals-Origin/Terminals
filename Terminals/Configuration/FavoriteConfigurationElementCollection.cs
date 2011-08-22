using System;
using System.Collections.Generic;
using System.Configuration;

namespace Terminals
{
    public class FavoriteConfigurationElementCollection : ConfigurationElementCollection
    {
        public FavoriteConfigurationElementCollection()
            : base(StringComparer.CurrentCultureIgnoreCase)
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
            return new FavoriteConfigurationElement();
        }


        protected override ConfigurationElement CreateNewElement(string elementName)
        {
            return new FavoriteConfigurationElement();
        }

        protected override Object GetElementKey(ConfigurationElement element)
        {
            return ((FavoriteConfigurationElement)element).Name;
        }

        public FavoriteConfigurationElement this[int index]
        {
            get
            {
                return (FavoriteConfigurationElement)BaseGet(index);
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

        new public FavoriteConfigurationElement this[string name]
        {
            get
            {
                return (FavoriteConfigurationElement)BaseGet(name);
            }
            set
            {
                if (BaseGet(name) != null)
                {
                    BaseRemove(name);
                }
                BaseAdd(value);
            }
        }

        public int IndexOf(FavoriteConfigurationElement item)
        {
            return BaseIndexOf(item);
        }

        public void Add(FavoriteConfigurationElement item)
        {
            BaseAdd(item);
        }

        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }

        public void Remove(FavoriteConfigurationElement item)
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

        internal List<FavoriteConfigurationElement> ToList()
        {
            List<FavoriteConfigurationElement> favorites = new List<FavoriteConfigurationElement>();
            foreach (FavoriteConfigurationElement favorite in this)
            {
               favorites.Add(favorite);
            }

            return favorites;
        }
    }

}
