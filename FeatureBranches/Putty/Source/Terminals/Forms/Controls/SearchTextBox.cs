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
        private readonly AutoCompleteStringCollection autocompleteSource = new AutoCompleteStringCollection();
        private readonly System.Threading.Timer timer;
        private bool alreadyFocused;

        /// <summary>
        /// Gets or sets current collection of searched texts used as textbox AutoCompleteSource
        /// </summary>
        internal string[] SearchedTexts
        {
            get
            {
                var current = new string[this.autocompleteSource.Count];
                this.autocompleteSource.CopyTo(current, 0);
                return current;
            }
            set
            {
                this.autocompleteSource.Clear();
                this.autocompleteSource.AddRange(value);
            }
        }

        /// <summary>
        /// Gets value of the text to search entered by the user in the text box.
        /// </summary>
        internal string SearchText
        {
            get
            {
                return this.valueTextBox.Text;
            }
        }

        /// <summary>
        /// Informs, that user requests new search by changing the text to search or press enter key,
        /// or click on search button. Event is delayed by 250 ms, but fired in GUI thread.
        /// </summary>
        public event EventHandler<SearchEventArgs> Start;

        /// <summary>
        /// User requests to cancel currently running search by click on cancel button or by press of Escape key
        /// </summary>
        public event EventHandler Cancel;

        public SearchTextBox()
        {
            this.InitializeComponent();

            this.ConfigureAutoComplete();
            this.timer = new System.Threading.Timer(c => this.StartSearch(), null, Timeout.Infinite, Timeout.Infinite);
            this.valueTextBox.GotFocus += this.ValueTextBox_GotFocus;
        }

        private void ConfigureAutoComplete()
        {
            this.valueTextBox.AutoCompleteCustomSource = this.autocompleteSource;
            this.valueTextBox.AutoCompleteMode = AutoCompleteMode.Suggest;
            this.valueTextBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
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
            if (string.IsNullOrEmpty(this.SearchText))
                return;

            this.isSearching = true;
            this.AppendAutoComplete(this.SearchText);
            this.searchButton.Image = Resources.escape;
            this.FireStart();
        }

        private void AppendAutoComplete(string searchText)
        {
            if (!this.autocompleteSource.Contains(searchText))
                this.autocompleteSource.Add(searchText);
        }

        private void FireStart()
        {
            if (this.Start != null)
            {
                var args = new SearchEventArgs(this.SearchText);
                this.Start(this, args);
            }
        }

        private void CancelSearch()
        {
            this.isSearching = false;
            this.searchButton.Image = Resources.search_glyph;
            this.UpdateTextValue();
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

        private void ValueTextBox_Leave(object sender, EventArgs e)
        {
            this.alreadyFocused = false;
        }

        private void ValueTextBox_GotFocus(object sender, EventArgs e)
        {
            if (MouseButtons != MouseButtons.None || this.alreadyFocused)
                return;
            this.valueTextBox.SelectAll();
            this.alreadyFocused = true;
        }

        private void ValueTextBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (this.alreadyFocused || this.valueTextBox.SelectionLength != 0)
                return;
            this.alreadyFocused = true;
            this.valueTextBox.SelectAll();
        }
    }
}
