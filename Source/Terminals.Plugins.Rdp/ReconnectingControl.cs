using System;
using System.Windows.Forms;

namespace Terminals.Forms.Controls
{
    /// <summary>
    /// Panel with rotating progress bar to present the reconnecting procedure.
    /// Contains abort button to be able to request the reconnect cancelation.
    /// </summary>
    internal partial class ReconnectingControl : UserControl
    {
        /// <summary>
        /// Fired when user clicks on abort button
        /// </summary>
        internal event EventHandler AbortReconnectRequested;

        /// <summary>
        /// Gets or sets the state of the "Reconnect when ready" checkbox.
        /// </summary>
        internal bool Reconnect 
        {
            get { return this.reconnectCheckBox.Checked; }
            set { this.reconnectCheckBox.Checked = value; }
        }

        /// <summary>
        /// Gets a flag identifiyng, that the user wants to disable this feature
        /// </summary>
        internal bool Disable
        {
            get { return this.dotAskCheckBox.Checked; }
        }

        private Control oldParent;

        public ReconnectingControl()
        {
            InitializeComponent();
        }

        private void AbortButtonClick(object sender, EventArgs e)
        {
            if (AbortReconnectRequested != null)
                AbortReconnectRequested(this, EventArgs.Empty);
        }

        protected override void OnParentChanged(EventArgs e)
        {
            if (this.oldParent != null)
                this.oldParent.Resize -= Parent_Resize;
            
            if (this.Parent == null)
                return;

            this.oldParent = this.Parent;
            this.oldParent.Resize += new EventHandler(Parent_Resize);
            this.CenterInParent();
            base.OnParentChanged(e);
        }

        private void Parent_Resize(object sender, EventArgs e)
        {
            this.CenterInParent();
        }

        private void CenterInParent()
        {
            var parentSize = this.Parent.Size;
            this.Left = parentSize.Width / 2 - this.Width / 2;
            this.Top = parentSize.Height / 2 - this.Height / 2;
        }
    }
}
