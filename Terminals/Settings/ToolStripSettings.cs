﻿using System;
using System.IO;
using Unified;

namespace Terminals
{
    /// <summary>
    /// Dictionary settings for tool strips in main window, used to backup and restore their posistion
    /// </summary>
    public class ToolStripSettings : SerializableSortedDictionary<int, ToolStripSetting>
    {
        private static string ToolStripSettingsFile = "ToolStrip.settings.config";

        internal static ToolStripSettings Load()
        {
            if (File.Exists(ToolStripSettingsFile))
            {
                String xmlContent = File.ReadAllText(ToolStripSettingsFile);
                return LoadFromXmlString(xmlContent);
            }
            return null;
        }

        internal void Save()
        {
            File.WriteAllText(ToolStripSettingsFile, this.ToXmlString());
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
