using System;
using System.Text;

namespace Terminals
{
    public class ToolStripSetting
    {
        private string _name;
        private string _dock;
        private int _left;
        private int _top;
        private bool _visible;
        private int _row;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public bool Visible
        {
            get { return _visible; }
            set { _visible = value; }
        }

        public int Row
        {
            get { return _row; }
            set { _row = value; }
        }

        public string Dock
        {
            get { return _dock; }
            set { _dock = value; }
        }

        public int Left
        {
            get { return _left; }
            set { _left = value; }
        }

        public int Top
        {
            get { return _top; }
            set { _top = value; }
        }
    }
}
