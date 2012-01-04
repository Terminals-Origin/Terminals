using System;
using Terminals.Converters;

namespace Terminals.Data
{
    /// <summary>
    /// Telnet and ssh protocol console options
    /// </summary>
    [Serializable]
    public class ConsoleOptions : ICloneable
    {
        private int rows = 33;
        public int Rows
        {
            get
            {
                return rows;
            }
            set
            {
                rows = value;
            }
        }

        private int columns = 110;
        public Int32 Columns
        {
            get
            {
                return columns;
            }
            set
            {
                columns = value;
            }
        }

        private string font;
        public String Font
        {
            get
            {
                if (String.IsNullOrEmpty(this.font))
                    return FontParser.DEFAULT_FONT;

                return this.font;
            }
            set
            {
                this.font = value;
            }
        }

        private string backColor = "Black";
        public String BackColor
        {
            get
            {
                return this.backColor;
            }
            set
            {
                this.backColor = value;
            }
        }

        private string textColor = "White";
        public String TextColor
        {
            get
            {
                return this.textColor;
            }
            set
            {
                this.textColor = value;
            }
        }

        private string cursorColor = "Green";
        public String CursorColor
        {
            get { return this.cursorColor; }
            set { this.cursorColor  = value; }
        }

        internal ConsoleOptions Copy()
        {
            return new ConsoleOptions
                {
                    CursorColor = this.CursorColor,
                    BackColor = this.BackColor,
                    TextColor = this.TextColor,
                    Columns = this.Columns,
                    Rows = this.Rows,
                    Font = this.Font
                };
        }

        public object Clone()
        {
            return Copy();
        }
    }
}
