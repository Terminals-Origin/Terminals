using System;
using Terminals.Data;

namespace Terminals.Connections
{
    /// <summary>
    /// The connection is closed by its explicit call to Dispose,
    /// because most connections work with unmanaged resources and we usualy dont reconnect.
    /// Instead we close the current connection and create new instance.
    /// So there is no oposit to Connect method.
    /// </summary>
    public interface IConnection : IDisposable
    {
        IFavorite Favorite { get; }

        bool Connected { get; }

        /// <summary>
        /// Create connection doesnt mean to open the connection.
        /// Use explicit call instead. Because there may be related resources, 
        /// call <see cref="IDisposable.Dispose"/> to close the connection to prevent memory leak.
        /// </summary>
        bool Connect();

        void ChangeDesktopSize(DesktopSize size);
    }
}
