using System;

namespace Terminals
{
    /// <summary>
    /// One tool strip configuration. Used to backup and restore the window layout.
    /// </summary>
    public class ToolStripSetting
    {
        public String Name { get; set; }
        public Boolean Visible { get; set; }
        public Int32 Row { get; set; }
        public String Dock { get; set; }
        public Int32 Left { get; set; }
        public Int32 Top { get; set; }

        public override String ToString()
        {
            return String.Format("ToolStripSetting:Name={0},Visible={1},Row={2},Position=[{3},{4}],Dock={5}",
                                    this.Name, this.Visible, this.Row, this.Left, this.Top, this.Dock);
        }
    }
}
