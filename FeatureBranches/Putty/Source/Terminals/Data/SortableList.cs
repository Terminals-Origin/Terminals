using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Forms;

namespace Terminals
{
    /// <summary>
    /// Extended generic list, which allows to sort by property name
    /// </summary>
    internal class SortableList<TItemType>: List<TItemType>
    {
        internal SortableList() { }
        internal SortableList(IEnumerable<TItemType> collection) : base(collection) { }

        internal SortableList<TItemType> SortByProperty(string propertyName, SortOrder direction)
        {
            if (direction == SortOrder.None)
                return this;

            var param = Expression.Parameter(typeof(TItemType), "item");
            var mySortExpression = Expression
                .Lambda<Func<TItemType, object>>(Expression.Property(param, propertyName), param);

            if (direction == SortOrder.Ascending)
            {
                return new SortableList<TItemType>(this.AsQueryable().OrderBy(mySortExpression));
            }

            return new SortableList<TItemType>(this.AsQueryable().OrderByDescending(mySortExpression));
        }
    }
}
