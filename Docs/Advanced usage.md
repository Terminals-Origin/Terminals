# Advanced usage
Here are some tips, how to use Terminals which cant be directly clean to users.

* Share favorites file: If you can't use SQL database (which is recommended scenario) and you want to share your connections, you can share your favorites file directly using command line argument. More about command line arguments see [Command line arguments](Command-line-arguments)

* Configure RDP to allow drag and drop into to the session: See [Configure connection share](Configure-connection-share)

* Connect to Virtual private network (VPN) before starting remote connection: For this purpose you can use "Execute before connect" options. This options are available for all connection types. Go to favorite "Execute" tab page and "Execute before connect" check box. In next three fields fill command line to execute (or path to script) like you do from command prompt.

* Connect to Windwos Azure: Go to favorite RDP options > Extended Settings and fill "Load balanced info" field using following example: <RoleName>#<RoleInstanceName>

Where: "RoleName" and "RoleInstanceName" (without brackets) should be replaced by your values.
More Info: [http://msdn.microsoft.com/en-us/library/windowsazure/gg433063.aspx](http___msdn.microsoft.com_en-us_library_windowsazure_gg433063.aspx)