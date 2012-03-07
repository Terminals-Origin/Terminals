
namespace Terminals.Native
{
    internal enum CSIDL
    {
        CSIDL_FLAG_CREATE = (0x8000), // Version 5.0. Combine this CSIDL with any of the following 
        //CSIDLs to force the creation of the associated folder. 
        CSIDL_ADMINTOOLS = (0x0030), // Version 5.0. The file system directory that is used to store 
        // administrative tools for an individual user. The Microsoft 
        // Management Console (MMC) will save customized consoles to 
        // this directory, and it will roam with the user.
        CSIDL_ALTSTARTUP = (0x001d), // The file system directory that corresponds to the user's 
        // nonlocalized Startup program group.
        CSIDL_APPDATA = (0x001a), // Version 4.71. The file system directory that serves as a 
        // common repository for application-specific data. A typical
        // path is C:\Documents and Settings\username\Application Data. 
        // This CSIDL is supported by the redistributable Shfolder.dll 
        // for systems that do not have the Microsoft® Internet 
        // Explorer 4.0 integrated Shell installed.
        CSIDL_BITBUCKET = (0x000a), // The virtual folder containing the objects in the user's 
        // Recycle Bin.
        CSIDL_CDBURN_AREA = (0x003b), // Version 6.0. The file system directory acting as a staging
        // area for files waiting to be written to CD. A typical path 
        // is C:\Documents and Settings\username\Local Settings\
        // Application Data\Microsoft\CD Burning.
        CSIDL_COMMON_ADMINTOOLS = (0x002f), // Version 5.0. The file system directory containing 
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

    internal enum SHGFP_TYPE
    {
        SHGFP_TYPE_CURRENT = 0, // current value for user, verify it exists
        SHGFP_TYPE_DEFAULT = 1 // default value, may not exist
    }

    internal enum SFGAO : uint
    {
        SFGAO_CANCOPY = 0x00000001, // Objects can be copied    
        SFGAO_CANMOVE = 0x00000002, // Objects can be moved     
        SFGAO_CANLINK = 0x00000004, // Objects can be linked    
        SFGAO_STORAGE = 0x00000008, // supports BindToObject(IID_IStorage)
        SFGAO_CANRENAME = 0x00000010, // Objects can be renamed
        SFGAO_CANDELETE = 0x00000020, // Objects can be deleted
        SFGAO_HASPROPSHEET = 0x00000040, // Objects have property sheets
        SFGAO_DROPTARGET = 0x00000100, // Objects are drop target
        SFGAO_CAPABILITYMASK = 0x00000177, // This flag is a mask for the capability flags.
        SFGAO_ENCRYPTED = 0x00002000, // object is encrypted (use alt color)
        SFGAO_ISSLOW = 0x00004000, // 'slow' object
        SFGAO_GHOSTED = 0x00008000, // ghosted icon
        SFGAO_LINK = 0x00010000, // Shortcut (link)
        SFGAO_SHARE = 0x00020000, // shared
        SFGAO_READONLY = 0x00040000, // read-only
        SFGAO_HIDDEN = 0x00080000, // hidden object
        SFGAO_DISPLAYATTRMASK = 0x000FC000, // This flag is a mask for the display attributes.
        SFGAO_FILESYSANCESTOR = 0x10000000, // may contain children with SFGAO_FILESYSTEM
        SFGAO_FOLDER = 0x20000000, // support BindToObject(IID_IShellFolder)
        SFGAO_FILESYSTEM = 0x40000000, // is a win32 file system object (file/folder/root)
        SFGAO_HASSUBFOLDER = 0x80000000, // may contain children with SFGAO_FOLDER
        SFGAO_CONTENTSMASK = 0x80000000, // This flag is a mask for the contents attributes.
        SFGAO_VALIDATE = 0x01000000, // invalidate cached information
        SFGAO_REMOVABLE = 0x02000000, // is this removeable media?
        SFGAO_COMPRESSED = 0x04000000, // Object is compressed (use alt color)
        SFGAO_BROWSABLE = 0x08000000, // supports IShellFolder, but only implements CreateViewObject() (non-folder view)
        SFGAO_NONENUMERATED = 0x00100000, // is a non-enumerated object
        SFGAO_NEWCONTENT = 0x00200000, // should show bold in explorer tree
        SFGAO_CANMONIKER = 0x00400000, // defunct
        SFGAO_HASSTORAGE = 0x00400000, // defunct
        SFGAO_STREAM = 0x00400000, // supports BindToObject(IID_IStream)
        SFGAO_STORAGEANCESTOR = 0x00800000, // may contain children with SFGAO_STORAGE or SFGAO_STREAM
        SFGAO_STORAGECAPMASK = 0x70C50008 // for determining storage capabilities, ie for open/save semantics
    }

    internal enum SHCONTF
    {
        SHCONTF_FOLDERS = 0x0020, // only want folders enumerated (SFGAO_FOLDER)
        SHCONTF_NONFOLDERS = 0x0040, // include non folders
        SHCONTF_INCLUDEHIDDEN = 0x0080, // show items normally hidden
        SHCONTF_INIT_ON_FIRST_NEXT = 0x0100, // allow EnumObject() to return before validating enum
        SHCONTF_NETPRINTERSRCH = 0x0200, // hint that client is looking for printers
        SHCONTF_SHAREABLE = 0x0400, // hint that client is looking sharable resources (remote shares)
        SHCONTF_STORAGE = 0x0800, // include all items with accessible storage and their ancestors
    }

    internal enum SHCIDS : uint
    {
        SHCIDS_ALLFIELDS = 0x80000000, // Compare all the information contained in the ITEMIDLIST 
        // structure, not just the display names
        SHCIDS_CANONICALONLY = 0x10000000, // When comparing by name, compare the system names but not the 
        // display names. 
        SHCIDS_BITMASK = 0xFFFF0000,
        SHCIDS_COLUMNMASK = 0x0000FFFF
    }

    internal enum SHGNO
    {
        SHGDN_NORMAL = 0x0000, // default (display purpose)
        SHGDN_INFOLDER = 0x0001, // displayed under a folder (relative)
        SHGDN_FOREDITING = 0x1000, // for in-place editing
        SHGDN_FORADDRESSBAR = 0x4000, // UI friendly parsing name (remove ugly stuff)
        SHGDN_FORPARSING = 0x8000 // parsing name for ParseDisplayName()
    }

    internal enum STRRET_TYPE
    {
        STRRET_WSTR = 0x0000, // Use STRRET.pOleStr
        STRRET_OFFSET = 0x0001, // Use STRRET.uOffset to Ansi
        STRRET_CSTR = 0x0002 // Use STRRET.cStr
    }

    internal enum PrinterActions
    {
        PRINTACTION_OPEN = 0, // The printer specified by the name in lpBuf1 will be opened. 
        // lpBuf2 is ignored. 
        PRINTACTION_PROPERTIES = 1, // The properties for the printer specified by the name in lpBuf1
        // will be displayed. lpBuf2 can either be NULL or specify.
        PRINTACTION_NETINSTALL = 2, // The network printer specified by the name in lpBuf1 will be 
        // installed. lpBuf2 is ignored. 
        PRINTACTION_NETINSTALLLINK = 3, // A shortcut to the network printer specified by the name in lpBuf1
        // will be created. lpBuf2 specifies the drive and path of the folder 
        // in which the shortcut will be created. The network printer must 
        // have already been installed on the local computer. 
        PRINTACTION_TESTPAGE = 4, // A test page will be printed on the printer specified by the name
        // in lpBuf1. lpBuf2 is ignored. 
        PRINTACTION_OPENNETPRN = 5, // The network printer specified by the name in lpBuf1 will be
        // opened. lpBuf2 is ignored. 
        PRINTACTION_DOCUMENTDEFAULTS = 6, // Microsoft® Windows NT® only. The default document properties for
        // the printer specified by the name in lpBuf1 will be displayed. 
        // lpBuf2 is ignored. 
        PRINTACTION_SERVERPROPERTIES = 7 // Windows NT only. The properties for the server of the printer 
        // specified by the name in lpBuf1 will be displayed. lpBuf2 
        // is ignored.
    }
}
