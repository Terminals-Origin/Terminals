using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Forms;

namespace Terminals
{
    /// <summary>
    /// Extened generic list, which allows to sort by property name
    /// </summary>
    internal class SortableList<ItemType>: List<ItemType>
    {
        internal SortableList() { }
        internal SortableList(IEnumerable<ItemType> collection) : base(collection) { }

        internal SortableList<ItemType> SortByProperty(string propertyName, SortOrder direction)
        {
            if (direction == SortOrder.None)
                return this;

            var param = Expression.Parameter(typeof(ItemType), "item");
            var mySortExpression = Expression
                .Lambda<Func<ItemType, object>>(Expression.Property(param, propertyName), param);

            if (direction == SortOrder.Ascending)
            {
                return new SortableList<ItemType>(this.AsQueryable().OrderBy(mySortExpression));
            }

            return new SortableList<ItemType>(this.AsQueryable().OrderByDescending(mySortExpression));
        }
    }
}
