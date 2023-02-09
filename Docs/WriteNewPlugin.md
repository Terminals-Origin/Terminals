# How to write new plugin

Terminals supports plugability for connections. As an example you can have a look how Pytty pluging was developed or any other plugin we provide out of the box to see, how they are implemented. To create new application extension follow these steps:

1. Create new class library with output name like "Terminals.Plugins.*.dll"
2. Add reference to the **Terminals.Common.dll**
3. Add and compile new extension class implementing **IConnectionPlugin**
4. Deploy the extension by putting it to the the "Plugins" directory

Plugin implementation consist of three parts:

* Protocol identifiers - Port number, Protocol name and its icon to be able distinguis each plugins favorite.
* Protocol Options - plugin specific settings per connection favorite. 
* Options controls - these controls are added to the favorite properties dialog to be able change values defined by your ProtocolOptions. 
Detailed documentation see IConnectionPlugin.cs.

Additionally to the plugin it self, following rules apply:

* No registration is needed, all plugins are loaded automatically during application startup
* Plugins may be disabled in Application options, but are enabled by default
* Application is unable to start, if there is no enabled plugin available
* All published plugins need to have unique protocol names
* One assembly may contain multiple plugins
* No versioning is applied to the plugins interface (first version)
* In case of issues with loading the plugin debug PluginLoader and later ConnectionManager classes.