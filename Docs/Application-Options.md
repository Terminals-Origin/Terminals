# Application Options
Because application contains large number of options used to configure its behavior and it is not possible to describe the behavior directly in the option, here is list of the options and their explanations. To configure the application options go to "Tools > Options" (Shortcut {"Ctrl+Shift+P"}).

NOTE: This page is under construction!

## Startup & Shutdown
* Allow a single instance of the application: When starting Terminals it is checked, if the application is running. If running, the command line arguments are executed on the running instance. If disabled, you are able to start more than one instance of Terminals.
* Don't keep me up-to-date on Terminals project: If enabled, shows information about new releases during application start up.
* Show close confirmation dialog: When closing Terminals main window, you will be prompted, if you want to do so.
* Save connections on close: If enabled and you are closing the application, you can store list of opened connections and Terminals will try to restore them after you start the app again.

## Interface
* Show user name in tab title: If enabled, Not only the connection name, but also the user information will be shown in connection tab title
* Show connection information in tooltips: If enabled, favorite detail information (server address, port, protocol and user name) tool tip will be shown when mouse over the favorites tree or connection tab. Also when selecting connection its information is shown in application main window title.
* Show full information: Like previous option, but tool tip will contain more detailed information (Groups, Notes)
* Minimize to System Tray: If enabled, click on the application icon in the Windows task bar, Terminals will minimize (disappear) from the task bar and only its icon will be available in system tray. 
* Enable Groups menu: When disabled, removes "Groups" menu from the main window main menu.
* Theme: Defines color of the tool bars and application main menu. Choose Normal to define usual color.
* Favorites:
	* Enable Favorites panel: When disabled favorites panel (Favorites tree and History) is removed from main window
	* Auto expand (show) favorites Panel, when hidden: Favorites panel is automatically expanded whe mouse is over its collapse/expand handle.
	* Favorites sorting: Used to sort favorites in favorites tree and all menus, where favorites are listed

## Security
* Master password: You can change the application master password. This password is used to protect all passwords you store in the app. Even if not defined, your passwords are not readable to others, but are weak to be hacked. You may be prompt to provide it during application upgrades. NOTE: **Requires application restart**.
* Default password: Credentials to be used only when opening new connection and no credentials are provided by the favorite. 
* Amazon S3 Service: Synchronize your Terminals.config file only to the Cloud provided by Amazon S3 service. Use "Backup" button to upload the file or "Restore" button to replace your current file by the service store.

## Connections
* Validate Server names: When editing favorite, the "Server Name" field is validate to be valid computer name or IP address. If not valid you can't save the favorite
* Show confirm dialog on Close or warn or disconnect: If connection is lost or you are closing the connection, you are prompted, if you want to close the connection. This allows you to be informed, that the connection is lost. If not enabled and connection is lost opened connection is closed without warning (except RDP, see next option)
* Ask to reconnect when connection is lost due Shutdown or reboot: This option applies to RDP only and replaces previous option if enabled. If connection is lost a reconnecting dialog is shown and you are able to manually ask to be reconnected (when machine is available again) or close the connection. This options is helpful in a case you or someone else is restarting the server and you want to be reconnected later. Terminals tries to reconnect one hour, if server is not available, the connection is closed.
* Automatically restore the main window when the last connection is closed: Helpful, if you are using Terminals minimized to system tray and you want to continue in the app with another connection or favorite
* [Default Desktop Share](Configure-connection-share.md)
* Port scanner timeout: Number in seconds, the maximum time the networking tool "Port scanner" will try to check opened port on target machine. Range 0 - 60, default value is 5.
* Execute before connect: This options allow you to execute some application (or script) each time you open new connection. Applies to all connections and the configured command starts on your computer locally.
* Proxy: Applies only to Download manger to inform you about latest releases. If not defined, the application will use the system proxy defined for your browsers. Currently doesn't apply to the HTTP or HTTPS favorites.

## Screen capture
See also: ([How to use screen capture](Capture-connection-screen.md))
* Enable screen capture to clipboard: When you press button to capture screen the screen shot is stored into the Windows clipboard.
* Enable screen capture to folder: Like the previous option, but the picture is saved to the directory, configured as Screen capture root folder.
* Auto switch to Capture manager on capture: After taking a screen capture, Capture manager tool is automatically opened.
* Screen capture root folder: Folder used to store captured pictures and is shown as root folder when opening Capture manager. By default the capture manager points to "Terminals Captures" in your "Pictures" directory.
* Flicker: Online service which allows to synchronize and publish your pictures online. Authorize first and then you can access your service. 

## Data Store
NOTE: **Requires application restart**.
* Files in local profile (Default): All application and configuration files are stored in your user profile or application directory in case of portable version. Read more about files configuration and usage in  [Backup and restore settings files](Backup-and-restore-settings-files.md). Read more about portable version in  [Upgrade to version 3.0](Upgrade-to-version-3.0.md).
* [Microsoft SQL database server](Store-data-in-SQL-database.md)