using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Terminals.Data.DB
{
    /// <summary>
    /// Represents caching of entities based on their integer unique identifier
    /// </summary>
    internal class EntitiesCache<TEntity> : IEnumerable<TEntity>
        where TEntity : class, IIntegerKeyEnityty
    {
        private readonly Dictionary<int, TEntity> cache = new Dictionary<int, TEntity>();

        internal bool IsEmpty
        {
            get { return this.cache.Count == 0; }
        }

        internal TEntity this[int id]
        {
            get
            {
                if (this.cache.ContainsKey(id))
                    return this.cache[id];

                return default(TEntity);
            }
        }

        internal bool Add(TEntity toAdd)
        {
            if (toAdd == null || this.cache.ContainsKey(toAdd.Id))
                return false;

            this.cache.Add(toAdd.Id, toAdd);
            return true;
        }

        internal void Add(List<TEntity> toAdd)
        {
            IEnumerable<TEntity> notInCache = toAdd.Where(item => !this.cache.ContainsKey(item.Id));
            foreach (TEntity item in notInCache)
            {
                this.cache.Add(item.Id, item);
            }
        }

        internal bool Delete(TEntity toDelete)
        {
            if (toDelete == null || !this.cache.ContainsKey(toDelete.Id))
                return false;

            this.cache.Remove(toDelete.Id);
            return true;
        }

        internal void Delete(List<TEntity> toDelete)
        {
            IEnumerable<TEntity> inCache = toDelete.Where(item => this.cache.ContainsKey(item.Id));
            foreach (TEntity item in inCache)
            {
                this.cache.Remove(item.Id);
            }
        }

        internal void Clear()
        {
            this.cache.Clear();
        }

        internal void Update(List<TEntity> toUpdate)
        {
            foreach (TEntity item in toUpdate)
            {
                this.Update(item);
            }
        }

        internal void Update(TEntity item)
        {
            if (this.cache.ContainsKey(item.Id))
                this.cache[item.Id] = item;
            else
                this.cache.Add(item.Id, item);
        }

        #region IEnumerable members

        public IEnumerator<TEntity> GetEnumerator()
        {
            return this.cache.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion

        public override string ToString()
        {
            return string.Format("EntitiesCache:Count={0}", this.cache.Count);
        }
    }
}
