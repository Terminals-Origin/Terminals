using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Terminals.Data;

namespace Terminals.Forms.Controls
{
    internal partial class SearchResultsPanel : UserControl
    {
        private IPersistence persistence;

        /// <summary>
        /// Gets or sets the context menu, which will be shown, when selected item is clicked.
        /// Needs to be public get and set to be available in designer.
        /// </summary>
        public ContextMenuStrip ResultsContextMenu { get; set; }

        /// <summary>
        /// Gets or sets the value indicating, the the rename of items in results panel is allowed.
        /// This only wrappes related property in wrapped listview control.
        /// </summary>
        public bool LabelEdit
        {
            get { return this.resultsListView.LabelEdit; }
            set { this.resultsListView.LabelEdit = value; }
        }

        /// <summary>
        /// Gets first selected favorite in results list view. Null if nothing is selected.
        /// </summary>
        internal IFavorite SelectedFavorite
        {
            get { return this.SelectedFavorites.FirstOrDefault(); }
        }

        internal List<IFavorite> SelectedFavorites
        {
            get
            {
                return this.resultsListView.SelectedItems.Cast<ListViewItem>()
                    .Select(item => item.Tag as IFavorite)
                    .ToList();
            }
        }

        /// <summary>
        /// Ocures, when the label of an item in results listview is edited by the user.
        /// This only forwards the event to the wrapped listview control.
        /// </summary>
        public event LabelEditEventHandler ResultListAfterLabelEdit
        {
            add { this.resultsListView.AfterLabelEdit += value; }
            remove { this.resultsListView.AfterLabelEdit -= value; }
        }

        /// <summary>
        /// Ocures, when result list id double clicked. This only forwards the event to the wrapped control.
        /// </summary>
        public event EventHandler ResultListDoubleClick
        {
            add { this.resultsListView.DoubleClick += value; }
            remove { this.resultsListView.DoubleClick -= value; }
        }

        /// <summary>
        /// Ocures, when a key is released and while the results list has a Focus.
        /// This only forwards the event to the wrapped control.
        /// </summary>
        public event KeyEventHandler ResultListKeyUp
        {
            add { this.resultsListView.KeyUp += value; }
            remove { this.resultsListView.KeyUp -= value; }
        }

        /// <summary>
        /// Ocures, when selection state of an item in results list changes.
        /// This only forwards the event to the wrapped control.
        /// </summary>
        public event ListViewItemSelectionChangedEventHandler ResultsSelectionChanged
        {
            add { this.resultsListView.ItemSelectionChanged += value; }
            remove { this.resultsListView.ItemSelectionChanged -= value; }
        }

        public SearchResultsPanel()
        {
            InitializeComponent();
        }

        internal void LoadEvents(IPersistence persistence)
        {
            this.persistence = persistence;
            this.LoadAll();
        }
       
        private static ListViewItem FavoriteToListViewItem(IFavorite favorite)
        {
            var item = new ListViewItem();
            item.Tag = favorite;
            item.Text = favorite.Name;
            item.ToolTipText = favorite.GetToolTipText();
            item.ImageKey = FavoriteIcons.GetTreeviewImageListKey(favorite.Protocol);
            return item;
        }

        private void ResultsListView_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
                return;

            this.resultsListView.SelectedItems.Clear();
            this.ShowContextMenu(e);
        }

        private void ShowContextMenu(MouseEventArgs e)
        {
            ListViewItem clickedItem = this.resultsListView.GetItemAt(e.X, e.Y);

            if (this.ResultsContextMenu == null || clickedItem == null)
                return;

            clickedItem.Selected = true;
            this.ResultsContextMenu.Show(this.resultsListView, new Point(e.X, e.Y));
        }

        /// <summary>
        /// Simulation of all favorites loaded by default
        /// </summary>
        internal void LoadAll()
        {
            SortableList<IFavorite> favorites = this.persistence.Favorites.ToListOrderedByDefaultSorting();
            this.LoadFromFavorites(favorites);
        }

        internal void LoadFromFavorites(List<IFavorite> favorites)
        {
            this.resultsListView.Items.Clear();
            ListViewItem[] transformed = favorites.Select(FavoriteToListViewItem).ToArray();
            this.resultsListView.Items.AddRange(transformed);
            this.resultsListView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
        }

        internal void BeginRename()
        {
            var firstSelected = this.resultsListView.SelectedItems
                .OfType<ListViewItem>()
                .FirstOrDefault();

            if (firstSelected != null)
                firstSelected.BeginEdit();
        }
    }
}
