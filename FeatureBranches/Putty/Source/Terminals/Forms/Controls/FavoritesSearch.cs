using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Terminals.Data;

namespace Terminals.Forms.Controls
{
    /// <summary>
    /// Cancelable comparison of favorite to match search text
    /// </summary>
    internal class FavoritesSearch
    {
        private readonly CompareInfo invariantCompare = CultureInfo.InvariantCulture.CompareInfo;

        private CancellationToken Token { get; set; }

        private readonly string[] criteria;

        private readonly IFavorites favoritesSource;

        public FavoritesSearch(IFavorites favoritesSource, CancellationToken token, string searchText)
        {
            this.Token = token;
            this.favoritesSource = favoritesSource;
            this.criteria = this.ParseSearchText(searchText);
        }

        private string[] ParseSearchText(string searchText)
        {
            string[] words = searchText.Split(' ');
            return words.Where(word => !String.IsNullOrEmpty(word)).ToArray();
        }

        internal Task<List<IFavorite>> FindAsync()
        {
            return Task<List<IFavorite>>.Factory.StartNew(Find, this.Token);
        }

        private List<IFavorite> Find()
        {
            // already sorted, we dont have to sort the results once again
            SortableList<IFavorite> all = this.favoritesSource.ToListOrderedByDefaultSorting();
            return all.AsParallel()
                .WithCancellation(this.Token)
                .Where(Meet)
                .ToList();
        }

        private bool Meet(IFavorite favorite)
        {
            if (this.Contains(favorite.Protocol))
                return true;

            if (this.Contains(favorite.Name))
                return true;

            if (this.Contains(favorite.ServerName))
                return true;

            if (this.Contains(favorite.Port.ToString(CultureInfo.InvariantCulture)))
                return true;

            return this.Contains(favorite.Notes);
        }

        private bool Contains(string word)
        {
            if (string.IsNullOrEmpty(word))
                return false;

            this.Token.ThrowIfCancellationRequested();
            // this is recomended way how to do invariant culture string.Contains ignore case comparison.
            return this.criteria.Any(part => this.invariantCompare.IndexOf(word, part, CompareOptions.IgnoreCase) >= 0);
        }
    }
}