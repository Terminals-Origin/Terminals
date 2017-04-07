# How to backup and restore settings files

Like other applications Terminals stores its data in data files on your disk. All these files are stored under application directory except files, where explicit path is mentioned. Following chapters list all files explaining their content and meaning. Terminals doesn't store any data in Windows registry, so you only have to backup the application directory. This also allows use Terminals as portable application, but require run it under elevated privileges on Windows Vista and later OS version.
Any changes are immediately confirmed into the files. Passwords saved in all files are always written in encrypted form.
To reset configuration or restore to previous state simply close Terminals and replace or remove required file. 

**It is strongly recommended, that you backup these files before you upgrade to new version. **

## Data files

When discussing application data, we are talking about values, you create using Terminals like Favorites, Credentials, Tags and history.
* **History.xml** - Stores content visible in History pane. This content is updated whenever you connect to some favorite. Removing this file will clear the history.
* **Credentials.xml** - contains values of stored credentials.
* **Terminals.config** - This is the main Terminals data file. Here Tags, Favorites and application application options are saved. Removing  this will clear all your favorites and next time you start terminals, first run wizard will appear.

## Window layout files

Layout files hold only positions of main window and tool bars. When you encounter any problem with window position or tool bars, you can remove this files to reset the window layout.
* **ToolStrip.settings.config** - Main window tool bar positions
* **user.config** - Not used since version 3.0. Was stored in user profile (usually in "c:\Documents and Settings\<user_name>\Local Settings\Application Data\Robert_Chartier\<Terminals>\<Version>\"). Contains main window position and size.

## Log files

* **Terminals.log4net.config** - logging configuration. No one usually changes content of this file.
* **currentlog.txt** - located "Logs" subdirectory in application folder. All problems are noticed in this files. If you want to report any issue, please provide this file as attachment.
* **LastUpdateCheck.txt** - holds only date of last update. This file isn't used yet.

## Version 3.0:
* Has different files structure depending on installation type, which is described in [Upgrade to version 3.0](Upgrade-to-version-3.0.md)*
* All user files of the application are stored in one "**Data**" directory. This make the application 100% portable.
* All other rules are still valid except:
	* Content of the files is not backward compatible with version 2.0 and older
	* Stronger encryption is used and also additional fields in data files are encrypted (Domain and user name wasn't encrypted till version 3.0)
	* New file was added: **Favorites.xml** - contains all groups and their favorites. This version no longer stored favorites and their groups (Tags) in config file.
	* User.config file is not  used. Currently window positions were moved to the Terminals.config