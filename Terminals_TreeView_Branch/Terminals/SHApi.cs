using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Terminals
{
    public class SHApi
    {
        public delegate Int32 BrowseCallbackProc(IntPtr hwnd, UInt32 uMsg, Int32 lParam, Int32 lpData);

        // Contains parameters for the SHBrowseForFolder function and receives information about the folder selected 
        // by the user.
        [StructLayout(LayoutKind.Sequential)]
        public struct BROWSEINFO
        {
            public IntPtr hwndOwner;				// Handle to the owner window for the dialog box.

            public IntPtr pidlRoot;					// Pointer to an item identifier list (PIDL) specifying the 
            // location of the root folder from which to start browsing.

            [MarshalAs(UnmanagedType.LPStr)]		// Address of a buffer to receive the display name of the 
            public String pszDisplayName;			// folder selected by the user.

            [MarshalAs(UnmanagedType.LPStr)]		// Address of a null-terminated string that is displayed 
            public String lpszTitle;				// above the tree view control in the dialog box.

            public UInt32 ulFlags;					// Flags specifying the options for the dialog box. 

            [MarshalAs(UnmanagedType.FunctionPtr)]	// Address of an application-defined function that the 
            public BrowseCallbackProc lpfn;			// dialog box calls when an event occurs.

            public Int32 lParam;					// Application-defined value that the dialog box passes to 
            // the callback function

            public Int32 iImage;					// Variable to receive the image associated with the selected folder.
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct STRRET
        {
            [FieldOffset(0)]
            public UInt32 uType;						// One of the STRRET_* values

            [FieldOffset(4)]
            public IntPtr pOleStr;						// must be freed by caller of GetDisplayNameOf

            [FieldOffset(4)]
            public IntPtr pStr;							// NOT USED

            [FieldOffset(4)]
            public UInt32 uOffset;						// Offset into SHITEMID

            [FieldOffset(4)]
            public IntPtr cStr;							// Buffer to fill in (ANSI)
        }

        // Contains information used by ShellExecuteEx
        [StructLayout(LayoutKind.Sequential)]
        public struct SHELLEXECUTEINFO
        {
            public UInt32 cbSize;					// Size of the structure, in bytes. 
            public UInt32 fMask;					// Array of flags that indicate the content and validity of the 
            // other structure members.
            public IntPtr hwnd;						// Window handle to any message boxes that the system might produce
            // while executing this function. 
            [MarshalAs(UnmanagedType.LPWStr)]
            public String lpVerb;					// String, referred to as a verb, that specifies the action to 
            // be performed. 
            [MarshalAs(UnmanagedType.LPWStr)]
            public String lpFile;					// Address of a null-terminated string that specifies the name of 
            // the file or object on which ShellExecuteEx will perform the 
            // action specified by the lpVerb parameter.
            [MarshalAs(UnmanagedType.LPWStr)]
            public String lpParameters;				// Address of a null-terminated string that contains the 
            // application parameters.
            [MarshalAs(UnmanagedType.LPWStr)]
            public String lpDirectory;				// Address of a null-terminated string that specifies the name of 
            // the working directory. 
            public Int32 nShow;						// Flags that specify how an application is to be shown when it 
            // is opened.
            public IntPtr hInstApp;					// If the function succeeds, it sets this member to a value 
            // greater than 32.
            public IntPtr lpIDList;					// Address of an ITEMIDLIST structure to contain an item identifier
            // list uniquely identifying the file to execute.
            [MarshalAs(UnmanagedType.LPWStr)]
            public String lpClass;					// Address of a null-terminated string that specifies the name of 
            // a file class or a globally unique identifier (GUID). 
            public IntPtr hkeyClass;				// Handle to the registry key for the file class.
            public UInt32 dwHotKey;					// Hot key to associate with the application.
            public IntPtr hIconMonitor;				// Handle to the icon for the file class. OR Handle to the monitor 
            // upon which the document is to be displayed. 
            public IntPtr hProcess;					// Handle to the newly started application.
        }

        // Contains information that the SHFileOperation function uses to perform file operations. 
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct SHFILEOPSTRUCT
        {
            public IntPtr hwnd;						// Window handle to the dialog box to display information about the 
            // status of the file operation. 
            public UInt32 wFunc;					// Value that indicates which operation to perform.
            public IntPtr pFrom;					// Address of a buffer to specify one or more source file names. 
            // These names must be fully qualified paths. Standard Microsoft® 
            // MS-DOS® wild cards, such as "*", are permitted in the file-name 
            // position. Although this member is declared as a null-terminated 
            // string, it is used as a buffer to hold multiple file names. Each 
            // file name must be terminated by a single NULL character. An	
            // additional NULL character must be appended to the end of the 
            // final name to indicate the end of pFrom. 
            public IntPtr pTo;						// Address of a buffer to contain the name of the destination file or 
            // directory. This parameter must be set to NULL if it is not used.
            // Like pFrom, the pTo member is also a double-null terminated 
            // string and is handled in much the same way. 
            public UInt16 fFlags;					// Flags that control the file operation. 
            public Int32 fAnyOperationsAborted;		// Value that receives TRUE if the user aborted any file operations
            // before they were completed, or FALSE otherwise. 
            public IntPtr hNameMappings;			// A handle to a name mapping object containing the old and new 
            // names of the renamed files. This member is used only if the 
            // fFlags member includes the FOF_WANTMAPPINGHANDLE flag.
            [MarshalAs(UnmanagedType.LPWStr)]
            public String lpszProgressTitle;		// Address of a string to use as the title of a progress dialog box.
            // This member is used only if fFlags includes the 
            // FOF_SIMPLEPROGRESS flag.
        }


        // Retrieves a pointer to the Shell's IMalloc interface.
        [DllImport("shell32.dll")]
        public static extern Int32 SHGetMalloc(
            out IntPtr hObject);	// Address of a pointer that receives the Shell's IMalloc interface pointer. 

        // Retrieves the path of a folder as an PIDL.
        [DllImport("shell32.dll")]
        public static extern Int32 SHGetFolderLocation(
            IntPtr hwndOwner,		// Handle to the owner window.
            Int32 nFolder,			// A CSIDL value that identifies the folder to be located.
            IntPtr hToken,			// Token that can be used to represent a particular user.
            UInt32 dwReserved,		// Reserved.
            out IntPtr ppidl);		// Address of a pointer to an item identifier list structure 
        // specifying the folder's location relative to the root of the namespace 
        // (the desktop). 

        // Converts an item identifier list to a file system path. 
        [DllImport("shell32.dll")]
        public static extern Int32 SHGetPathFromIDList(
            IntPtr pidl,			// Address of an item identifier list that specifies a file or directory location 
            // relative to the root of the namespace (the desktop). 
            StringBuilder pszPath);	// Address of a buffer to receive the file system path.


        // Takes the CSIDL of a folder and returns the pathname.
        [DllImport("shell32.dll")]
        public static extern Int32 SHGetFolderPath(
            IntPtr hwndOwner,			// Handle to an owner window.
            Int32 nFolder,				// A CSIDL value that identifies the folder whose path is to be retrieved.
            IntPtr hToken,				// An access token that can be used to represent a particular user.
            UInt32 dwFlags,				// Flags to specify which path is to be returned. It is used for cases where 
            // the folder associated with a CSIDL may be moved or renamed by the user. 
            StringBuilder pszPath);		// Pointer to a null-terminated string which will receive the path.

        // Translates a Shell namespace object's display name into an item identifier list and returns the attributes 
        // of the object. This function is the preferred method to convert a string to a pointer to an item 
        // identifier list (PIDL). 
        [DllImport("shell32.dll")]
        public static extern Int32 SHParseDisplayName(
            [MarshalAs(UnmanagedType.LPWStr)]
			String pszName,				// Pointer to a zero-terminated wide string that contains the display name 
            // to parse. 
            IntPtr pbc,					// Optional bind context that controls the parsing operation. This parameter 
            // is normally set to NULL.
            out IntPtr ppidl,			// Address of a pointer to a variable of type ITEMIDLIST that receives the item
            // identifier list for the object.
            UInt32 sfgaoIn,				// ULONG value that specifies the attributes to query.
            out UInt32 psfgaoOut);		// Pointer to a ULONG. On return, those attributes that are true for the 
        // object and were requested in sfgaoIn will be set. 


        // Retrieves the IShellFolder interface for the desktop folder, which is the root of the Shell's namespace. 
        [DllImport("shell32.dll")]
        public static extern Int32 SHGetDesktopFolder(
            out IntPtr ppshf);			// Address that receives an IShellFolder interface pointer for the 
        // desktop folder.

        // This function takes the fully-qualified pointer to an item identifier list (PIDL) of a namespace object, 
        // and returns a specified interface pointer on the parent object.
        [DllImport("shell32.dll")]
        public static extern Int32 SHBindToParent(
            IntPtr pidl,			// The item's PIDL. 
            [MarshalAs(UnmanagedType.LPStruct)]
			Guid riid,				// The REFIID of one of the interfaces exposed by the item's parent object. 
            out IntPtr ppv,			// A pointer to the interface specified by riid. You must release the object when 
            // you are finished. 
            ref IntPtr ppidlLast);	// The item's PIDL relative to the parent folder. This PIDL can be used with many
        // of the methods supported by the parent folder's interfaces. If you set ppidlLast 
        // to NULL, the PIDL will not be returned. 

        // Accepts a STRRET structure returned by IShellFolder::GetDisplayNameOf that contains or points to a 
        // string, and then returns that string as a BSTR.
        [DllImport("shlwapi.dll")]
        public static extern Int32 StrRetToBSTR(
            ref STRRET pstr,		// Pointer to a STRRET structure.
            IntPtr pidl,			// Pointer to an ITEMIDLIST uniquely identifying a file object or subfolder relative
            // to the parent folder.
            [MarshalAs(UnmanagedType.BStr)]
			out String pbstr);		// Pointer to a variable of type BSTR that contains the converted string.

        // Takes a STRRET structure returned by IShellFolder::GetDisplayNameOf, converts it to a string, and 
        // places the result in a buffer. 
        [DllImport("shlwapi.dll")]
        public static extern Int32 StrRetToBuf(
            ref STRRET pstr,		// Pointer to the STRRET structure. When the function returns, this pointer will no
            // longer be valid.
            IntPtr pidl,			// Pointer to the item's ITEMIDLIST structure.
            StringBuilder pszBuf,	// Buffer to hold the display name. It will be returned as a null-terminated
            // string. If cchBuf is too small, the name will be truncated to fit. 
            UInt32 cchBuf);			// Size of pszBuf, in characters. If cchBuf is too small, the string will be 
        // truncated to fit. 



        // Displays a dialog box that enables the user to select a Shell folder. 
        [DllImport("shell32.dll")]
        public static extern IntPtr SHBrowseForFolder(
            ref BROWSEINFO lbpi);	// Pointer to a BROWSEINFO structure that contains information used to display 
        // the dialog box. 

        // Performs an operation on a specified file.
        [DllImport("shell32.dll")]
        public static extern IntPtr ShellExecute(
            IntPtr hwnd,			// Handle to a parent window.
            [MarshalAs(UnmanagedType.LPStr)]
			String lpOperation,		// Pointer to a null-terminated string, referred to in this case as a verb, 
            // that specifies the action to be performed.
            [MarshalAs(UnmanagedType.LPStr)]
			String lpFile,			// Pointer to a null-terminated string that specifies the file or object on which 
            // to execute the specified verb.
            [MarshalAs(UnmanagedType.LPStr)]
			String lpParameters,	// If the lpFile parameter specifies an executable file, lpParameters is a pointer 
            // to a null-terminated string that specifies the parameters to be passed 
            // to the application.
            [MarshalAs(UnmanagedType.LPStr)]
			String lpDirectory,		// Pointer to a null-terminated string that specifies the default directory. 
            Int32 nShowCmd);		// Flags that specify how an application is to be displayed when it is opened.

        // Performs an action on a file. 
        [DllImport("shell32.dll")]
        public static extern Int32 ShellExecuteEx(
            ref SHELLEXECUTEINFO lpExecInfo);	// Address of a SHELLEXECUTEINFO structure that contains and receives 
        // information about the application being executed. 

        // Copies, moves, renames, or deletes a file system object. 
        [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
        public static extern Int32 SHFileOperation(
            ref SHFILEOPSTRUCT lpFileOp);		// Address of an SHFILEOPSTRUCT structure that contains information 
        // this function needs to carry out the specified operation. This 
        // parameter must contain a valid value that is not NULL. You are 
        // responsibile for validating the value. If you do not validate it, 
        // you will experience unexpected results. 

        // Notifies the system of an event that an application has performed. An application should use this function
        // if it performs an action that may affect the Shell. 
        [DllImport("shell32.dll")]
        public static extern void SHChangeNotify(
            UInt32 wEventId,				// Describes the event that has occurred. the 
            // ShellChangeNotificationEvents enum contains a list of options.
            UInt32 uFlags,					// Flags that indicate the meaning of the dwItem1 and dwItem2 parameters.
            IntPtr dwItem1,					// First event-dependent value. 
            IntPtr dwItem2);				// Second event-dependent value. 

        // Adds a document to the Shell's list of recently used documents or clears all documents from the list. 
        [DllImport("shell32.dll")]
        public static extern void SHAddToRecentDocs(
            UInt32 uFlags,					// Flag that indicates the meaning of the pv parameter.
            IntPtr pv);						// A pointer to either a null-terminated string with the path and file name 
        // of the document, or a PIDL that identifies the document's file object. 
        // Set this parameter to NULL to clear all documents from the list. 
        [DllImport("shell32.dll")]
        public static extern void SHAddToRecentDocs(
            UInt32 uFlags,
            [MarshalAs(UnmanagedType.LPWStr)]
			String pv);

        // Executes a command on a printer object. 
        [DllImport("shell32.dll")]
        public static extern Int32 SHInvokePrinterCommand(
            IntPtr hwnd,						// Handle of the window that will be used as the parent of any windows 
            // or dialog boxes that are created during the operation.
            UInt32 uAction,						// A value that determines the type of printer operation that will be 
            // performed.
            [MarshalAs(UnmanagedType.LPWStr)]
			String lpBuf1,						// Address of a null_terminated string that contains additional 
            // information for the printer command. 
            [MarshalAs(UnmanagedType.LPWStr)]	
			String lpBuf2,						// Address of a null-terminated string that contains additional
            // information for the printer command. 
            Int32 fModal);						//  value that determines whether SHInvokePrinterCommand should return
        // after initializing the command or wait until the command is completed.


        public static Int16 GetHResultCode(Int32 hr)
        {
            hr = hr & 0x0000ffff;
            return (Int16)hr;
        }


        public enum CSIDL
        {
            CSIDL_FLAG_CREATE = (0x8000),	// Version 5.0. Combine this CSIDL with any of the following 
            //CSIDLs to force the creation of the associated folder. 
            CSIDL_ADMINTOOLS = (0x0030),	// Version 5.0. The file system directory that is used to store 
            // administrative tools for an individual user. The Microsoft 
            // Management Console (MMC) will save customized consoles to 
            // this directory, and it will roam with the user.
            CSIDL_ALTSTARTUP = (0x001d),	// The file system directory that corresponds to the user's 
            // nonlocalized Startup program group.
            CSIDL_APPDATA = (0x001a),	// Version 4.71. The file system directory that serves as a 
            // common repository for application-specific data. A typical
            // path is C:\Documents and Settings\username\Application Data. 
            // This CSIDL is supported by the redistributable Shfolder.dll 
            // for systems that do not have the Microsoft® Internet 
            // Explorer 4.0 integrated Shell installed.
            CSIDL_BITBUCKET = (0x000a),	// The virtual folder containing the objects in the user's 
            // Recycle Bin.
            CSIDL_CDBURN_AREA = (0x003b),	// Version 6.0. The file system directory acting as a staging
            // area for files waiting to be written to CD. A typical path 
            // is C:\Documents and Settings\username\Local Settings\
            // Application Data\Microsoft\CD Burning.
            CSIDL_COMMON_ADMINTOOLS = (0x002f),	// Version 5.0. The file system directory containing 
            // administrative tools for all users of the computer.
            CSIDL_COMMON_ALTSTARTUP = (0x001e), // The file system directory that corresponds to the 
            // nonlocalized Startup program group for all users. Valid only 
            // for Microsoft Windows NT® systems.
            CSIDL_COMMON_APPDATA = (0x0023), // Version 5.0. The file system directory containing application 
            // data for all users. A typical path is C:\Documents and 
            // Settings\All Users\Application Data.
            CSIDL_COMMON_DESKTOPDIRECTORY = (0x0019), // The file system directory that contains files and folders 
            // that appear on the desktop for all users. A typical path is 
            // C:\Documents and Settings\All Users\Desktop. Valid only for 
            // Windows NT systems.
            CSIDL_COMMON_DOCUMENTS = (0x002e), // The file system directory that contains documents that are 
            // common to all users. A typical paths is C:\Documents and 
            // Settings\All Users\Documents. Valid for Windows NT systems 
            // and Microsoft Windows® 95 and Windows 98 systems with 
            // Shfolder.dll installed.
            CSIDL_COMMON_FAVORITES = (0x001f), // The file system directory that serves as a common repository
            // for favorite items common to all users. Valid only for 
            // Windows NT systems.
            CSIDL_COMMON_MUSIC = (0x0035), // Version 6.0. The file system directory that serves as a 
            // repository for music files common to all users. A typical 
            // path is C:\Documents and Settings\All Users\Documents\
            // My Music.
            CSIDL_COMMON_PICTURES = (0x0036), // Version 6.0. The file system directory that serves as a 
            // repository for image files common to all users. A typical 
            // path is C:\Documents and Settings\All Users\Documents\
            // My Pictures.
            CSIDL_COMMON_PROGRAMS = (0x0017), // The file system directory that contains the directories for 
            // the common program groups that appear on the Start menu for
            // all users. A typical path is C:\Documents and Settings\
            // All Users\Start Menu\Programs. Valid only for Windows NT 
            // systems.
            CSIDL_COMMON_STARTMENU = (0x0016), // The file system directory that contains the programs and 
            // folders that appear on the Start menu for all users. A 
            // typical path is C:\Documents and Settings\All Users\
            // Start Menu. Valid only for Windows NT systems.
            CSIDL_COMMON_STARTUP = (0x0018), // The file system directory that contains the programs that 
            // appear in the Startup folder for all users. A typical path 
            // is C:\Documents and Settings\All Users\Start Menu\Programs\
            // Startup. Valid only for Windows NT systems.
            CSIDL_COMMON_TEMPLATES = (0x002d), // The file system directory that contains the templates that 
            // are available to all users. A typical path is C:\Documents 
            // and Settings\All Users\Templates. Valid only for Windows 
            // NT systems.
            CSIDL_COMMON_VIDEO = (0x0037), // Version 6.0. The file system directory that serves as a 
            // repository for video files common to all users. A typical 
            // path is C:\Documents and Settings\All Users\Documents\
            // My Videos.
            CSIDL_CONTROLS = (0x0003), // The virtual folder containing icons for the Control Panel 
            // applications.
            CSIDL_COOKIES = (0x0021), // The file system directory that serves as a common repository 
            // for Internet cookies. A typical path is C:\Documents and 
            // Settings\username\Cookies.
            CSIDL_DESKTOP = (0x0000), // The virtual folder representing the Windows desktop, the root 
            // of the namespace.
            CSIDL_DESKTOPDIRECTORY = (0x0010), // The file system directory used to physically store file 
            // objects on the desktop (not to be confused with the desktop 
            // folder itself). A typical path is C:\Documents and 
            // Settings\username\Desktop.
            CSIDL_DRIVES = (0x0011), // The virtual folder representing My Computer, containing 
            // everything on the local computer: storage devices, printers,
            // and Control Panel. The folder may also contain mapped 
            // network drives.
            CSIDL_FAVORITES = (0x0006), // The file system directory that serves as a common repository 
            // for the user's favorite items. A typical path is C:\Documents
            // and Settings\username\Favorites.
            CSIDL_FONTS = (0x0014), // A virtual folder containing fonts. A typical path is 
            // C:\Windows\Fonts.
            CSIDL_HISTORY = (0x0022), // The file system directory that serves as a common repository
            // for Internet history items.
            CSIDL_INTERNET = (0x0001), // A virtual folder representing the Internet.
            CSIDL_INTERNET_CACHE = (0x0020), // Version 4.72. The file system directory that serves as a 
            // common repository for temporary Internet files. A typical 
            // path is C:\Documents and Settings\username\Local Settings\
            // Temporary Internet Files.
            CSIDL_LOCAL_APPDATA = (0x001c), // Version 5.0. The file system directory that serves as a data
            // repository for local (nonroaming) applications. A typical 
            // path is C:\Documents and Settings\username\Local Settings\
            // Application Data.
            CSIDL_MYDOCUMENTS = (0x000c), // Version 6.0. The virtual folder representing the My Documents
            // desktop item. This should not be confused with 
            // CSIDL_PERSONAL, which represents the file system folder that 
            // physically stores the documents.
            CSIDL_MYMUSIC = (0x000d), // The file system directory that serves as a common repository 
            // for music files. A typical path is C:\Documents and Settings
            // \User\My Documents\My Music.
            CSIDL_MYPICTURES = (0x0027), // Version 5.0. The file system directory that serves as a 
            // common repository for image files. A typical path is 
            // C:\Documents and Settings\username\My Documents\My Pictures.
            CSIDL_MYVIDEO = (0x000e), // Version 6.0. The file system directory that serves as a 
            // common repository for video files. A typical path is 
            // C:\Documents and Settings\username\My Documents\My Videos.
            CSIDL_NETHOOD = (0x0013), // A file system directory containing the link objects that may 
            // exist in the My Network Places virtual folder. It is not the
            // same as CSIDL_NETWORK, which represents the network namespace
            // root. A typical path is C:\Documents and Settings\username\
            // NetHood.
            CSIDL_NETWORK = (0x0012), // A virtual folder representing Network Neighborhood, the root
            // of the network namespace hierarchy.
            CSIDL_PERSONAL = (0x0005), // The file system directory used to physically store a user's
            // common repository of documents. A typical path is 
            // C:\Documents and Settings\username\My Documents. This should
            // be distinguished from the virtual My Documents folder in 
            // the namespace, identified by CSIDL_MYDOCUMENTS. 
            CSIDL_PRINTERS = (0x0004), // The virtual folder containing installed printers.
            CSIDL_PRINTHOOD = (0x001b), // The file system directory that contains the link objects that
            // can exist in the Printers virtual folder. A typical path is 
            // C:\Documents and Settings\username\PrintHood.
            CSIDL_PROFILE = (0x0028), // Version 5.0. The user's profile folder. A typical path is 
            // C:\Documents and Settings\username. Applications should not 
            // create files or folders at this level; they should put their
            // data under the locations referred to by CSIDL_APPDATA or
            // CSIDL_LOCAL_APPDATA.
            CSIDL_PROFILES = (0x003e), // Version 6.0. The file system directory containing user 
            // profile folders. A typical path is C:\Documents and Settings.
            CSIDL_PROGRAM_FILES = (0x0026), // Version 5.0. The Program Files folder. A typical path is 
            // C:\Program Files.
            CSIDL_PROGRAM_FILES_COMMON = (0x002b), // Version 5.0. A folder for components that are shared across 
            // applications. A typical path is C:\Program Files\Common. 
            // Valid only for Windows NT, Windows 2000, and Windows XP 
            // systems. Not valid for Windows Millennium Edition 
            // (Windows Me).
            CSIDL_PROGRAMS = (0x0002), // The file system directory that contains the user's program 
            // groups (which are themselves file system directories).
            // A typical path is C:\Documents and Settings\username\
            // Start Menu\Programs. 
            CSIDL_RECENT = (0x0008), // The file system directory that contains shortcuts to the 
            // user's most recently used documents. A typical path is 
            // C:\Documents and Settings\username\My Recent Documents. 
            // To create a shortcut in this folder, use SHAddToRecentDocs.
            // In addition to creating the shortcut, this function updates
            // the Shell's list of recent documents and adds the shortcut 
            // to the My Recent Documents submenu of the Start menu.
            CSIDL_SENDTO = (0x0009), // The file system directory that contains Send To menu items.
            // A typical path is C:\Documents and Settings\username\SendTo.
            CSIDL_STARTMENU = (0x000b), // The file system directory containing Start menu items. A 
            // typical path is C:\Documents and Settings\username\Start Menu.
            CSIDL_STARTUP = (0x0007), // The file system directory that corresponds to the user's 
            // Startup program group. The system starts these programs 
            // whenever any user logs onto Windows NT or starts Windows 95.
            // A typical path is C:\Documents and Settings\username\
            // Start Menu\Programs\Startup.
            CSIDL_SYSTEM = (0x0025), // Version 5.0. The Windows System folder. A typical path is 
            // C:\Windows\System32.
            CSIDL_TEMPLATES = (0x0015), // The file system directory that serves as a common repository
            // for document templates. A typical path is C:\Documents 
            // and Settings\username\Templates.
            CSIDL_WINDOWS = (0x0024), // Version 5.0. The Windows directory or SYSROOT. This 
            // corresponds to the %windir% or %SYSTEMROOT% environment 
            // variables. A typical path is C:\Windows.
        }

        public enum SHGFP_TYPE
        {
            SHGFP_TYPE_CURRENT = 0,		// current value for user, verify it exists
            SHGFP_TYPE_DEFAULT = 1		// default value, may not exist
        }

        public enum SFGAO : uint
        {
            SFGAO_CANCOPY = 0x00000001,	// Objects can be copied    
            SFGAO_CANMOVE = 0x00000002,	// Objects can be moved     
            SFGAO_CANLINK = 0x00000004,	// Objects can be linked    
            SFGAO_STORAGE = 0x00000008,   // supports BindToObject(IID_IStorage)
            SFGAO_CANRENAME = 0x00000010,   // Objects can be renamed
            SFGAO_CANDELETE = 0x00000020,   // Objects can be deleted
            SFGAO_HASPROPSHEET = 0x00000040,   // Objects have property sheets
            SFGAO_DROPTARGET = 0x00000100,   // Objects are drop target
            SFGAO_CAPABILITYMASK = 0x00000177,	// This flag is a mask for the capability flags.
            SFGAO_ENCRYPTED = 0x00002000,   // object is encrypted (use alt color)
            SFGAO_ISSLOW = 0x00004000,   // 'slow' object
            SFGAO_GHOSTED = 0x00008000,   // ghosted icon
            SFGAO_LINK = 0x00010000,   // Shortcut (link)
            SFGAO_SHARE = 0x00020000,   // shared
            SFGAO_READONLY = 0x00040000,   // read-only
            SFGAO_HIDDEN = 0x00080000,   // hidden object
            SFGAO_DISPLAYATTRMASK = 0x000FC000,	// This flag is a mask for the display attributes.
            SFGAO_FILESYSANCESTOR = 0x10000000,   // may contain children with SFGAO_FILESYSTEM
            SFGAO_FOLDER = 0x20000000,   // support BindToObject(IID_IShellFolder)
            SFGAO_FILESYSTEM = 0x40000000,   // is a win32 file system object (file/folder/root)
            SFGAO_HASSUBFOLDER = 0x80000000,   // may contain children with SFGAO_FOLDER
            SFGAO_CONTENTSMASK = 0x80000000,	// This flag is a mask for the contents attributes.
            SFGAO_VALIDATE = 0x01000000,   // invalidate cached information
            SFGAO_REMOVABLE = 0x02000000,   // is this removeable media?
            SFGAO_COMPRESSED = 0x04000000,   // Object is compressed (use alt color)
            SFGAO_BROWSABLE = 0x08000000,   // supports IShellFolder, but only implements CreateViewObject() (non-folder view)
            SFGAO_NONENUMERATED = 0x00100000,   // is a non-enumerated object
            SFGAO_NEWCONTENT = 0x00200000,   // should show bold in explorer tree
            SFGAO_CANMONIKER = 0x00400000,   // defunct
            SFGAO_HASSTORAGE = 0x00400000,   // defunct
            SFGAO_STREAM = 0x00400000,   // supports BindToObject(IID_IStream)
            SFGAO_STORAGEANCESTOR = 0x00800000,   // may contain children with SFGAO_STORAGE or SFGAO_STREAM
            SFGAO_STORAGECAPMASK = 0x70C50008    // for determining storage capabilities, ie for open/save semantics

        }

        public enum SHCONTF
        {
            SHCONTF_FOLDERS = 0x0020,   // only want folders enumerated (SFGAO_FOLDER)
            SHCONTF_NONFOLDERS = 0x0040,   // include non folders
            SHCONTF_INCLUDEHIDDEN = 0x0080,   // show items normally hidden
            SHCONTF_INIT_ON_FIRST_NEXT = 0x0100,   // allow EnumObject() to return before validating enum
            SHCONTF_NETPRINTERSRCH = 0x0200,   // hint that client is looking for printers
            SHCONTF_SHAREABLE = 0x0400,   // hint that client is looking sharable resources (remote shares)
            SHCONTF_STORAGE = 0x0800,   // include all items with accessible storage and their ancestors
        }

        public enum SHCIDS : uint
        {
            SHCIDS_ALLFIELDS = 0x80000000,	// Compare all the information contained in the ITEMIDLIST 
            // structure, not just the display names
            SHCIDS_CANONICALONLY = 0x10000000,	// When comparing by name, compare the system names but not the 
            // display names. 
            SHCIDS_BITMASK = 0xFFFF0000,
            SHCIDS_COLUMNMASK = 0x0000FFFF
        }

        public enum SHGNO
        {
            SHGDN_NORMAL = 0x0000,		// default (display purpose)
            SHGDN_INFOLDER = 0x0001,		// displayed under a folder (relative)
            SHGDN_FOREDITING = 0x1000,		// for in-place editing
            SHGDN_FORADDRESSBAR = 0x4000,		// UI friendly parsing name (remove ugly stuff)
            SHGDN_FORPARSING = 0x8000		// parsing name for ParseDisplayName()
        }

        public enum STRRET_TYPE
        {
            STRRET_WSTR = 0x0000,			// Use STRRET.pOleStr
            STRRET_OFFSET = 0x0001,			// Use STRRET.uOffset to Ansi
            STRRET_CSTR = 0x0002			// Use STRRET.cStr
        }


        public enum PrinterActions
        {
            PRINTACTION_OPEN = 0,	// The printer specified by the name in lpBuf1 will be opened. 
            // lpBuf2 is ignored. 
            PRINTACTION_PROPERTIES = 1,	// The properties for the printer specified by the name in lpBuf1
            // will be displayed. lpBuf2 can either be NULL or specify.
            PRINTACTION_NETINSTALL = 2,	// The network printer specified by the name in lpBuf1 will be 
            // installed. lpBuf2 is ignored. 
            PRINTACTION_NETINSTALLLINK = 3,	// A shortcut to the network printer specified by the name in lpBuf1
            // will be created. lpBuf2 specifies the drive and path of the folder 
            // in which the shortcut will be created. The network printer must 
            // have already been installed on the local computer. 
            PRINTACTION_TESTPAGE = 4,	// A test page will be printed on the printer specified by the name
            // in lpBuf1. lpBuf2 is ignored. 
            PRINTACTION_OPENNETPRN = 5,	// The network printer specified by the name in lpBuf1 will be
            // opened. lpBuf2 is ignored. 
            PRINTACTION_DOCUMENTDEFAULTS = 6,	// Microsoft® Windows NT® only. The default document properties for
            // the printer specified by the name in lpBuf1 will be displayed. 
            // lpBuf2 is ignored. 
            PRINTACTION_SERVERPROPERTIES = 7		// Windows NT only. The properties for the server of the printer 
            // specified by the name in lpBuf1 will be displayed. lpBuf2 
            // is ignored.
        }
    }
}
