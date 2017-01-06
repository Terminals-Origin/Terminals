using System;
using System.IO;
using Terminals.Configuration;
using Unified;

namespace Terminals
{
    /// <summary>
    /// Dictionary settings for tool strips in main window, used to backup and restore their posistion
    /// </summary>
    public class ToolStripSettings : SerializableSortedDictionary<int, ToolStripSetting>
    {
        internal static ToolStripSettings Load()
        {
            string fullFileName = FileLocations.ToolStripsFullFileName;
            if (File.Exists(fullFileName))
            {
                String xmlContent = File.ReadAllText(fullFileName);
                return LoadFromXmlString(xmlContent);
            }
            return null;
        }

        /// <summary>
        /// Doesnt handle file write exceptions.
        /// </summary>
        internal void Save()
        {
            File.WriteAllText(FileLocations.ToolStripsFullFileName, this.ToXmlString());
        }

        private static ToolStripSettings LoadFromXmlString(String xmlContent)
        {
            return (ToolStripSettings)Serialize.DeSerializeXML(xmlContent, typeof(ToolStripSettings), false);
        }

        private string ToXmlString()
        {
            string val = "";
            using (MemoryStream stm = Serialize.SerializeXML(this, typeof(ToolStripSettings), false))
            {
                if (stm != null)
                {
                    if (stm.CanSeek && stm.Position > 0)
                        stm.Position = 0;

                    using (StreamReader sr = new StreamReader(stm))
                    {
                        val = sr.ReadToEnd();
                    }
                }
            }

            return val;
        }
    }
}
