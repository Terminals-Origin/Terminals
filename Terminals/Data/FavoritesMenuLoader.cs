using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Terminals.Data;
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

            CreateFavoritesSeparator();
            CreateTrayMenuItems();
            UpdateMenuAndContextMenu();
            
            DataDispatcher.Instance.TagsChanged += new TagsChangedEventHandler(this.OnDataChanged);
            DataDispatcher.Instance.FavoritesChanged += new FavoritesChangedEventHandler(this.OnDataChanged);
        }

        private void OnDataChanged(EventArgs args)
        {
            UpdateMenuAndContextMenu();
        }

        private void UpdateMenuAndContextMenu()
        {
            this.FillMainMenu();
            this.FillTrayContextMenu();
        }

        private void CreateFavoritesSeparator()
        {
            this.favoritesSeparator = new ToolStripSeparator();
            this.favoritesSeparator.Name = "favoritesSeparator";
            this.favoritesSeparator.Size = new Size(212, 6);
            this.favoritesToolStripMenuItem.DropDownItems.Add(this.favoritesSeparator);
        }

        /// <summary>
        /// Creates skeleton for system tray menu items. No tags or favorites are added here.
        /// </summary>
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

        private void FillMainMenu()
        {
            ReFreshConnectionsComboBox();
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
            Int32 seperatorIndex = this.favoritesToolStripMenuItem.DropDownItems.IndexOf(this.favoritesSeparator);
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
            ToolStripMenuItem tagMenu = sender as ToolStripMenuItem;
            if (!tagMenu.HasDropDownItems)
            {
                List<FavoriteConfigurationElement> tagFavorites = Settings.GetFavoritesByTag(tagMenu.Text);
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

        private void FillTrayContextMenu()
        {
            this.ClearTrayFavoritesMenu();
            this.AddTagTrayMenuItems();
            this.alphabeticalMenu.DropDownItems.Clear();
        }

        private void ClearTrayFavoritesMenu()
        {
            int startIndex = this.quickContextMenu.Items.IndexOf(this.favoritesTrayStartSeparator) + 1;
            while (startIndex < this.AlphabeticalMenuItemIndex)
            {
                // unregister event handler to release the menu item
                ToolStripMenuItem tagItem = this.quickContextMenu.Items[startIndex] as ToolStripMenuItem;
                tagItem.DropDownItemClicked -= this.quickContextMenu_ItemClicked;
                tagItem.DropDownOpening -= this.OnTagTrayMenuItemDropDownOpening;
                this.quickContextMenu.Items.RemoveAt(startIndex);
            }
        }

        private void AddTagTrayMenuItems()
        {
            foreach (String tag in Settings.Tags)
            {
                ToolStripMenuItem tagMenuItem = CreateTagMenuItem(tag);
                tagMenuItem.DropDownItemClicked += new ToolStripItemClickedEventHandler(this.quickContextMenu_ItemClicked);
                tagMenuItem.DropDownOpening += new EventHandler(this.OnTagTrayMenuItemDropDownOpening);
                this.quickContextMenu.Items.Insert(this.AlphabeticalMenuItemIndex, tagMenuItem);
            }
        }

        private void OnTagTrayMenuItemDropDownOpening(object sender, EventArgs e)
        {
            ToolStripMenuItem tagMenu = sender as ToolStripMenuItem;
            if (!tagMenu.HasDropDownItems)
            {
                List<FavoriteConfigurationElement> tagFavorites = Settings.GetFavoritesByTag(tagMenu.Text);
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
            specialItem.Image = GetMenuItemImage(commad.Thumbnail, Resources.server_administrator_icon);
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
            ToolStripItem specialItem = (ToolStripItem)sender;
            SpecialCommandConfigurationElement elm = (SpecialCommandConfigurationElement)specialItem.Tag;
            elm.Launch();
        }

        private static ToolStripMenuItem CreateTagMenuItem(String tag)
        {
            ToolStripMenuItem tagMenuItem = new ToolStripMenuItem();
            tagMenuItem.Tag = TAG;
            tagMenuItem.Text = tag;
            return tagMenuItem;
        }

        private static ToolStripMenuItem CreateFavoriteMenuItem(FavoriteConfigurationElement favorite)
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
            this.alphabeticalMenu.DropDownOpening += new EventHandler(this.OnAlphabeticalMenuDropDownOpening);
            this.alphabeticalMenu.Image = Resources.atoz;
            this.quickContextMenu.Items.Add(this.alphabeticalMenu);
        }

        private void OnAlphabeticalMenuDropDownOpening(object sender, EventArgs e)
        {
            if (!this.alphabeticalMenu.HasDropDownItems)
            {
                foreach (FavoriteConfigurationElement favorite in Settings.GetFavorites())
                {
                    ToolStripMenuItem sortedItem = CreateFavoriteMenuItem(favorite);
                    this.alphabeticalMenu.DropDownItems.Add(sortedItem);
                }

                Boolean alphaMenuVisible = this.alphabeticalMenu.DropDownItems.Count > 0;
                this.alphabeticalMenu.Visible = alphaMenuVisible;
                this.favoritesTrayStartSeparator.Visible = alphaMenuVisible;
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
