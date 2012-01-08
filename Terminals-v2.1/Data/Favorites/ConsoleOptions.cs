using System;
using Terminals.Converters;

namespace Terminals.Data
{
    /// <summary>
    /// Telnet and ssh protocol console options
    /// </summary>
    [Serializable]
    public class ConsoleOptions : ProtocolOptions
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

        internal override ProtocolOptions Copy()
        {
            return Copy2();
        }

        internal ConsoleOptions Copy2()
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

        internal override void FromCofigFavorite(FavoriteConfigurationElement favorite)
        {
            this.BackColor = favorite.ConsoleBackColor;
            this.TextColor = favorite.ConsoleTextColor;
            this.CursorColor = favorite.ConsoleCursorColor;
            this.Columns = favorite.ConsoleCols;
            this.Rows = favorite.ConsoleRows;
            this.Font = favorite.ConsoleFont;
        }

        internal override void ToConfigFavorite(FavoriteConfigurationElement favorite)
        {
            favorite.ConsoleBackColor = this.BackColor;
            favorite.ConsoleTextColor = this.TextColor;
            favorite.ConsoleCursorColor = this.CursorColor;
            favorite.ConsoleCols = this.Columns;
            favorite.ConsoleRows = this.Rows;
            favorite.ConsoleFont = this.Font;
        }
    }
}
