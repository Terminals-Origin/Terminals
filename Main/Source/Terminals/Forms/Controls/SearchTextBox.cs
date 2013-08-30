using System;
using System.Threading;
using System.Windows.Forms;
using Terminals.Properties;

namespace Terminals.Forms.Controls
{
    /// <summary>
    /// User control to simulate delayed search.
    /// </summary>
    internal partial class SearchTextBox : UserControl
    {
        private bool isSearching;
        private bool changingState;
        private readonly System.Threading.Timer timer;

        /// <summary>
        /// Informs, that user requests new search by changing the text to search or press enter key,
        /// or click on search button. Event is delayed by 250 ms, but fired in GUI thread.
        /// </summary>
        internal event EventHandler<SearchEventArgs> Start;

        /// <summary>
        /// User requests to cancel currently running search by click on cancel button or by press of Escape key
        /// </summary>
        internal event EventHandler Cancel;

        public SearchTextBox()
        {
            this.InitializeComponent();
            this.timer = new System.Threading.Timer(c => this.StartSearch(), null, Timeout.Infinite, Timeout.Infinite);
        }

        private void ValueTextBoxTextChanged(object sender, EventArgs e)
        {
            if (!this.changingState)
                this.ScheduleSearch();
        }

        private void SearchButtonClick(object sender, EventArgs e)
        {
            if (this.isSearching)
                this.CancelSearch();
            else
                this.ScheduleSearch();
        }

        private void ValueTextBoxKeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    this.CancelSearch();
                    break;
                case Keys.Enter:
                    this.ScheduleSearch();
                    break;
            }
        }

        private void ScheduleSearch()
        {
            this.timer.Change(250, Timeout.Infinite);
        }

        private void StartSearch()
        {
            if (this.InvokeRequired)
                this.Invoke(new Action(this.StartSearch));
            else
                this.DoSearch();
        }

        private void DoSearch()
        {
            if (string.IsNullOrEmpty(this.valueTextBox.Text))
                return;

            this.isSearching = true;
            this.searchButton.Image = Resources.escape;
            this.valueTextBox.Focus();
            this.FireStart();
        }

        private void FireStart()
        {
            if (this.Start != null)
            {
                var args = new SearchEventArgs(this.valueTextBox.Text);
                this.Start(this, args);
            }
        }

        private void CancelSearch()
        {
            this.isSearching = false;
            this.searchButton.Image = Resources.search_glyph;
            this.UpdateTextValue();
            this.valueTextBox.Focus();
            this.FireCancel();
        }

        private void UpdateTextValue()
        {
            this.changingState = true;
            this.valueTextBox.Text = string.Empty;
            this.changingState = false;
        }

        private void FireCancel()
        {
            if (this.Cancel != null)
                this.Cancel(this, EventArgs.Empty);
        }
    }
}
