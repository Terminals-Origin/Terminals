using System.Windows.Forms;
using Terminals.Data;

namespace Terminals.Connections
{
    internal class Connection : Control, IConnection
    {
        public delegate void LogHandler(string entry);

        public event LogHandler OnLog;

        public delegate void Disconnected(Connection connection);

        public event Disconnected OnDisconnected;

        public string LastError { get; protected set; }

        public virtual bool Connected 
        {
            get; private set;
        }

        /// <summary>
        /// Gets or sets the associated favorite.
        /// If the connection is virtual, it doesnt have any favorite, so it can be null.
        /// </summary>
        public IFavorite Favorite { get; set; }

        public IConnectionMainView ParentForm { get; set; }

        /// <summary>
        /// Create this control doesnt mean to open the connection.
        /// Use explicit call instead. Because there may be related resources, 
        /// call Dispose to close the connection to prevent memory leak.
        /// </summary>
        public virtual bool Connect()
        {
            return true;
        }

        protected void Log(string text)
        {
            if (this.OnLog != null)
                this.OnLog(text);
        }

        /// <summary>
        /// Default empty implementation to be overriden by connection
        /// </summary>
        public virtual void ChangeDesktopSize(DesktopSize size)
        {
        }

        /// <summary>
        /// Avoid to call remove the tab control from connection.
        /// Instead we are going to fire event to the MainForm, which will do for us:
        /// - remove the tab
        /// - Close the connection, when necessary
        /// - and finally the expected Dispose of Disposable resources 
        /// After that the connection wouldnt need reference to the TabControl and MainForm.
        /// </summary>
        protected void FireDisconnected()
        {
            if (this.OnDisconnected != null)
                this.OnDisconnected(this);
        }
    }
}
