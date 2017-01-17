using System;
using System.Linq;
using System.Windows.Forms;
using Terminals.Data;
using Terminals.History;

namespace Terminals.Forms.Controls
{
    internal partial class HistoryTreeView : TreeView
    {
        private IPersistence persistence;

        /// <summary>
        /// Gets the time stamp of last full refresh. This allowes to reload the tree nodes
        /// when Terminals longer than one day
        /// </summary>
        private DateTime lastFullRefresh = DateTime.Now;

        private ToolTipBuilder toolTipBuilder;

        private FavoriteIcons favoriteIcons;

        public HistoryTreeView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Dont call from constructor to support designer
        /// </summary>
        internal void Load(IPersistence persistence, FavoriteIcons favoriteIcons)
        {
            this.persistence = persistence;
            this.favoriteIcons = favoriteIcons;
            this.toolTipBuilder = new ToolTipBuilder(this.persistence.Security);
            var connectionHistory = this.persistence.ConnectionHistory;
            connectionHistory.HistoryRecorded += new HistoryRecorded(this.HistoryRecorded);
            connectionHistory.HistoryClear += new Action(this.ConnectionHistory_HistoryClear);

            var iconsBuilder = new ProtocolImageListBuilder(favoriteIcons.GetProtocolIcons);
            iconsBuilder.Build(this.imageListIcons);

            // init groups before loading the history to prevent to run the callback earlier
            InitializeTimeLineTreeNodes();
        }

        private void ConnectionHistory_HistoryClear()
        {
            RefreshAllExpanded();
        }

        private void InitializeTimeLineTreeNodes()
        {
            this.SuspendLayout();
            // keep chronological order
            this.AddNewHistoryGroupNode(HistoryIntervals.TODAY, "history_icon_today.png");
            this.AddNewHistoryGroupNode(HistoryIntervals.YESTERDAY, "history_icon_yesterday.png");
            this.AddNewHistoryGroupNode(HistoryIntervals.WEEK, "history_icon_week.png");
            this.AddNewHistoryGroupNode(HistoryIntervals.TWOWEEKS, "history_icon_twoweeks.png");
            this.AddNewHistoryGroupNode(HistoryIntervals.MONTH, "history_icon_month.png");
            this.AddNewHistoryGroupNode(HistoryIntervals.OVERONEMONTH, "history_icon_overmonth.png");
            this.AddNewHistoryGroupNode(HistoryIntervals.HALFYEAR, "history_icon_halfyear.png");
            this.AddNewHistoryGroupNode(HistoryIntervals.YEAR, "history_icon_year.png");

            this.ResumeLayout();
        }

        private void AddNewHistoryGroupNode(string name, string imageKey)
        {
            IGroup virtualGroup = this.persistence.Factory.CreateGroup(name);
            var groupNode = new GroupTreeNode(virtualGroup, imageKey);
            this.Nodes.Add(groupNode);
        }

        /// <summary>
        /// add new history item in todays list and/or perform full refresh,
        /// if day has changed since last refresh
        /// </summary>
        private void HistoryRecorded(HistoryRecordedEventArgs args)
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
            GroupTreeNode todayGroup = this.Nodes[HistoryIntervals.TODAY] as GroupTreeNode;
            if (todayGroup.NotLoadedYet)
                return;

            if (!todayGroup.ContainsFavoriteNode(args.Favorite))
                InsertRecordedNode(todayGroup, args);
        }

        private void InsertRecordedNode(GroupTreeNode todayGroup, HistoryRecordedEventArgs args)
        {
            IFavorite favorite = args.Favorite;
            if (favorite != null) // shouldnt happen, because the favorite was actualy processed
            {
                var nodes = new TreeListNodes(todayGroup.Nodes, this.toolTipBuilder, this.favoriteIcons);
                nodes.InsertFavorite(favorite);
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

        private void RefreshGroupNodes(GroupTreeNode groupNode)
        {
            groupNode.Nodes.Clear();
            var groupFavorites = this.persistence.ConnectionHistory.GetDateItems(groupNode.Name);
            CreateGroupNodes(groupNode, groupFavorites);
        }

        private void CreateGroupNodes(GroupTreeNode groupNode,
            SortableList<IFavorite> groupFavorites)
        {
            foreach (IFavorite favorite in groupFavorites)
            {
                string toolTip = this.toolTipBuilder.BuildTooTip(favorite);
                var favoriteNode = new FavoriteTreeNode(this.favoriteIcons, favorite, toolTip);
                groupNode.Nodes.Add(favoriteNode);
            }
        }
    }
}
