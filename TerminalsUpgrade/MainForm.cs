using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;

namespace TerminalsUpgrade
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (File.Exists(txtConfig.Text))
            {
                if (MessageBox.Show("Are you sure that you want to upgrade the config file? Did you backup the file?",
                    "Terminal Upgrade", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.Load(txtConfig.Text);
                    XmlNode favoritesButtonsXmlNode = xmlDocument.SelectSingleNode("//configuration/settings/favoritesButtonsList");
                    if (favoritesButtonsXmlNode == null)
                    {
                        XmlNode buttonsXmlNode = xmlDocument.CreateNode(XmlNodeType.Element, "favoritesButtonsList", "");
                        XmlNodeList addXmlNodeList = xmlDocument.SelectNodes("//configuration/settings/favorites/add");
                        foreach (XmlNode addXmlNode in addXmlNodeList)
                        {
                            if (addXmlNode.Attributes["showOnToolbar"].Value == "true")
                            {
                                XmlNode addButtonXmlNode = xmlDocument.CreateNode(XmlNodeType.Element, "add", "");
                                XmlAttribute xmlAttribute = xmlDocument.CreateAttribute("name");
                                xmlAttribute.Value = addXmlNode.Attributes["name"].Value;
                                addButtonXmlNode.Attributes.Append(xmlAttribute);
                                buttonsXmlNode.AppendChild(addButtonXmlNode);
                            }
                            XmlAttribute deletedAttribute = addXmlNode.Attributes["showOnToolbar"];
                            if (deletedAttribute != null)
                            {
                                addXmlNode.Attributes.Remove(deletedAttribute);
                            }
                        }
                        XmlNode settingsXmlNode = xmlDocument.SelectSingleNode("//configuration/settings");
                        settingsXmlNode.AppendChild(buttonsXmlNode);
                        xmlDocument.Save(txtConfig.Text);
                        MessageBox.Show("Upgrade finished.", "Terminals Upgrade", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("No need to upgrade.", "Terminals Upgrade", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            else
            {
                MessageBox.Show("Cannot find the file.", "Terminals Upgrade", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                txtConfig.Text = openFileDialog.FileName;
            }
        }
    }
}