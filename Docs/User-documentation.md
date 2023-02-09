# User documentation

Well, on the beginning it was a small application...
But anyway, don't search here for SSH specification, or how to enable remote desktop for your account on Windows server. Simply we have to expect, that you know, how to use required service. It is out of scope of this documentation to describe, how network protocols work or how to configure them. And because each implementation of the same service may differ, here you can find only information, how to use and configure Terminals as a service client and what are its specifics.

## Key application terms

* **Connection** - session between computer, which offers remote control, and your Terminals application
* **Terminal** - text window, which can be used for not GUI remote control protocols like Telnet or SSH
* **Favorite** - stored configuration, which allows you to connect to one service on one server using one user login
* **Tag** - text associated with one or more favorites, which allows you to group your favorites into logical groups visible in tree.
* **Group** - set of favorites used for batch connection operations. Since version 3.0 only Groups and Tags are used in the same meaning
* **Shortcut** - stored configuration, which allows you to start script or application directly from Terminals application. By default Terminals comes with shortcuts to Microsoft management console and Control panel applets
* **Capture** - stored screen picture of your running connection
* **Protocol** - type of remote control service offered by server you want to connect to
* **Credentials** - authentication information (Domain, user name and password). Note, that you don't have to store your passwords. For more details check the documentation.

Note: New version 3.0 CTP doesn't distinguish between Tag and group

## How to

* Installation
  * [System requirements](./System-Requirements.md)
  * [Upgrade to version 3.0](./Upgrade-to-version-3.0.md)
* [Features and Screen shots](./Features-and-Screen-shots.md)
* [Developer guide](./Developer-guide.md) (How to contribute)
* [Road map](./Road-map.md)
* [Manage connections](./Manage-connections.md)
  * Create connection
  * Check why i can't connect
  * Move connection to the other monitor
  * [Configure connection share](./Configure-connection-share.md)
  * [Powershell script to create import file](./Powershell-script-to-create-import-file.md)
* [Manipulate with connection credentials](./Manipulate-with-connection-credentials.md)
  * Define custom connection credentials
  * Reuse stored credentials for more connections
  * Protect my connections with master password
* [Organize favorites](./Organize-favorites.md)
  * Manage Tags and Groups
  * Import favorites
  * Search for computers servicing remote control protocol
* [Capture connection screen](./Capture-connection-screen.md)
* Configure application data
  * [Backup and restore settings files](./Backup-and-restore-settings-files.md)
  * [Store data in SQL database](./Store-data-in-SQL-database.md)
* Start Terminals using [Command line arguments](./Command-line-arguments.md)
* Select proper [Application Options](./Application-Options.md)
* [Advanced usage](./Advanced-usage.md)
