using System.ComponentModel;

namespace Terminals.Data
{
    /// <summary>
    /// Data layer store of Terminals data. Handles favorites, groups, credentials and history.
    /// Provides also abstract central point to create new items in Factory, and using eventing
    /// informs listeners about changes.
    /// </summary>
    internal interface IPersistence
    {
        IFavorites Favorites { get; }
        
        IGroups Groups { get; }

        IFactory Factory { get; }

        IConnectionHistory ConnectionHistory { get; }

        ICredentials Credentials { get; }
        
        DataDispatcher Dispatcher { get; }

        /// <summary>
        /// Gets the master password authentication module
        /// </summary>
        PersistenceSecurity Security { get; }

        /// <summary>
        /// Because file watcher is created before the main form in GUI thread.
        /// This lets to fire the file system watcher events in GUI thread. 
        /// </summary>
        void AssignSynchronizationObject(ISynchronizeInvoke synchronizer);

        /// <summary>
        /// Prevents save after each change. After this call, no values are saved
        /// into persistence, until you call SaveAndFinishDelayedUpdate.
        /// This dramatically increases performance. Use this method for batch updates.
        /// </summary>
        void StartDelayedUpdate();

        /// <summary>
        /// Stops prevent write changes into persistence file and immediately writes last state.
        /// Usually the changes are saved immediately.
        /// </summary>
        void SaveAndFinishDelayedUpdate();
    }
}
