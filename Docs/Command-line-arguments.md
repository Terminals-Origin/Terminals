# Terminals Command line arguments

To customize terminals like other applications, terminals accepts command line arguments at startup. All arguments have to start with "/" or "-". Even if you provide parameters from command line, the application will start its window. If application isn't able to parse parameters you provided, it starts like without them. Order of arguments doesn't meter, but arguments are case sensitive. Paths have to be encapsulated in quotation marks if they contain spaces.
All switches, which target file location can point to the UNC path and the file name has to respect the default file name.

NOTE: "favsfile" and "cred" switches are new in version 3.0 CTP and aren't present in previous versions.

## List of available switches

| Parameter | Shortcut | Description |
|---|---|---|
| url | - | URL to quick connect to. Only TRM protocl. The "TRM" prefix has to be provided. |
| config | - | Local path the the config file to use.  Defaults to the standard Terminals.config. |
| favsfile | ff | Path to the favorites and groups file to use. Defaults to the standard favorites.xml. 
| cred | - | Path to the credentials file to use. Defaults to the standard Credentials.xml. |
| favs | - | Commma separated Favorite names to quick connect to. Spaces in favorite names aren't supported. |
| console | c | Connect to the console. |
| machine | v | Quick connect to machine via RDP on default port  (this switch matches mstsc.exe's parameter). |
| fullscreen | f | Run terminals in Full Screen mode at startup. |
| AutomaticallyUpdate | au | Enable Automatic Updates. Checks for updates during application start. |
| reuse | r | Force single instance application, even if the option isn't set in application options. |


## Examples
* Connect to a Single machine:

```
terminals /url:trm://server
terminals /v:server
```

* Connect to machines stored in favorites named "server" and "server1" at once:

```
terminals /favs:server,server1
```

* Connect to console on "server" computer:

```
terminals /v:server /c
```

* Connect to Single, Console, Fullsceen on "server" computer:

```
terminals /v:server /c /f
```

* When starting Terminals use file stored at \\serverShare\terminalsFiles\terminals.config:

```
terminals /config:\\serverShare\terminalsFiles\terminals.config
```
