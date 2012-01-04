using System;
using System.Windows.Forms;
using Terminals.Converters;
using Terminals.Data;

namespace Terminals
{
    internal partial class ConsolePreferences : UserControl
    {
        public ConsolePreferences()
        {
            InitializeComponent();
        }

        public void FillControls(IFavorite favorite)
        {
            var consoleOptions = favorite.ProtocolProperties as ConsoleOptions;
            if (consoleOptions == null)
                return;
            BackColorTextBox.Text = consoleOptions.BackColor;
            FontTextbox.Text = consoleOptions.Font;
            CursorColorTextBox.Text = consoleOptions.CursorColor;
            TextColorTextBox.Text = consoleOptions.TextColor;
            ColumnsTextBox.Text = consoleOptions.Columns.ToString();
            RowsTextBox.Text = consoleOptions.Rows.ToString();
        }

        public void FillFavorite(IFavorite favorite)
        {
            var consoleOptions = favorite.ProtocolProperties as ConsoleOptions;
            if (consoleOptions == null)
                return;
            consoleOptions.BackColor = BackColorTextBox.Text;
            consoleOptions.Font = FontTextbox.Text;
            consoleOptions.CursorColor = CursorColorTextBox.Text;
            consoleOptions.TextColor = TextColorTextBox.Text;
            consoleOptions.Columns = Convert.ToInt32(ColumnsTextBox.Text);
            consoleOptions.Rows = Convert.ToInt32(RowsTextBox.Text);
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
