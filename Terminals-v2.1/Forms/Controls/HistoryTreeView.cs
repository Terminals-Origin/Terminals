using System;
using System.Linq;
using System.Windows.Forms;
using Terminals.Data;
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
            var connectionHistory = Persistance.Instance.ConnectionHistory;
            connectionHistory.LoadHistoryAsync();
            // dont apply OnHistoryLoaded event handler to do the lazy loading
            connectionHistory.OnHistoryRecorded += new HistoryRecorded(this.OnHistoryRecorded);
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
            IGroup virtualGroup = Persistance.Instance.Factory.CreateGroup(name);
            var groupNode = new GroupTreeNode(virtualGroup, imageKey);
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
            var expandedNodes = this.Nodes.Cast<GroupTreeNode>().Where(groupNode => !groupNode.NotLoadedYet);
            foreach (GroupTreeNode groupNode in expandedNodes)
            {
                RefreshGroupNodes(groupNode);
            }

            this.lastFullRefresh = DateTime.Now;
        }

        private void InsertRecordedNode(HistoryRecordedEventArgs args)
        {
            GroupTreeNode todayGroup = this.Nodes[HistoryByFavorite.TODAY] as GroupTreeNode;
            if (todayGroup.NotLoadedYet)
                return;

            if (!todayGroup.ContainsFavoriteNode(args.ConnectionName))
                 InsertRecordedNode(todayGroup, args);
        }

        private static void InsertRecordedNode(GroupTreeNode todayGroup, HistoryRecordedEventArgs args)
        {
            IFavorite favorite = Persistance.Instance.Favorites[args.ConnectionName];
            if (favorite != null) // shouldnt happen, because the favorite was actualy processed
            {
                int insertIndex = FavoriteTreeListLoader.FindFavoriteNodeInsertIndex(todayGroup.Nodes, favorite);
                var favoriteNode = new FavoriteTreeNode(favorite);
                todayGroup.Nodes.Insert(insertIndex, favoriteNode);
            }
        }

        private void OnTreeViewExpand(object sender, TreeViewEventArgs e)
        {
            GroupTreeNode groupNode = e.Node as GroupTreeNode;
            ExpandDateGroupNode(groupNode);
        }

        private void ExpandDateGroupNode(GroupTreeNode groupNode)
        {
            this.Cursor = Cursors.WaitCursor;
            if (groupNode.NotLoadedYet)
            {
                RefreshGroupNodes(groupNode);
            }
            this.Cursor = Cursors.Default;
        }

        private static void RefreshGroupNodes(GroupTreeNode groupNode)
        {
            groupNode.Nodes.Clear();
            var groupFavorites = Persistance.Instance.ConnectionHistory.GetDateItems(groupNode.Name);
            CreateGroupNodes(groupNode, groupFavorites);
        }

        private static void CreateGroupNodes(GroupTreeNode groupNode,
            SortableList<IFavorite> groupFavorites)
        {
            foreach (IFavorite favorite in groupFavorites)
            {
                var favoriteNode = new FavoriteTreeNode(favorite);
                groupNode.Nodes.Add(favoriteNode);
            }
        }
    }
}
