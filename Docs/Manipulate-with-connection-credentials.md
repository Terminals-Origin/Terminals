# How to manipulate with connection credentials
If you are connecting to many servers, it can help to don't type user name and password (credentials) every time you open a connection. For this reason, Terminals allow you to store user names and passwords.
* To define stored credentials you can open "Credential manager" window from "Tools" menu.
* Or if you are currenty editing connection, you can open Credential manager by click on icon close to the "Credential" combo box.

## Reuse stored credentials for more connections 
Stored credentials can be shared between connections. When opening a connection (which uses stored credentials), Terminals takes stored user name and password and uses them to open the session. To assign credential to a connection, open its properties and select one from "Credential" combox. If you later change assigned credential properties (eg. password), the newly set values will be applied to all related connections next time you try to connect. To remove stored credential in connection properties, select "(custom)" value in "Credential" combo box.

## Define custom connection credentials
Even if no stored credential is selected in connection properties, you can store user name and password by selecting "Save password" check box. In this case credentials aren't saved in credential manager and you can't share them across connections. User name and domain are always saved in connection settings.
There is also possibility to define default credentials. These are used always, when new connection is created and you don't type custom values for user name and domain. To define default credentials go to "Tools" > "Options" and fill text boxes under "Security" > "Default password".

## Protect my connections with master password
Because Terminals stores credentials on your disk, there is always possibility, that someone else can use your credentials. To improve the application security, you can define "master password" to protect your credentials at once. If this password is defined, you can't start Terminals application without it. To define this password go to "Tools" > "Options" and navigate to the "Security" > "Master password". You will be prompted to type the password twice. If confirmation doesn't match, you will be informed. Next time you Terminals application starts, you have to provide the master password. To remove the master password, go to the same dialog and use "Clear master password" button.