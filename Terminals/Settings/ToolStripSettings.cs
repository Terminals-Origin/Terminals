using System;
using System.IO;
using System.Text;

namespace Terminals
{
    public class ToolStripSettings : SerializableSortedDictionary<int, ToolStripSetting>
    {
        public ToolStripSettings() { }

        public static ToolStripSettings LoadFromString(string Settings)
        {
            return (ToolStripSettings)Unified.Serialize.DeSerializeXML(Settings, typeof(ToolStripSettings), false);
        }

        public override string ToString()
        {
            string val = "";
            using (MemoryStream stm = Unified.Serialize.SerializeXML(this, typeof(ToolStripSettings), false))
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
