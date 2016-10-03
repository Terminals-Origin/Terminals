using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using Terminals.Configuration;
using Terminals.Connections;
using Terminals.Data;

namespace Terminals.Forms
{
    internal partial class OptionDialog : Form
    {
        private readonly Settings settings = Settings.Instance;
        private UserControl currentPanel;

        public OptionDialog(IConnectionExtra terminal, IPersistence persistence)
        {
            this.ApplySystemFont();

            InitializeComponent();

            this.panelMasterPassword.Security = persistence.Security;
            MovePanelsFromTabsIntoControls();
            settings.ConfigurationChanged += new ConfigurationChangedHandler(this.SettingsConfigFileReloaded);
            LoadSettings();
            
            this.SetFormSize();
            UpdateLookAndFeel(terminal);
        }

        private void SettingsConfigFileReloaded(ConfigurationChangedEventArgs args)
        {
            LoadSettings();
        }

        private void UpdateLookAndFeel(IConnectionExtra terminal)
        {
            // Update the old treeview theme to the new theme
            Native.Methods.SetWindowTheme(this.OptionsTreeView.Handle, "Explorer", null);

            this.panelConnections.CurrentTerminal = terminal;
            this.currentPanel = this.panelStartupShutdown;
            this.OptionsTreeView.SelectedNode = this.OptionsTreeView.Nodes[0];
            this.OptionsTreeView.Select();

            this.DrawBottomLine();
        }

        /// <summary>
        /// Set default font type by Windows theme to use for all controls on form
        /// </summary>
        private void ApplySystemFont()
        {
            this.Font = SystemFonts.IconTitleFont;
            this.AutoScaleMode = AutoScaleMode.Dpi;
        }

        private void SetFormSize()
        {
            // The option title label is the anchor for the form's width
            Int32 formWidth = this.OptionTitelLabel.Location.X + this.OptionTitelLabel.Width + 15;
            this.Width = formWidth;
        }

        /// <summary>
        /// Hide tabpage control, only used in design time
        /// </summary>
        private void MovePanelsFromTabsIntoControls()
        {
            this.tabCtrlOptionPanels.Hide();
            this.CollectOptionPanelControls();
        }

        /// <summary>
        /// Get all the panel control from the tabpages 
        /// and add them to the form controls collection and hide the controls
        /// </summary>
        private void CollectOptionPanelControls()
        {
            foreach (TabPage tp in this.tabCtrlOptionPanels.TabPages)
            {
                foreach (Control ctrl in tp.Controls)
                {
                    if (ctrl is UserControl)
                    {
                        ctrl.Hide();
                        this.Controls.Add(ctrl);
                    }
                }
            }
        }

        private void DrawBottomLine()
        {
            Label lbl = new Label();
            lbl.AutoSize = false;
            lbl.BorderStyle = BorderStyle.Fixed3D;
            lbl.SetBounds(
                this.OptionTitelLabel.Left,
                this.OptionsTreeView.Top + this.OptionsTreeView.Height - 1,
                this.OptionTitelLabel.Width,
                2);
            this.Controls.Add(lbl);
            lbl.Show();
        }

        private void OptionsTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                this.currentPanel.Hide();
                SelectNewPanel();
                UpdatePanelPosition();
                this.currentPanel.Show();
                this.OptionTitelLabel.Text = this.OptionsTreeView.SelectedNode.Name.Replace("&", "&&");
                UpdateTreeNodeState(e);
            }
            catch (Exception ex)
            {
                Logging.Info(ex);
            }
        }

        private static void UpdateTreeNodeState(TreeViewEventArgs e)
        {
            if (e.Node.GetNodeCount(true) > 0)
            {
                switch (e.Action)
                {
                    case TreeViewAction.ByKeyboard:
                    case TreeViewAction.ByMouse:
                        if (e.Node.IsExpanded)
                            e.Node.Collapse();
                        else
                            e.Node.Expand();
                        break;
                }
            }
        }

        private void SelectNewPanel()
        {
            string panelName = "panel" + this.OptionsTreeView.SelectedNode.Tag;
            System.Diagnostics.Debug.WriteLine("Selected panel: " + panelName);
            this.currentPanel = this.Controls[panelName] as UserControl;
        }

        private void UpdatePanelPosition()
        {
            Int32 x = this.OptionTitelLabel.Left;
            Int32 y = this.OptionTitelLabel.Top + this.OptionTitelLabel.Height + 3;
            this.currentPanel.Location = new Point(x, y);
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            try
            {
                settings.StartDelayedUpdate();
                SaveAllPanels();
            }
            catch (Exception exception)
            {
                Logging.Error("Error saving application settings.", exception);
                MessageBox.Show(String.Format("Error saving settings.\r\n{0}", exception.Message));
            }
            finally
            {
                settings.SaveAndFinishDelayedUpdate();
            }
        }

        private void SaveAllPanels()
        {
            foreach (IOptionPanel optionPanel in FindOptionPanels())
            {
                optionPanel.SaveSettings();
            }
        }

        private void LoadSettings()
        {
            foreach (IOptionPanel optionPanel in FindOptionPanels())
            {
                optionPanel.LoadSettings();
            }
        }

        private IEnumerable<IOptionPanel> FindOptionPanels()
        {
            return this.Controls
                .Cast<Control>()
                .OfType<IOptionPanel>();
        }

        private void OptionDialog_FormClosed(object sender, FormClosedEventArgs e)
        {
            settings.ConfigurationChanged -= SettingsConfigFileReloaded;
        }
    }
}