# How to manage connections?

## Create or open connection 
The main feature of this application is to provide easy access to different remote control servers. To handle many different services across all your computers you some how have to identify your connections a you will need to reconnect without searching for server name or port. With Terminals you store your connections by connection name. When creating new connection the only thing you have to keep in mind is, how to name the connection configuration. You can open new connection by:
* Clicking on green "Plus" (New connection) button on the Toolbar
* From file menu select "New connection"

To open existing connection type the connection name into the "Connect to" text box.
If the "New connection configuration" window appears,
# Select the required service from Protocol drop down list. Changing the service protocol, the window can switch to different tab page depending on the configuration associate with the selected service.
# Write IP address or computer name into the "Computer" text box
# If your service uses not standard port, fill the "Port" text box also, otherwise default port will be used
# And now is time to name your configuration in connection name text box.
# Click on the OK button to connect

You can later change to configuration by right click on the connection and select Properties.
If there already is connection with the same name you typed into the "Connection name" text box, you will overwrite that connection settings.
For HTTP, the computer name left blank and type the connection address into the "URL" text box on HTTP tab page.
To close opened connection close its window or from "Terminal" menu select "Disconnect" command

To use advanced favorites management continue to [Organize favorites](Organize-favorites.md)
Tip: Note, that changed connection properties immediately save the changes into the configuration file and there is no possibility to do an Undo. 

## Check why i can't connect 
Because there are different protocols with different reasons, why the connection couldn't be established, you usually wouldn't be able to identify, whats the problem.
So, you will see only a poor error message telling you to check the log file.
The log file is stored under the application Logs directory as currentlog.txt.
Here are some tips, what to check, if you aren't able to connect:
* Check the general network setting: firewall, service ports configuration, authentication method used on both sides, if your account isn't locked, that you typed the password properly etc.
* And now the SSH: This protocol is able to use different authentication methods. If you configure the SSH client and the server isn't satisfied, than the authentication fails. Our protocol ssh implementation isn't able to use Host authentication and requires, that you type the password into the configuration for Password authentication,  Otherwise the connection is forced to use keyboard interactive authentication (you type the password into the terminal console window prompt) and server doesn't have to support this type of authentication (e.g. the Linux based wifi access points).
* ICA Citrix: the client is version 2.4, which is actually an old version. We plan to upgrade it in next version. It means, that you will be able to connect only to old severs.

For troubles with Firewall , here are default service ports:
||Service name||Default port number||
|Remote desktop | 3389|
|VNC,VMRC| 5900|
|Telnet | 23|
|SSH|22|
|ICA | 1494|
|HTTP| 80|
|HTTPS| 443|

NOTE: In a case of "**a website is trying to start a remote connection**" warning message, install the KB2592687 Windows update for RDP 8.0 to remove the warning.

## Move opened connection to the other monitor
Usually the connection opens in tab page inside the main window.  If you are using multimonitor environment, it can be helpful to use different connections on each monitor. 
* To detach the active connection from the main window select "View" > "View in new window".
* To attach the selected window back to the Terminals main window, click on the "Attach to main window" button. 
In connection properties window, on general tab page, there is also a check box, where you can explicitly define, that you want to open this connection in new window.
When resizing the window, the control is usually configured to scale up to maximum initial window size (at connection startup). 
Additionally you can also enlarge the window into the full screen mode. To do so, select "View" > "Full screen" or press F11 key. To leave the fullscreen press F11 again.