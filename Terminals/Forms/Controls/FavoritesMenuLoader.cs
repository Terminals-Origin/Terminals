using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Terminals.Configuration;
using Terminals.Data;
using Terminals.Properties;
using Settings = Terminals.Configuration.Settings;

namespace Terminals.Forms.Controls
{
    /// <summary>
    /// Fills menu, tool strip menu and tool bar with favorite buttons
    /// </summary>
    internal class FavoritesMenuLoader
    {
        private ToolStripMenuItem favoritesToolStripMenuItem;
        private TagMenuItem untaggedToolStripMenuItem;
        private ToolStripComboBox tscConnectTo;
        private EventHandler serverToolStripMenuItem_Click;
        private ToolStrip favoriteToolBar;
        private ContextMenuStrip quickContextMenu;
        private ToolStripItemClickedEventHandler quickContextMenu_ItemClicked;

        internal const String COMMAND_EXIT = "Exit";
        internal const String COMMAND_SPECIAL = "SpecialCommands";
        internal const String COMMAND_RESTORESCREEN = "RestoreScreen";
        internal const String COMMAND_FULLSCREEN = "FullScreen";
        internal const String COMMAND_SHOWMENU = "ShowMenu";
        internal const String COMMAND_OPTIONS = "Options";
        internal const String COMMAND_CAPTUREMANAGER = "ScreenCaptureManager";
        internal const String COMMAND_NETTOOLS = "NetworkingTools";
        internal const String COMMAND_CREDENTIALMANAGER = "CredentialsManager";
        internal const String COMMAND_ORGANIZEFAVORITES = "OrganizeFavorites";
        private const String COMMAND_MANAGEMENT = "Management";
        private const String COMMAND_CONTROLPANEL = "ControlPanel";
        private const String COMMAND_OTHER = "Other";
        private const String COMMAND_ALPHABETICAL = "Alphabetical";

        private ToolStripMenuItem special;
        private ToolStripMenuItem mgmt;
        private ToolStripMenuItem cpl;
        private ToolStripMenuItem other;
        private TagMenuItem unTaggedQuickMenuItem;
        private ToolStripMenuItem alphabeticalMenu;
        private ToolStripItem restoreScreenMenuItem;
        private ToolStripItem fullScreenMenuItem;

        /// <summary>
        /// Stored in context menu Tag to identify virtual context menu groups by tag
        /// </summary>
        internal const String TAG = "tag";

        /// <summary>
        /// Stored in context menu Tag to identify favorite context menu items
        /// </summary>
        internal const String FAVORITE = "favorite";

        private int AlphabeticalMenuItemIndex
        {
            get { return this.quickContextMenu.Items.IndexOf(this.alphabeticalMenu); }
        }

        internal FavoritesMenuLoader(ToolStripMenuItem favoritesToolStripMenuItem,
            ToolStripComboBox tscConnectTo, EventHandler serverToolStripMenuItem_Click, ToolStrip favoriteToolBar,
            ContextMenuStrip quickContextMenu, ToolStripItemClickedEventHandler quickContextMenu_ItemClicked)
        {
            this.favoritesToolStripMenuItem = favoritesToolStripMenuItem;
            this.tscConnectTo = tscConnectTo;
            this.serverToolStripMenuItem_Click = serverToolStripMenuItem_Click;
            this.favoriteToolBar = favoriteToolBar;
            this.quickContextMenu = quickContextMenu;
            this.quickContextMenu_ItemClicked = quickContextMenu_ItemClicked;

            this.favoritesToolStripMenuItem.DropDownItems.Add("-");
            CreateUntaggedItem();
            CreateTrayMenuItems();
            UpdateMenuAndContextMenu();

            DataDispatcher.Instance.TagsChanged += new TagsChangedEventHandler(this.OnDataChanged);
            DataDispatcher.Instance.FavoritesChanged += new FavoritesChangedEventHandler(this.OnDataChanged);
            Settings.ConfigurationChanged += new ConfigurationChangedHandler(this.OnSettingsConfigurationChanged);
        }

        private void OnDataChanged(EventArgs args)
        {
            UpdateMenuAndContextMenu();
        }
        
        private void OnSettingsConfigurationChanged(ConfigurationChangedEventArgs args)
        {
            LoadFavoritesToolbar();
        }

        /// <summary>
        /// Simple refresh, which removes all menu items and recreates new, when necessary using lazy loading.
        /// </summary>
        private void UpdateMenuAndContextMenu()
        {
            this.FillMainMenu();
            this.FillTrayContextMenu();
        }

        private void CreateUntaggedItem()
        {
            this.untaggedToolStripMenuItem = CreateTagMenuItem(Settings.UNTAGGED_NODENAME);
            this.untaggedToolStripMenuItem.DropDownOpening += new EventHandler(this.OnTagMenuDropDownOpening);
            this.favoritesToolStripMenuItem.DropDownItems.Add(this.untaggedToolStripMenuItem);
        }

        /// <summary>
        /// Creates skeleton for system tray menu items. No tags or favorites are added here.
        /// </summary>
        private void CreateTrayMenuItems()
        {
            this.AddGeneralTrayContextMenu();
            this.AddTraySpecialCommandsContextMenu();
            this.quickContextMenu.Items.Add("-");
            AddUntaggedQuickContextMenu();

            // here favorite Tags will be placed

            this.AddAlphabeticalContextMenu();
            this.quickContextMenu.Items.Add("-");
            ToolStripItem exitMenu = this.quickContextMenu.Items.Add(Program.Resources.GetString(COMMAND_EXIT));
            exitMenu.Name = COMMAND_EXIT;
        }

        private void AddUntaggedQuickContextMenu()
        {
            this.unTaggedQuickMenuItem = CreateTagMenuItem(Settings.UNTAGGED_NODENAME);
            unTaggedQuickMenuItem.DropDownItemClicked += new ToolStripItemClickedEventHandler(this.quickContextMenu_ItemClicked);
            unTaggedQuickMenuItem.DropDownOpening += new EventHandler(OnTagTrayMenuItemDropDownOpening);
            this.quickContextMenu.Items.Add(unTaggedQuickMenuItem);
        }

        private void FillMainMenu()
        {
            ReFreshConnectionsComboBox();
            this.untaggedToolStripMenuItem.ClearDropDownsToEmpty();
            this.ClearFavoritesToolStripmenuItems();
            this.CreateTagsToolStripMenuItems();
            this.LoadFavoritesToolbar();
        }

        private void ReFreshConnectionsComboBox()
        {
            this.tscConnectTo.Items.Clear();
            String[] connectionNames = Settings.GetFavorites()
                .ToList()
                .Select(favorite => favorite.Name).ToArray();
            this.tscConnectTo.Items.AddRange(connectionNames);
        }

        #region Menu toolstrips

        private void ClearFavoritesToolStripmenuItems()
        {
            Int32 seperatorIndex = this.favoritesToolStripMenuItem.DropDownItems.IndexOf(this.untaggedToolStripMenuItem);
            for (Int32 index = this.favoritesToolStripMenuItem.DropDownItems.Count - 1; index > seperatorIndex; index--)
            {
                ToolStripMenuItem tagMenuItem = this.favoritesToolStripMenuItem.DropDownItems[index] as ToolStripMenuItem;
                UnregisterTagMenuItemEventHandlers(tagMenuItem);
                this.favoritesToolStripMenuItem.DropDownItems.RemoveAt(index);
            }
        }

        private void UnregisterTagMenuItemEventHandlers(ToolStripMenuItem tagMenuItem)
        {
            tagMenuItem.DropDownOpening -= OnTagMenuDropDownOpening;
            foreach (ToolStripMenuItem favoriteItem in tagMenuItem.DropDownItems)
            {
                favoriteItem.Click += serverToolStripMenuItem_Click;
            }
        }

        /// <summary>
        /// Fills the main window "favorites" menu, after separator places all tags
        /// and their favorites as dropdown items
        /// </summary>
        private void CreateTagsToolStripMenuItems()
        {
            foreach (String tag in Settings.Tags)
            {
                ToolStripMenuItem tagMenu = CreateTagMenuItem(tag);
                tagMenu.DropDownOpening += new EventHandler(this.OnTagMenuDropDownOpening);
                this.favoritesToolStripMenuItem.DropDownItems.Add(tagMenu);
            }
        }

        /// <summary>
        /// Lazy loading for favorites dropdown menu items in Tag menu item
        /// </summary>
        private void OnTagMenuDropDownOpening(object sender, EventArgs e)
        {
            TagMenuItem tagMenu = sender as TagMenuItem;
            if (tagMenu.IsEmpty)
            {
                tagMenu.DropDown.Items.Clear();
                List<FavoriteConfigurationElement> tagFavorites = Settings.GetSortedFavoritesByTag(tagMenu.Text);
                foreach (FavoriteConfigurationElement favorite in tagFavorites)
                {
                    ToolStripMenuItem item = this.CreateToolStripItemByFavorite(favorite);
                    tagMenu.DropDown.Items.Add(item);
                }
            }
        }

        private ToolStripMenuItem CreateToolStripItemByFavorite(FavoriteConfigurationElement favorite)
        {
            ToolStripMenuItem item = CreateFavoriteMenuItem(favorite);
            item.Click += this.serverToolStripMenuItem_Click;
            return item;
        }

        #endregion

        #region Fill tool bar by user defined items

        private void LoadFavoritesToolbar()
        {
            try
            {
                favoriteToolBar.SuspendLayout();
                favoriteToolBar.Items.Clear();
                CreateFavoriteButtons();
                favoriteToolBar.ResumeLayout();
            }
            catch (Exception exc)
            {
                Logging.Log.Error("Error Loading Favorites Toolbar", exc);
            }
        }

        private void CreateFavoriteButtons()
        {
            FavoriteConfigurationElementCollection favorites = Settings.GetFavorites();
            foreach (String favoriteName in Settings.FavoritesToolbarButtons)
            {
                this.CreateFavoriteButton(favorites, favoriteName);
            }
        }

        private void CreateFavoriteButton(FavoriteConfigurationElementCollection favorites, string favoriteName)
        {
            FavoriteConfigurationElement favorite = favorites[favoriteName];
            if (favorite != null)
            {
                ToolStripButton favoriteBtn = this.CreateFavoriteButton(favorite);
                this.favoriteToolBar.Items.Add(favoriteBtn);
            }
        }

        private ToolStripButton CreateFavoriteButton(FavoriteConfigurationElement favorite)
        {
            Image buttonImage = FavoriteIcons.GetFavoriteIcon(favorite);
            ToolStripButton favoriteBtn = new ToolStripButton(favorite.Name, buttonImage, this.serverToolStripMenuItem_Click);
            favoriteBtn.ToolTipText = favorite.GetToolTipText();
            favoriteBtn.Tag = favorite;
            favoriteBtn.Overflow = ToolStripItemOverflow.AsNeeded;
            return favoriteBtn;
        }

        #endregion

        #region System tray context menu

        private void FillTrayContextMenu()
        {
            this.unTaggedQuickMenuItem.ClearDropDownsToEmpty();
            this.ClearTrayFavoritesMenu();
            this.AddTagTrayMenuItems();
            this.alphabeticalMenu.DropDownItems.Clear();
        }

        private void ClearTrayFavoritesMenu()
        {
            int startIndex = this.quickContextMenu.Items.IndexOf(this.unTaggedQuickMenuItem) + 1;
            while (startIndex < this.AlphabeticalMenuItemIndex)
            {
                // unregister event handler to release the menu item
                ToolStripMenuItem tagItem = this.quickContextMenu.Items[startIndex] as ToolStripMenuItem;
                tagItem.DropDownItemClicked -= this.quickContextMenu_ItemClicked;
                tagItem.DropDownOpening -= OnTagTrayMenuItemDropDownOpening;
                this.quickContextMenu.Items.RemoveAt(startIndex);
            }
        }

        private void AddTagTrayMenuItems()
        {
            foreach (String tag in Settings.Tags)
            {
                ToolStripMenuItem tagMenuItem = CreateTagMenuItem(tag);
                tagMenuItem.DropDownItemClicked += new ToolStripItemClickedEventHandler(this.quickContextMenu_ItemClicked);
                tagMenuItem.DropDownOpening += new EventHandler(OnTagTrayMenuItemDropDownOpening);
                this.quickContextMenu.Items.Insert(this.AlphabeticalMenuItemIndex, tagMenuItem);
            }
        }

        private static void OnTagTrayMenuItemDropDownOpening(object sender, EventArgs e)
        {
            TagMenuItem tagMenu = sender as TagMenuItem;
            if (tagMenu.IsEmpty)
            {
                tagMenu.DropDown.Items.Clear();
                List<FavoriteConfigurationElement> tagFavorites = Settings.GetSortedFavoritesByTag(tagMenu.Text);
                foreach (FavoriteConfigurationElement favorite in tagFavorites)
                {
                    ToolStripMenuItem item = CreateFavoriteMenuItem(favorite);
                    tagMenu.DropDown.Items.Add(item);
                }
            }
        }

        private void AddTraySpecialCommandsContextMenu()
        {
            AddCommandMenuItems();

            foreach (SpecialCommandConfigurationElement commad in Settings.SpecialCommands)
            {
                AddSpecialMenuItem(commad);
            }
        }

        private void AddSpecialMenuItem(SpecialCommandConfigurationElement commad)
        {
            ToolStripItem specialItem = CreateSpecialItem(commad);
            specialItem.Image = FavoriteIcons.LoadImage(commad.Thumbnail, Resources.server_administrator_icon);
            specialItem.ImageTransparentColor = Color.Magenta;
            specialItem.Tag = commad;
            specialItem.Click += new EventHandler(specialItem_Click);
        }

        private ToolStripItem CreateSpecialItem(SpecialCommandConfigurationElement commad)
        {
            String commandExeName = commad.Executable.ToLower();

            if (commandExeName.EndsWith("cpl"))
                return this.cpl.DropDown.Items.Add(commad.Name);
            if (commandExeName.EndsWith("msc"))
                return this.mgmt.DropDown.Items.Add(commad.Name);

            return this.other.DropDown.Items.Add(commad.Name);
        }

        private void AddCommandMenuItems()
        {
            this.special = new ToolStripMenuItem(Program.Resources.GetString(COMMAND_SPECIAL), Resources.computer_link);
            this.mgmt = new ToolStripMenuItem(Program.Resources.GetString(COMMAND_MANAGEMENT), Resources.CompMgmt);
            this.cpl = new ToolStripMenuItem(Program.Resources.GetString(COMMAND_CONTROLPANEL), Resources.ControlPanel);
            this.other = new ToolStripMenuItem(Program.Resources.GetString(COMMAND_OTHER));

            this.quickContextMenu.Items.Add(this.special);
            this.special.DropDown.Items.Add(this.mgmt);
            this.special.DropDown.Items.Add(this.cpl);
            this.special.DropDown.Items.Add(this.other);
        }

        private static void specialItem_Click(object sender, EventArgs e)
        {
            ToolStripItem specialItem = sender as ToolStripItem;
            SpecialCommandConfigurationElement elm = specialItem.Tag as SpecialCommandConfigurationElement;
            elm.Launch();
        }

        private static TagMenuItem CreateTagMenuItem(String tag)
        {
            TagMenuItem tagMenuItem = new TagMenuItem();
            tagMenuItem.Tag = TAG;
            tagMenuItem.Text = tag;
            return tagMenuItem;
        }

        private static ToolStripMenuItem CreateFavoriteMenuItem(FavoriteConfigurationElement favorite)
        {
            ToolStripMenuItem item = new ToolStripMenuItem(favorite.Name);
            item.Tag = FAVORITE;
            item.Image = FavoriteIcons.GetFavoriteIcon(favorite);
            item.ToolTipText = favorite.GetToolTipText();
            return item;
        }

        private void AddAlphabeticalContextMenu()
        {
            this.alphabeticalMenu = new ToolStripMenuItem(Program.Resources.GetString(COMMAND_ALPHABETICAL));
            this.alphabeticalMenu.Name = COMMAND_ALPHABETICAL;
            this.alphabeticalMenu.DropDownItemClicked += new ToolStripItemClickedEventHandler(this.quickContextMenu_ItemClicked);
            this.alphabeticalMenu.DropDownOpening += new EventHandler(this.OnAlphabeticalMenuDropDownOpening);
            this.alphabeticalMenu.Image = Resources.atoz;
            this.quickContextMenu.Items.Add(this.alphabeticalMenu);
        }

        private void OnAlphabeticalMenuDropDownOpening(object sender, EventArgs e)
        {
            if (!this.alphabeticalMenu.HasDropDownItems)
            {
                List<FavoriteConfigurationElement> favorites = Settings.GetFavorites()
                    .ToList().SortByProperty("Name", SortOrder.Ascending);

                CreateAlphabeticalFavoriteMenuItems(favorites);
                Boolean alphaMenuVisible = this.alphabeticalMenu.DropDownItems.Count > 0;
                this.alphabeticalMenu.Visible = alphaMenuVisible;
            }
        }

        private void CreateAlphabeticalFavoriteMenuItems(List<FavoriteConfigurationElement> favorites)
        {
            foreach (FavoriteConfigurationElement favorite in favorites)
            {
                ToolStripMenuItem sortedItem = CreateFavoriteMenuItem(favorite);
                this.alphabeticalMenu.DropDownItems.Add(sortedItem);
            }
        }

        private void AddGeneralTrayContextMenu()
        {
            restoreScreenMenuItem = this.CreateGeneralTrayContextMenuItem(COMMAND_RESTORESCREEN, Resources.arrow_in);
            fullScreenMenuItem = this.CreateGeneralTrayContextMenuItem(COMMAND_FULLSCREEN, Resources.arrow_out);

            this.quickContextMenu.Items.Add("-");
            ToolStripItem showMenu = this.quickContextMenu.Items.Add(Program.Resources.GetString(COMMAND_SHOWMENU));
            showMenu.Name = COMMAND_SHOWMENU;
            this.quickContextMenu.Items.Add("-");
            CreateGeneralTrayContextMenuItem(COMMAND_CAPTUREMANAGER, Resources.screen_capture_box);
            CreateGeneralTrayContextMenuItem(COMMAND_NETTOOLS, Resources.computer_link);
            this.quickContextMenu.Items.Add("-");
            CreateGeneralTrayContextMenuItem(COMMAND_CREDENTIALMANAGER, Resources.computer_security);
            CreateGeneralTrayContextMenuItem(COMMAND_ORGANIZEFAVORITES, Resources.star);
            CreateGeneralTrayContextMenuItem(COMMAND_OPTIONS, Resources.options);
            this.quickContextMenu.Items.Add("-");
        }

        internal void UpdateSwitchFullScreenMenuItemsVisibility(Boolean fullScreen)
        {
            restoreScreenMenuItem.Visible = fullScreen;
            fullScreenMenuItem.Visible = !fullScreen;
        }

        private ToolStripItem CreateGeneralTrayContextMenuItem(String commnadName, Image icon)
        {
            String menuText = Program.Resources.GetString(commnadName);
            ToolStripItem menuItem = this.quickContextMenu.Items.Add(menuText, icon);
            menuItem.Name = commnadName;
            return menuItem;
        }

        #endregion
    }
}
