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
                Name = node.ChildNodes[0].InnerText;
                Parsed = true;
            }
        }
    }
}
