# Screen captures
One of the maintenance helpful things is to take a picture of current session, if there happens something wrong to document such behavior. Terminals allows to take a screen shot directly to clipboard or to selected folder. This feature is called "Capture" and is disabled by default. To enable it you have to go to "Tools > Options" and under "Screen capture" select at least of the check boxes. By default the pictures are saved under your user profile "\My Pictures\Terminals Captures" directory. You can change this path also in the captures options.

## How to capture connection screen
To take a picture of your connection you can use "Tools > Capture terminal Screen" command or click on the command button in detached session window. The picture is captured immediately and no confirm message is shown.

Tip: In options you can enable option to "Auto switch to manager on capture" after picture is taken.

## Captures manager
When managing large number of screen shots it may help to have a control on captured pictures. There is a simple pictures manager build in Terminals. To open it select "Tools > Screen capture manager". This manager allows manage folders, view captured screen shots and assign comments to them.

* **Add or delete directory**: right click on the directory tree and select required command from context menu, like in any other file manager
* **Delete selected image**: in preview window in middle of the capture manager pane right click on required file and select "Delete"
* **Copy path** or **image to clipboard**: select the command from picture context menu like to delete

## Manage captured image comments
Capture manager allows you to assign custom comment to the picture. The comment is stored in a text file, in the same directory where the selected picture resides. The name of an comment file is the picture file name, but with the "**comments**" extension. To manage picture comments, select required picture in preview pane and type 
* **Save**: Creates or updates comments file associated with the picture
* **Delete**: Simply removes comments file form your capture directory

## Publish pictures using Flickr
There are also additional options to authenticate against Flickr service, which allows you to publish your capture using this service. There are no additional options to handle this service. To configure this service go to "Flickr" page in Capture configuration of application options.