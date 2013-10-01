using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Terminals.Configuration;

namespace Terminals
{
    internal partial class OrganizeShortcuts : Form
    {
        public OrganizeShortcuts()
        {
            InitializeComponent();
            
        }

        private void shortcutCombobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.executableTextBox.Text = "";
            this.argumentsTextBox.Text = "";
            this.workingFolderTextBox.Text = "";
            this.iconPicturebox.Image = null;
            this.IconImageList.Images.Clear();
            this.toolStrip1.Items.Clear();

            string name = shortcutCombobox.Text;
            if (name.Trim() != "" && name.Trim()!="<New...>")
            {
                SpecialCommandConfigurationElement shortcut = shortucts[name];
                if (shortcut != null)
                {
                    this.executableTextBox.Text = shortcut.Executable;
                    this.argumentsTextBox.Text = shortcut.Arguments;
                    this.workingFolderTextBox.Text = shortcut.WorkingFolder;
                    LoadIconsFromExe();
                }
            }
            else if (name.Trim() == "<New...>")
            {
                shortcutCombobox.SelectedIndex = -1;
                shortcutCombobox.Focus();
            }
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            string name = shortcutCombobox.Text;
            if (name.Trim() != "" && name.Trim() != "<New...>")
            {
                SpecialCommandConfigurationElement shortcut = shortucts[name];
                if(shortcut==null) shortcut = new SpecialCommandConfigurationElement();

                shortcut.Name = name.Trim();
                shortcut.Executable = executableTextBox.Text;
                shortcut.WorkingFolder = workingFolderTextBox.Text;
                shortcut.Arguments = argumentsTextBox.Text;
                string imageName = Path.GetFileName(shortcut.Executable) + ".ico";
                string imageFullName = Path.Combine(FileLocations.ThumbsDirectoryFullPath, imageName);
                if(this.iconPicturebox.Image != null)
                {
                    try
                    {
                        this.iconPicturebox.Image.Save(imageFullName);
                    }
                    catch(Exception exc)
                    {
                        Logging.Error("Saving icon picture box failed", exc);
                        imageFullName = "";
                    }
                }
                else
                {
                    imageFullName = "";
                }
                shortcut.Thumbnail = imageFullName;
                shortucts.Remove(shortcut);
                shortucts.Add(shortcut);
                
                Settings.SpecialCommands = shortucts;
                shortucts = Settings.SpecialCommands;
            }
        }

        SpecialCommandConfigurationElementCollection shortucts = Settings.SpecialCommands;

        private void OrganizeShortcuts_Load(object sender, EventArgs e)
        {
            LoadShortcuts();
        }
        private void LoadShortcuts()
        {
            shortcutCombobox.Items.Clear();
            this.shortcutCombobox.Items.Add("<New...>");
            foreach (SpecialCommandConfigurationElement shortcut in shortucts)
            {
                this.shortcutCombobox.Items.Add(shortcut.Name);
            }
            if(shortcutCombobox.Items.Count>0)  this.shortcutCombobox.SelectedIndex = 0;
            
        }
        private void deleteButton_Click(object sender, EventArgs e)
        {
            string name = shortcutCombobox.Text;
            if (name.Trim() != "" && name.Trim() != "<New...>")
            {
                SpecialCommandConfigurationElement shortcut = shortucts[name];
                if (shortcut != null)
                {
                    shortucts.Remove(shortcut);
                    Settings.SpecialCommands = shortucts;
                    shortucts = Settings.SpecialCommands;
                }
            }
            LoadShortcuts();
        }

        private void LoadIconsFromExe()
        {
            try
            {
                Icon[] IconsList = IconHandler.IconHandler.IconsFromFile(this.executableTextBox.Text, IconHandler.IconSize.Small);
                if (IconsList != null && IconsList.Length > 0)
                {
                    this.iconPicturebox.Image = IconsList[0].ToBitmap();

                    IconImageList.Images.Clear();
                    toolStrip1.Items.Clear();
                    foreach (Icon TmpIcon in IconsList)
                    {
                        IconImageList.Images.Add(TmpIcon);
                        toolStrip1.Items.Add(TmpIcon.ToBitmap());
                    }


                }

            }
            catch (Exception exc)
            {
                Logging.Error("LoadIconsFromExe", exc);
            }
        }
        
        private void executableBrowseButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.CheckFileExists = true;
            ofd.CheckPathExists = true;
            ofd.Filter = "Executable Files|*.exe";
            ofd.Multiselect = false;
            ofd.ShowReadOnly = true;
            ofd.Title = "Browse for executable...";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                this.executableTextBox.Text = ofd.FileName;                
                LoadIconsFromExe();
            }
        }


        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            this.iconPicturebox.Image = e.ClickedItem.Image;
        }
    }
}