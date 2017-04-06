# Upgrade to version 3.0
This page describes installation scenarios when upgrading from previous versions to version 3.0 using setup and by using new binaries. Purpose of this page is to allow downgrade and specially help when changing between installation types. Notes to portable setup are also relevant to previous version. Purpose of this page is to prepare for an upgrade and specially help when changing between installation types. Notes to portable setup are also relevant to previous versions.

NOTE: Always backup all your files before doing an upgrade!

## Setup types
During installation there is a new page asking you to choose install scenario. In general this lets you choose, where your data files will be stored. You will have two options:
* **Local installation**: General type of setup known from other applications. All application configuration files are stored in user profile. This is new option not present in previous versions. In this case application doesn't write any files outside user profile
* **Portable version**: Version 3.0 is 100% portable. This is identical to previous versions setups. You may notice, that some new xml files appear in the application directory. This explains fixes in wrong file locations from previous version for portable configuration.

NOTE: Using portable version will request User account control prompt, when running from directory, which needs administrator privileges.

## Replacing binaries (Installation without Windows installer)
* Replace also Log4Net.config file (Path to the logs was changed)
* You can remove "Thumbs" and "Logs" directories in the application directory. They will be recreated.
* After first run of the new version, all your data files should be moved to the "Data" subdirectory

## Switching between local install and portable version
To switch already installed version to portable or vice versa you have to:
* Change the Log4Net logs directory in Log4Net.config file
* Change the "Portable" flag in Terminals.exe.config
* Move "Data" directory in expected location

Here are valid values for each configuration:
||Property name||Portable version value||Local installation||
|Portable xml node in Terminals.exe.config|True|False|
|Log File in Log4.Net.exe.config|"\Data\logs\CurrentLog.txt""|"${USERPROFILE}\Local Settings\Application Data\Robert_Chartier\Terminals\Data\logs\CurrentLog.txt""|

## Upgrade behavior
* If your data are protected by Master password and you are not able to provide it, application will exit without upgrading your files
* There is no downgrade from version 3.0 to version 2 or earlier, because encryption algorithm of passwords is not compatible. The only options is to export favorites in import into older version. Because there is no export of Credentials, they are always lost.
* There is no upgrade of connections history. After an upgrade your history will be deleted.