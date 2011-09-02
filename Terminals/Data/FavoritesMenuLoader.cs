using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Terminals.Properties;
using Settings = Terminals.Configuration.Settings;

namespace Terminals
{
    /// <summary>
    /// Fills menu, tool strip menu and tool bar with favorite buttons
    /// </summary>
    internal class FavoritesMenuLoader
    {
        private ToolStripMenuItem favoritesToolStripMenuItem;
        private ToolStripSeparator favoritesSeparator;
        private ToolStripComboBox tscConnectTo;
        private EventHandler serverToolStripMenuItem_Click;
        private ToolStrip favoriteToolBar;
        private ToolStripMenuItem toolStripMenuItemShowHideFavoriteToolbar;
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
        private ToolStripItem favoritesTrayStartSeparator;
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

        internal FavoritesMenuLoader(ToolStripMenuItem favoritesToolStripMenuItem, ToolStripSeparator favoritesSeparator,
            ToolStripComboBox tscConnectTo, EventHandler serverToolStripMenuItem_Click, ToolStrip favoriteToolBar,
            ToolStripMenuItem toolStripMenuItemShowHideFavoriteToolbar, ContextMenuStrip quickContextMenu,
            ToolStripItemClickedEventHandler quickContextMenu_ItemClicked)
        {
            this.favoritesToolStripMenuItem = favoritesToolStripMenuItem;
            this.favoritesSeparator = favoritesSeparator;
            this.tscConnectTo = tscConnectTo;
            this.serverToolStripMenuItem_Click = serverToolStripMenuItem_Click;
            this.favoriteToolBar = favoriteToolBar;
            this.toolStripMenuItemShowHideFavoriteToolbar = toolStripMenuItemShowHideFavoriteToolbar;
            this.quickContextMenu = quickContextMenu;
            this.quickContextMenu_ItemClicked = quickContextMenu_ItemClicked;

            CreateTrayMenuItems();
        }

        private void CreateTrayMenuItems()
        {
            this.AddGeneralTrayContextMenu();
            this.AddTraySpecialCommandsContextMenu();
            this.favoritesTrayStartSeparator = this.quickContextMenu.Items.Add("-");

            // here favorite Tags will be placed

            this.AddAlphabeticalContextMenu();
            this.quickContextMenu.Items.Add("-");
            this.quickContextMenu.Items.Add(Program.Resources.GetString(COMMAND_EXIT));
        }

        internal void FillMenu()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            ClearFavoritesToolStripmenuItems();
            ReFreshConnectionsComboBox();
            CreateFavoritesToolStrips();
            this.LoadFavoritesToolbar();

            stopwatch.Stop();
            Debug.WriteLine(String.Format("Favorite menu loaded in {0} ms", stopwatch.ElapsedMilliseconds));
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

        /// <summary>
        /// Fills the main window "favorites" menu, after separator places all tags
        /// and their favorites as dropdown items
        /// </summary>
        private void ClearFavoritesToolStripmenuItems()
        {
            Int32 seperatorIndex = this.favoritesToolStripMenuItem.DropDownItems.IndexOf(this.favoritesSeparator);
            for (Int32 i = this.favoritesToolStripMenuItem.DropDownItems.Count - 1; i > seperatorIndex; i--)
            {
                this.favoritesToolStripMenuItem.DropDownItems.RemoveAt(i);
            }
        }

        private void CreateFavoritesToolStrips()
        {
            FavoriteConfigurationElementCollection favorites = Settings.GetFavorites();
            Dictionary<String, ToolStripMenuItem> tagTools = new Dictionary<String, ToolStripMenuItem>();

            foreach (FavoriteConfigurationElement favorite in favorites)
            {
                if (favorite.TagList.Count > 0) // TagList is never null
                {
                    foreach (String tag in favorite.TagList)
                    {
                        ToolStripMenuItem tagMenu = this.GetParentToolStripMenuItemByTag(tagTools, tag);

                        if (tagMenu != null)
                        {
                            ToolStripMenuItem item = this.CreateToolStripItemByFavorite(favorite);
                            tagMenu.DropDown.Items.Add(item);
                        }
                    }
                }
                else
                {
                    ToolStripMenuItem item = this.CreateToolStripItemByFavorite(favorite);
                    this.favoritesToolStripMenuItem.DropDown.Items.Add(item);
                }
            }
        }

        private ToolStripMenuItem GetParentToolStripMenuItemByTag(Dictionary<String, ToolStripMenuItem> tagTools, String tag)
        {
            tag = tag.ToLower();
            ToolStripMenuItem tagMenu = null;
            if (tagTools.ContainsKey(tag))
            {
                tagMenu = tagTools[tag];
            }
            else if (!tag.Contains("Terminals"))
            {
                tagMenu = new ToolStripMenuItem(tag);
                tagMenu.Name = tag;
                tagTools.Add(tag, tagMenu);
                this.favoritesToolStripMenuItem.DropDown.Items.Add(tagMenu);
            }
            return tagMenu;
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
                favoriteToolBar.Items.Clear();
                if (Settings.FavoritesToolbarButtons != null)
                {
                    FavoriteConfigurationElementCollection favorites = Settings.GetFavorites();
                    foreach (String favoriteButton in Settings.FavoritesToolbarButtons)
                    {
                        FavoriteConfigurationElement favorite = favorites[favoriteButton];
                        if (favorite != null)
                        {
                            ToolStripButton favoriteBtn = CreateFavoriteButton(favorite);
                            favoriteToolBar.Items.Add(favoriteBtn);
                        }
                    }
                }

                favoriteToolBar.Visible = toolStripMenuItemShowHideFavoriteToolbar.Checked;
            }
            catch (Exception exc)
            {
                Logging.Log.Error("Error Loading Favorites Toolbar", exc);
            }
        }

        private ToolStripButton CreateFavoriteButton(FavoriteConfigurationElement favorite)
        {
            Image buttonImage = GetMenuItemImage(favorite.ToolBarIcon, Resources.smallterm);
            ToolStripButton favoriteBtn = new ToolStripButton(favorite.Name, buttonImage, this.serverToolStripMenuItem_Click);
            favoriteBtn.Tag = favorite;
            favoriteBtn.Overflow = ToolStripItemOverflow.AsNeeded;
            return favoriteBtn;
        }

        private static Image GetMenuItemImage(String imageFilePath, Image defaultIcon)
        {
            try
            {
                if (!String.IsNullOrEmpty(imageFilePath) && File.Exists(imageFilePath))
                {
                    return Image.FromFile(imageFilePath);
                }
            }
            catch (Exception ex)
            {
                Logging.Log.Error("Error Loading Favorites Toolbar (Button Bar)", ex);
            }

            return defaultIcon;
        }

        #endregion

        #region System tray context menu

        internal void FillTrayContextMenu(Boolean fullScreen)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            this.UpdateSwitchFullScreenMenuItemsVisibility(fullScreen);
            ClearTrayFavoritesMenu();
            this.AddFavoritesTrayContextMenu();

            stopwatch.Stop();
            Debug.WriteLine(String.Format("System tray menu loaded in {0} ms", stopwatch.ElapsedMilliseconds));
        }

        private void ClearTrayFavoritesMenu()
        {
            int startIndex = this.quickContextMenu.Items.IndexOf(this.favoritesTrayStartSeparator);
            while (startIndex + 1 < this.AlphabeticalMenuItemIndex)
            {
                this.quickContextMenu.Items.RemoveAt(startIndex + 1);
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
            specialItem.Image = GetMenuItemImage(commad.Thumbnail, Resources.server_administrator_icon);
            specialItem.ImageTransparentColor = Color.Magenta;
            specialItem.Tag = commad;
            specialItem.Click += new EventHandler(this.specialItem_Click);
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

        private void specialItem_Click(object sender, EventArgs e)
        {
            ToolStripItem specialItem = (ToolStripItem)sender;
            SpecialCommandConfigurationElement elm = (SpecialCommandConfigurationElement)specialItem.Tag;
            elm.Launch();
        }
        
        private void AddFavoritesTrayContextMenu()
        {
            FavoriteConfigurationElementCollection favorites = Settings.GetFavorites();
            Dictionary<String, ToolStripMenuItem> tagTools = new Dictionary<String, ToolStripMenuItem>();
            SortedDictionary<String, ToolStripMenuItem> sortedList = new SortedDictionary<String, ToolStripMenuItem>();

            foreach (FavoriteConfigurationElement favorite in favorites)
            {
                ToolStripMenuItem sortedItem = CreateFavoriteMenuItem(favorite);
                sortedList.Add(favorite.Name, sortedItem);
                AddFavoriteMenuItemByTag(favorite, tagTools);
            }

            FillAlphabeticalMenu(sortedList);
        }

        private void AddFavoriteMenuItemByTag(FavoriteConfigurationElement favorite, Dictionary<String, ToolStripMenuItem> tagTools)
        {
            if (favorite.TagList.Count > 0)
            {
                foreach (String tag in favorite.TagList)
                {
                    ToolStripMenuItem parent = this.EnsureTagMenuItem(tagTools, tag);
                    ToolStripMenuItem item = this.CreateFavoriteMenuItem(favorite);
                    parent.DropDown.Items.Add(item);
                }
            }
            else
            {
                ToolStripMenuItem item = this.CreateFavoriteMenuItem(favorite);
                this.quickContextMenu.Items.Add(item);
            }
        }

        private ToolStripMenuItem EnsureTagMenuItem(Dictionary<String, ToolStripMenuItem> tagTools, String tag)
        {
            if (tagTools.ContainsKey(tag))
                return tagTools[tag];

            ToolStripMenuItem parent = this.CreateTagMenuItem(tag);
            tagTools.Add(tag, parent);
            
            this.quickContextMenu.Items.Insert(this.AlphabeticalMenuItemIndex, parent);
            return parent;
        }

        private ToolStripMenuItem CreateTagMenuItem(String tag)
        {
            ToolStripMenuItem parent = new ToolStripMenuItem();
            parent.DropDownItemClicked += new ToolStripItemClickedEventHandler(this.quickContextMenu_ItemClicked);
            parent.Tag = TAG;
            parent.Text = tag;
            return parent;
        }
       
        private ToolStripMenuItem CreateFavoriteMenuItem(FavoriteConfigurationElement favorite)
        {
            ToolStripMenuItem item = new ToolStripMenuItem(favorite.Name);
            item.Tag = FAVORITE;
            item.Image = GetMenuItemImage(favorite.ToolBarIcon, Resources.smallterm);
            return item;
        }

        private void AddAlphabeticalContextMenu()
        {
            this.alphabeticalMenu = new ToolStripMenuItem(Program.Resources.GetString(COMMAND_ALPHABETICAL));
            this.alphabeticalMenu.Name = COMMAND_ALPHABETICAL;
            this.alphabeticalMenu.DropDownItemClicked += new ToolStripItemClickedEventHandler(this.quickContextMenu_ItemClicked);
            this.alphabeticalMenu.Image = Resources.atoz;
            this.quickContextMenu.Items.Add(this.alphabeticalMenu);
        }

        private void FillAlphabeticalMenu(SortedDictionary<String, ToolStripMenuItem> sortedList)
        {
            this.alphabeticalMenu.DropDownItems.Clear();
            foreach (String name in sortedList.Keys)
            {
                this.alphabeticalMenu.DropDownItems.Add(sortedList[name]);
            }

            Boolean alphaMenuVisible = sortedList.Count > 0;
            this.alphabeticalMenu.Visible = alphaMenuVisible;
            this.favoritesTrayStartSeparator.Visible = alphaMenuVisible;
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

        private void UpdateSwitchFullScreenMenuItemsVisibility(Boolean fullScreen)
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
