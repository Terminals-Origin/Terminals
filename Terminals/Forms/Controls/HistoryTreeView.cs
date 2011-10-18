using System;
using System.Linq;
using System.Windows.Forms;
using Terminals.Configuration;
using Terminals.History;

namespace Terminals.Forms.Controls
{
    internal partial class HistoryTreeView : TreeView
    {
        public HistoryTreeView()
        {
            InitializeComponent();

            // init groups before loading the history to prevent to run the callback earlier
            InitializeTimeLineTreeNodes();
            // consider remove next line to perform full lazy loading without loading the history data
            // directly after application start
            ConnectionHistory.Instance.LoadHistoryAsync();
            // dont apply OnHistoryLoaded event handler to do the lazy loading
            ConnectionHistory.Instance.OnHistoryRecorded += new HistoryRecorded(this.OnHistoryRecorded);
        }

        /// <summary>
        /// Gets the time stamp of last full refresh. This allowes to reload the tree nodes
        /// when Terminals longer than one day
        /// </summary>
        private DateTime lastFullRefresh = DateTime.Now;

        private void InitializeTimeLineTreeNodes()
        {
            this.SuspendLayout();
            // keep chronological order
            this.AddNewHistoryGroupNode(HistoryByFavorite.TODAY, "history_icon_today.png"); 
            this.AddNewHistoryGroupNode(HistoryByFavorite.YESTERDAY, "history_icon_yesterday.png"); 
            this.AddNewHistoryGroupNode(HistoryByFavorite.WEEK, "history_icon_week.png");
            this.AddNewHistoryGroupNode(HistoryByFavorite.TWOWEEKS, "history_icon_twoweeks.png"); 
            this.AddNewHistoryGroupNode(HistoryByFavorite.MONTH, "history_icon_month.png"); 
            this.AddNewHistoryGroupNode(HistoryByFavorite.OVERONEMONTH, "history_icon_overmonth.png"); 
            this.AddNewHistoryGroupNode(HistoryByFavorite.HALFYEAR, "history_icon_halfyear.png"); 
            this.AddNewHistoryGroupNode(HistoryByFavorite.YEAR, "history_icon_year.png"); 

            this.ResumeLayout();
        }

        private void AddNewHistoryGroupNode(string name, string imageKey)
        {
            var groupNode = new TagTreeNode(name, imageKey);
            this.Nodes.Add(groupNode);
        }

        /// <summary>
        /// add new history item in todays list and/or perform full refresh,
        /// if day has changed since last refresh
        /// </summary>
        private void OnHistoryRecorded(ConnectionHistory sender, HistoryRecordedEventArgs args)
        {
            if (IsDayGone())
                RefreshAllExpanded();
            else
                this.InsertRecordedNode(args);
        }

        private bool IsDayGone()
        {
            return this.lastFullRefresh.Date < DateTime.Now.Date;
        }

        private void RefreshAllExpanded()
        {
            var expandedNodes = this.Nodes.Cast<TagTreeNode>().Where(groupNode => !groupNode.NotLoadedYet);
            foreach (TagTreeNode groupNode in expandedNodes)
            {
                RefreshGroupNodes(groupNode);
            }

            this.lastFullRefresh = DateTime.Now;
        }

        private void InsertRecordedNode(HistoryRecordedEventArgs args)
        {
            TagTreeNode todayGroup = this.Nodes[HistoryByFavorite.TODAY] as TagTreeNode;
            if (todayGroup.NotLoadedYet)
                return;

            if (!todayGroup.ContainsFavoriteNode(args.ConnectionName))
                 InsertRecordedNode(todayGroup, args);
        }

        private static void InsertRecordedNode(TagTreeNode todayGroup, HistoryRecordedEventArgs args)
        {
            FavoriteConfigurationElement favorite = Settings.GetOneFavorite(args.ConnectionName);
            if (favorite != null) // shouldnt happen, because the favorite was actualy processed
            {
                int insertIndex = FavoriteTreeListLoader.FindFavoriteNodeInsertIndex(todayGroup.Nodes, favorite);
                var favoriteNode = new FavoriteTreeNode(favorite);
                todayGroup.Nodes.Insert(insertIndex, favoriteNode);
            }
        }

        private void OnTreeViewExpand(object sender, TreeViewEventArgs e)
        {
            TagTreeNode groupNode = e.Node as TagTreeNode;
            ExpandDateGroupNode(groupNode);
        }

        private void ExpandDateGroupNode(TagTreeNode groupNode)
        {
            this.Cursor = Cursors.WaitCursor;
            if (groupNode.NotLoadedYet)
            {
                RefreshGroupNodes(groupNode);
            }
            this.Cursor = Cursors.Default;
        }

        private static void RefreshGroupNodes(TagTreeNode groupNode)
        {
            groupNode.Nodes.Clear();
            var groupFavorites = ConnectionHistory.Instance.GetDateItems(groupNode.Name);
            CreateGroupNodes(groupNode, groupFavorites);
        }

        private static void CreateGroupNodes(TagTreeNode groupNode,
            SortableList<FavoriteConfigurationElement> groupFavorites)
        {
            foreach (FavoriteConfigurationElement favorite in groupFavorites)
            {
                var favoriteNode = new FavoriteTreeNode(favorite);
                groupNode.Nodes.Add(favoriteNode);
            }
        }
    }
}
