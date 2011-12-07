using System;
using System.Windows.Forms;
using Terminals.Converters;

namespace Terminals
{
    internal partial class ConsolePreferences : UserControl
    {
        public ConsolePreferences()
        {
            InitializeComponent();
        }

        public void FillControls(FavoriteConfigurationElement favorite)
        {
            BackColorTextBox.Text = favorite.ConsoleBackColor;
            FontTextbox.Text = favorite.ConsoleFont;
            CursorColorTextBox.Text = favorite.ConsoleCursorColor;
            TextColorTextBox.Text = favorite.ConsoleTextColor;
            ColumnsTextBox.Text = favorite.ConsoleCols.ToString();
            RowsTextBox.Text = favorite.ConsoleRows.ToString();
        }

        public void FillFavorite(FavoriteConfigurationElement favorite)
        {
            favorite.ConsoleBackColor = BackColorTextBox.Text;
            favorite.ConsoleFont = FontTextbox.Text;
            favorite.ConsoleCursorColor = CursorColorTextBox.Text;
            favorite.ConsoleTextColor = TextColorTextBox.Text;
            favorite.ConsoleCols = Convert.ToInt32(ColumnsTextBox.Text);
            favorite.ConsoleRows = Convert.ToInt32(RowsTextBox.Text);
        }

        private void FontButton_Click(object sender, EventArgs e)
        {
            this.fontDialog1.Font = FontParser.FromString(FontTextbox.Text);
            DialogResult result = this.fontDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                FontTextbox.Text = FontParser.ToString(this.fontDialog1.Font);
            }
        }

        private void BackcolorButton_Click(object sender, EventArgs e)
        {
            this.colorDialog1.Color = ColorParser.FromString(BackColorTextBox.Text);
            DialogResult result = this.colorDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                BackColorTextBox.Text = ColorParser.ToString(this.colorDialog1.Color);
            }
        }

        private void TextColorButton_Click(object sender, EventArgs e)
        {
            this.colorDialog1.Color = ColorParser.FromString(TextColorTextBox.Text);
            DialogResult result = this.colorDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                TextColorTextBox.Text = ColorParser.ToString(this.colorDialog1.Color);
            }
        }

        private void CursorColorButton_Click(object sender, EventArgs e)
        {
            this.colorDialog1.Color = ColorParser.FromString(CursorColorTextBox.Text);
            DialogResult result = this.colorDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                CursorColorTextBox.Text = ColorParser.ToString(this.colorDialog1.Color);
            }
        }
    }
}
