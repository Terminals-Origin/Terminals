using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("Terminals Launcher")]
[assembly: AssemblyDescription("RDP, VNC, VMRC, RAS, Telnet, SSH, ICA Citrix, " +
                               "Amazon S3 client helper application to start Terminals with proper rights")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("26530c22-acc3-4b1d-aa48-fc12c638ebdb")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//

[assembly: log4net.Config.XmlConfigurator(
ConfigFile = "TerminalsLauncher.log4net.config", Watch = true)]
