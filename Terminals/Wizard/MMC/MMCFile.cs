using System;
using System.Collections.Generic;
using System.Text;

namespace Terminals.Wizard.MMC
{
    public class MMCFile
    {
        System.IO.FileInfo mmcFileInfo;
        string rawContents;
        public string Name;
        public bool Parsed = false;
        public System.Drawing.Icon SmallIcon;

        public MMCFile(System.IO.FileInfo MMCFile)
        {
            mmcFileInfo = MMCFile;
            Parse();
        }
        public MMCFile(string MMCFile)
        {
            if (System.IO.File.Exists(MMCFile))
            {
                mmcFileInfo = new System.IO.FileInfo(MMCFile);
                Parse();
            }
        }
        protected void Parse()
        {
            rawContents = System.IO.File.ReadAllText(mmcFileInfo.FullName, Encoding.Default);
            if (rawContents != null && rawContents.Trim() != "" && rawContents.StartsWith("<?xml"))
            {
                System.Xml.XmlDocument xDoc = new System.Xml.XmlDocument();
                xDoc.LoadXml(rawContents);
                System.Xml.XmlNode node = xDoc.SelectSingleNode("/MMC_ConsoleFile/StringTables/StringTable/Strings");
                foreach (System.Xml.XmlNode cNode in node.ChildNodes)
                {
                    string name = cNode.InnerText;
                    if (name != "Favorites" && name != "Console Root")
                    {
                        Name = name;
                        Parsed = true;
                        break;
                    }
                }


                //System.Xml.XmlNode binarynode = xDoc.SelectSingleNode("/MMC_ConsoleFile/BinaryStorage");
                //foreach (System.Xml.XmlNode child in binarynode.ChildNodes)
                //{
                //    string childname = child.Attributes["Name"].Value;
                //    if (childname.ToLower().Contains("small"))
                //    {
                //        string image = child.InnerText;
                //        byte[] buff = System.Convert.FromBase64String(child.InnerText.Trim());
                //        System.IO.MemoryStream stm = new System.IO.MemoryStream(buff);
                //        if (stm.Position > 0 && stm.CanSeek) stm.Seek(0, System.IO.SeekOrigin.Begin);
                //        System.IO.File.WriteAllBytes(@"C:\Users\Administrator\Desktop\foo.ico", buff);
                //        System.Drawing.Icon ico = new System.Drawing.Icon(stm);

                //    }
                //}


                System.Xml.XmlNode visual = xDoc.SelectSingleNode("/MMC_ConsoleFile/VisualAttributes/Icon");
                if (visual != null)
                {
                    string iconFile = visual.Attributes["File"].Value;
                    int index = Convert.ToInt32(visual.Attributes["Index"].Value);
                    System.Drawing.Icon[] icons = IconHandler.IconHandler.IconsFromFile(iconFile, IconHandler.IconSize.Small);
                    if (icons!=null && icons.Length > 0)
                    {
                        if (icons.Length > index)
                        {
                            SmallIcon = icons[index];
                        }
                        else
                        {
                            SmallIcon = icons[0];
                        }
                    } 
                }

            }
        }
    }
}
