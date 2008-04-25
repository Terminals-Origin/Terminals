using System;
using System.Collections.Generic;
using System.Text;

namespace Terminals.SysInfo
{
    public enum NodeTypes { WMIQuery, CustomControl,  Other};
    [System.Serializable()]
    public class SystemInformationNode
    {
        public string NodeDisplayValue;
        public string WMIQuery;
        public string CustomControlAssembly;
        public string CustomControlType;

        public NodeTypes NodeType = NodeTypes.Other;
        public List<SystemInformationNode> Nodes = new List<SystemInformationNode>();

        public SystemInformationNode() { }
        public SystemInformationNode(string NodeDisplayValue)
        {
            this.NodeDisplayValue = NodeDisplayValue;
        }
        public SystemInformationNode(string NodeDisplayValue, string WMIQuery)
        {
            this.NodeDisplayValue = NodeDisplayValue;
            this.WMIQuery = WMIQuery;
            this.NodeType = NodeTypes.WMIQuery;
        }
        public SystemInformationNode(string NodeDisplayValue, string CustomControlAssembly, string CustomControlType)
        {
            this.NodeDisplayValue = NodeDisplayValue;
            this.CustomControlAssembly = CustomControlAssembly;
            this.CustomControlType = CustomControlType;
        }
        public static SystemInformationNode LoadRoot()
        {
            return LoadRoot("Sysinfo.xml");
        }
        public static SystemInformationNode LoadRoot(string Filename)
        {
            if(!System.IO.File.Exists(Filename)) return BuildBasicRoot();
            string xmlFile = System.IO.File.ReadAllText(Filename);
            if(xmlFile == null || xmlFile.Trim() == "") return BuildBasicRoot();
            try
            {
                return (SystemInformationNode)Unified.Serialize.DeSerializeXML(xmlFile, typeof(SystemInformationNode));
            }
            catch(Exception exc)
            {
                Terminals.Logging.Log.Error("Failed to Load SystemInformationNode Root", exc);
            }
            return BuildBasicRoot();
        }
        public static SystemInformationNode BuildBasicRoot()
        {
            SystemInformationNode root = new SystemInformationNode("System Information");
            root.Nodes.Add(new SystemInformationNode("Operating System", "select * from CIM_OperatingSystem"));
            root.Nodes.Add(new SystemInformationNode("Windows Product Activation", "select * from Win32_WindowsProductActivation"));
            
            return root;
        }
        public object Execute()
        {
            object result = null;
            switch(this.NodeType)
            {
                case NodeTypes.WMIQuery:
                    result = WMITestClient.WMIControl.WMIToDataTable(this.WMIQuery, "\\localhost", "", "");
                    break;
                case NodeTypes.CustomControl:
                    break;
                case NodeTypes.Other:
                    break;
                default:
                    break;
            }
            return result;
        }
        public static void SaveRoot(SystemInformationNode Node, string Filename)
        {
            if(System.IO.File.Exists(Filename)) System.IO.File.Delete(Filename);
            using(System.IO.MemoryStream stm = Unified.Serialize.SerializeXML(Node, typeof(SystemInformationNode)))
            {
                if(stm != null)
                {
                    byte[] data = new byte[(int)stm.Length];
                    stm.Read(data, 0, (int)stm.Length);
                    System.IO.File.WriteAllBytes(Filename, data);
                    if(stm.CanSeek && stm.Position > 0) stm.Position = 0;
                }
            }
        }
    }
}