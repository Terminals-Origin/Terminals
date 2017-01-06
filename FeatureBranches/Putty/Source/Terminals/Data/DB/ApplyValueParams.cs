using System;
using System.Collections.Generic;

namespace Terminals.Data.DB
{
    /// <summary>
    /// Method params container to apply values on collection of favorites
    /// </summary>
    internal class ApplyValueParams
    {
        internal ApplyValueParams(Action<List<IFavorite>, string> action, List<IFavorite> selectedFavorites,
                                  string valueToApply, string propertyName)
        {
            this.Action = action;
            this.Favorites = selectedFavorites;
            this.ValueToApply = valueToApply;
            this.PropertyName = propertyName;
        }

        /// <summary>
        /// Gets or sets a method to proceed
        /// </summary>
        internal Action<List<IFavorite>, string> Action { get; private set; }

        /// <summary>
        /// Gets or sets collection of favorites to update
        /// </summary>
        internal List<IFavorite> Favorites { get; private set; }

        /// <summary>
        /// Gets or sets a value to be applied to all favorites in SelectedFavorites
        /// </summary>
        internal string ValueToApply { get; private set; }

        /// <summary>
        /// Gets or sets a property name to log i a case of error
        /// </summary>
        internal string PropertyName { get; private set; }
    }
}