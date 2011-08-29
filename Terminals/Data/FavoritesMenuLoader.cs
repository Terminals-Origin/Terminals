using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
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
        }

        internal void FillMenu()
        {
            ClearFavoritesToolStripmenuItems();
            tscConnectTo.Items.Clear();

            Debug.WriteLine(DateTime.Now.ToString("hh:mm:ss,fff") + ": CreateFavoritesToolStrips");
            CreateFavoritesToolStrips();

            Debug.WriteLine(DateTime.Now.ToString("hh:mm:ss,fff") + ": LoadFavoritesToolbar");
            this.LoadFavoritesToolbar();
            Debug.WriteLine(DateTime.Now.ToString("hh:mm:ss,fff") + ": LoadFavorites - DONE");
        }

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
                this.tscConnectTo.Items.Add(favorite.Name);

                if (favorite.TagList.Count > 0) // TagList is never null
                {
                    foreach (String tag in favorite.TagList)
                    {
                        ToolStripMenuItem parent = this.GetParentToolStripMenuItemByTag(tagTools, tag);

                        if (parent != null)
                        {
                            ToolStripMenuItem item = this.CreateToolStripItemByFavorite(favorite);
                            parent.DropDown.Items.Add(item);
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
            ToolStripMenuItem parent = null;
            if (tagTools.ContainsKey(tag))
            {
                parent = tagTools[tag];
            }
            else if (!tag.Contains("Terminals"))
            {
                parent = new ToolStripMenuItem(tag);
                parent.Name = tag;
                tagTools.Add(tag, parent);
                this.favoritesToolStripMenuItem.DropDown.Items.Add(parent);
            }
            return parent;
        }

        private ToolStripMenuItem CreateToolStripItemByFavorite(FavoriteConfigurationElement favorite)
        {
            ToolStripMenuItem item = new ToolStripMenuItem(favorite.Name);
            item.Name = favorite.Name;
            item.Tag = "favorite";

            if (favorite.ToolBarIcon != null && File.Exists(favorite.ToolBarIcon))
                item.Image = Image.FromFile(favorite.ToolBarIcon);

            item.Click += this.serverToolStripMenuItem_Click;
            return item;
        }

        private void AddFavorite(FavoriteConfigurationElement favorite)
        {
            tscConnectTo.Items.Add(favorite.Name);
            ToolStripMenuItem serverToolStripMenuItem = new ToolStripMenuItem(favorite.Name);
            serverToolStripMenuItem.Name = favorite.Name;
            serverToolStripMenuItem.Click += serverToolStripMenuItem_Click;
            favoritesToolStripMenuItem.DropDownItems.Add(serverToolStripMenuItem);
        }

        private void LoadFavoritesToolbar()
        {
            try
            {
                favoriteToolBar.Items.Clear();
                if (Settings.FavoritesToolbarButtons != null)
                {
                    foreach (String favoriteButton in Settings.FavoritesToolbarButtons)
                    {
                        FavoriteConfigurationElementCollection favorites = Settings.GetFavorites();
                        FavoriteConfigurationElement favorite = favorites[favoriteButton];
                        Bitmap button = Resources.smallterm;
                        if (favorite != null)
                        {
                            if (!String.IsNullOrEmpty(favorite.ToolBarIcon) && File.Exists(favorite.ToolBarIcon))
                            {
                                try
                                {
                                    button = (Bitmap)Image.FromFile(favorite.ToolBarIcon);
                                }
                                catch (Exception ex)
                                {
                                    Logging.Log.Error("Error Loading Favorites Toolbar (Button Bar)", ex);
                                    button = Resources.smallterm;
                                }
                            }

                            ToolStripButton favoriteBtn = new ToolStripButton(favorite.Name, button, serverToolStripMenuItem_Click);
                            favoriteBtn.Tag = favorite;
                            favoriteBtn.Overflow = ToolStripItemOverflow.AsNeeded;
                            favoriteToolBar.Items.Add(favoriteBtn);
                        }
                    }
                }

                favoriteToolBar.Visible = toolStripMenuItemShowHideFavoriteToolbar.Checked;
                //this.favsList1.LoadFavs();
            }
            catch (Exception exc)
            {
                Logging.Log.Error("Error Loading Favorites Toolbar", exc);
            }
        }

        internal void FillTrayContextMenu(Boolean fullScreen)
        {
            this.quickContextMenu.Items.Clear();

            this.AddGeneralTrayContextMenu(fullScreen);
            this.AddTraySpecialCommandsContextMenu();
            this.quickContextMenu.Items.Add("-");
            this.AddFavoritesTrayContextMenu();
            this.quickContextMenu.Items.Add("-");
            this.quickContextMenu.Items.Add(Program.Resources.GetString("Exit"));
        }


        private void AddTraySpecialCommandsContextMenu()
        {
            ToolStripMenuItem special = new ToolStripMenuItem(Program.Resources.GetString("SpecialCommands"), Resources.computer_link);
            ToolStripMenuItem mgmt = new ToolStripMenuItem(Program.Resources.GetString("Management"), Resources.CompMgmt);
            ToolStripMenuItem cpl = new ToolStripMenuItem(Program.Resources.GetString("ControlPanel"), Resources.ControlPanel);
            ToolStripMenuItem other = new ToolStripMenuItem(Program.Resources.GetString("Other"));

            this.quickContextMenu.Items.Add(special);
            special.DropDown.Items.Add(mgmt);
            special.DropDown.Items.Add(cpl);
            special.DropDown.Items.Add(other);

            foreach (SpecialCommandConfigurationElement elm in Settings.SpecialCommands)
            {
                Image img = null;
                if (!String.IsNullOrEmpty(elm.Thumbnail) && File.Exists(elm.Thumbnail))
                {
                    img = Image.FromFile(elm.Thumbnail);
                }
                else
                {
                    img = Resources.server_administrator_icon;
                }

                ToolStripItem specialItem;
                if (elm.Executable.ToLower().EndsWith("cpl"))
                {
                    specialItem = cpl.DropDown.Items.Add(elm.Name, img);
                }
                else if (elm.Executable.ToLower().EndsWith("msc"))
                {
                    specialItem = mgmt.DropDown.Items.Add(elm.Name, img);
                }
                else
                {
                    specialItem = other.DropDown.Items.Add(elm.Name, img);
                }

                specialItem.Click += new EventHandler(this.specialItem_Click);
                specialItem.Tag = elm;
                specialItem.ImageTransparentColor = Color.Magenta;
            }
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
            ToolStripMenuItem sortedMenu = new ToolStripMenuItem(Program.Resources.GetString("Alphabetical"));
            sortedMenu.DropDownItemClicked += new ToolStripItemClickedEventHandler(this.quickContextMenu_ItemClicked);

            foreach (FavoriteConfigurationElement favorite in favorites)
            {
                ToolStripMenuItem sortedItem = new ToolStripMenuItem();
                sortedItem.Text = favorite.Name;
                sortedItem.Tag = "favorite";
                if (favorite.ToolBarIcon != null && File.Exists(favorite.ToolBarIcon))
                    sortedItem.Image = Image.FromFile(favorite.ToolBarIcon);

                sortedList.Add(favorite.Name, sortedItem);

                if (favorite.TagList != null && favorite.TagList.Count > 0)
                {
                    foreach (String tag in favorite.TagList)
                    {
                        ToolStripMenuItem parent;
                        if (tagTools.ContainsKey(tag))
                        {
                            parent = tagTools[tag];
                        }
                        else
                        {
                            parent = new ToolStripMenuItem();
                            parent.DropDownItemClicked += new ToolStripItemClickedEventHandler(this.quickContextMenu_ItemClicked);
                            parent.Tag = "tag";
                            parent.Text = tag;
                            tagTools.Add(tag, parent);
                            this.quickContextMenu.Items.Add(parent);
                        }

                        ToolStripMenuItem item = new ToolStripMenuItem();
                        item.Text = favorite.Name;
                        item.Tag = "favorite";
                        if (favorite.ToolBarIcon != null && File.Exists(favorite.ToolBarIcon))
                            item.Image = Image.FromFile(favorite.ToolBarIcon);

                        parent.DropDown.Items.Add(item);
                    }
                }
                else
                {
                    ToolStripMenuItem item = new ToolStripMenuItem(favorite.Name);
                    item.Tag = "favorite";
                    if (favorite.ToolBarIcon != null && File.Exists(favorite.ToolBarIcon))
                        item.Image = Image.FromFile(favorite.ToolBarIcon);

                    this.quickContextMenu.Items.Add(item);
                }
            }

            if (sortedList != null && sortedList.Count > 0)
            {
                this.quickContextMenu.Items.Add(sortedMenu);
                sortedMenu.Image = Resources.atoz;
                foreach (string name in sortedList.Keys)
                {
                    sortedMenu.DropDownItems.Add(sortedList[name]);
                }
            }
        }

        private void AddGeneralTrayContextMenu(Boolean fullScreen)
        {
            if (fullScreen)
                this.quickContextMenu.Items.Add(Program.Resources.GetString("RestoreScreen"), Resources.arrow_in);
            else
                this.quickContextMenu.Items.Add(Program.Resources.GetString("FullScreen"), Resources.arrow_out);

            this.quickContextMenu.Items.Add("-");
            this.quickContextMenu.Items.Add(Program.Resources.GetString("ShowMenu"));

            this.quickContextMenu.Items.Add("-");
            this.quickContextMenu.Items.Add(Program.Resources.GetString("ScreenCaptureManager"), Resources.screen_capture_box);
            this.quickContextMenu.Items.Add(Program.Resources.GetString("NetworkingTools"), Resources.computer_link);
            this.quickContextMenu.Items.Add("-");
            this.quickContextMenu.Items.Add(Program.Resources.GetString("CredentialsManager"), Resources.computer_security);
            this.quickContextMenu.Items.Add(Program.Resources.GetString("OrganizeFavorites"), Resources.star);
            this.quickContextMenu.Items.Add(Program.Resources.GetString("Options"), Resources.options);
            this.quickContextMenu.Items.Add("-");
        }
    }
}
