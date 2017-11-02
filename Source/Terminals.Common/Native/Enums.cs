
using System;

namespace Terminals.Native
{


    [Flags()]
    public enum WindowExStyles : uint
    {
        WS_EX_DLGMODALFRAME = 0x0001,
        WS_EX_NOPARENTNOTIFY = 0x0004,
        WS_EX_TOPMOST = 0x0008,
        WS_EX_ACCEPTFILES = 0x0010,
        WS_EX_TRANSPARENT = 0x0020,
        WS_EX_MDICHILD = 0x0040,
        WS_EX_TOOLWINDOW = 0x0080,
        WS_EX_WINDOWEDGE = 0x0100,
        WS_EX_CLIENTEDGE = 0x0200,
        WS_EX_CONTEXTHELP = 0x0400,
        WS_EX_RIGHT = 0x1000,
        WS_EX_LEFT = 0x0000,
        WS_EX_RTLREADING = 0x2000,
        WS_EX_LTRREADING = 0x0000,
        WS_EX_LEFTSCROLLBAR = 0x4000,
        WS_EX_RIGHTSCROLLBAR = 0x0000,
        WS_EX_CONTROLPARENT = 0x10000,
        WS_EX_STATICEDGE = 0x20000,
        WS_EX_APPWINDOW = 0x40000,
        WS_EX_OVERLAPPEDWINDOW = (WS_EX_WINDOWEDGE | WS_EX_CLIENTEDGE),
        WS_EX_PALETTEWINDOW = (WS_EX_WINDOWEDGE | WS_EX_TOOLWINDOW | WS_EX_TOPMOST),
        WS_EX_LAYERED = 0x00080000,
        WS_EX_NOINHERITLAYOUT = 0x00100000, // Disable inheritence of mirroring by children
        WS_EX_LAYOUTRTL = 0x00400000, // Right to left mirroring
        WS_EX_COMPOSITED = 0x02000000,
        WS_EX_NOACTIVATE = 0x08000000
    }


    /// <summary>
    /// Window Styles.
    /// The following styles can be specified wherever a window style is required. After the control has been created, these styles cannot be modified, except as noted.
    /// </summary>
    [Flags()]
    public enum WindowStyles : uint
    {
        /// <summary>The window has a thin-line border.</summary>
        WS_BORDER = 0x800000,

        /// <summary>The window has a title bar (includes the WS_BORDER style).</summary>
        WS_CAPTION = 0xc00000,

        /// <summary>The window is a child window. A window with this style cannot have a menu bar. This style cannot be used with the WS_POPUP style.</summary>
        WS_CHILD = 0x40000000,

        /// <summary>Excludes the area occupied by child windows when drawing occurs within the parent window. This style is used when creating the parent window.</summary>
        WS_CLIPCHILDREN = 0x2000000,

        /// <summary>
        /// Clips child windows relative to each other; that is, when a particular child window receives a WM_PAINT message, the WS_CLIPSIBLINGS style clips all other overlapping child windows out of the region of the child window to be updated.
        /// If WS_CLIPSIBLINGS is not specified and child windows overlap, it is possible, when drawing within the client area of a child window, to draw within the client area of a neighboring child window.
        /// </summary>
        WS_CLIPSIBLINGS = 0x4000000,

        /// <summary>The window is initially disabled. A disabled window cannot receive input from the user. To change this after a window has been created, use the EnableWindow function.</summary>
        WS_DISABLED = 0x8000000,

        /// <summary>The window has a border of a style typically used with dialog boxes. A window with this style cannot have a title bar.</summary>
        WS_DLGFRAME = 0x400000,

        /// <summary>
        /// The window is the first control of a group of controls. The group consists of this first control and all controls defined after it, up to the next control with the WS_GROUP style.
        /// The first control in each group usually has the WS_TABSTOP style so that the user can move from group to group. The user can subsequently change the keyboard focus from one control in the group to the next control in the group by using the direction keys.
        /// You can turn this style on and off to change dialog box navigation. To change this style after a window has been created, use the SetWindowLong function.
        /// </summary>
        WS_GROUP = 0x20000,

        /// <summary>The window has a horizontal scroll bar.</summary>
        WS_HSCROLL = 0x100000,

        /// <summary>The window is initially maximized.</summary> 
        WS_MAXIMIZE = 0x1000000,

        /// <summary>The window has a maximize button. Cannot be combined with the WS_EX_CONTEXTHELP style. The WS_SYSMENU style must also be specified.</summary> 
        WS_MAXIMIZEBOX = 0x10000,

        /// <summary>The window is initially minimized.</summary>
        WS_MINIMIZE = 0x20000000,

        /// <summary>The window has a minimize button. Cannot be combined with the WS_EX_CONTEXTHELP style. The WS_SYSMENU style must also be specified.</summary>
        WS_MINIMIZEBOX = 0x20000,

        /// <summary>The window is an overlapped window. An overlapped window has a title bar and a border.</summary>
        WS_OVERLAPPED = 0x0,

        /// <summary>The window is an overlapped window.</summary>
        WS_OVERLAPPEDWINDOW = WS_OVERLAPPED | WS_CAPTION | WS_SYSMENU | WS_SIZEFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX,

        /// <summary>The window is a pop-up window. This style cannot be used with the WS_CHILD style.</summary>
        WS_POPUP = 0x80000000u,

        /// <summary>The window is a pop-up window. The WS_CAPTION and WS_POPUPWINDOW styles must be combined to make the window menu visible.</summary>
        WS_POPUPWINDOW = WS_POPUP | WS_BORDER | WS_SYSMENU,

        /// <summary>The window has a sizing border.</summary>
        WS_SIZEFRAME = 0x40000,

        /// <summary>
        /// The window has a sizing border. Same as the WS_SIZEBOX style.
        /// </summary>
        WS_THICKFRAME = 0x00040000,

        /// <summary>The window has a window menu on its title bar. The WS_CAPTION style must also be specified.</summary>
        WS_SYSMENU = 0x80000,

        /// <summary>
        /// The window is a control that can receive the keyboard focus when the user presses the TAB key.
        /// Pressing the TAB key changes the keyboard focus to the next control with the WS_TABSTOP style.  
        /// You can turn this style on and off to change dialog box navigation. To change this style after a window has been created, use the SetWindowLong function.
        /// For user-created windows and modeless dialogs to work with tab stops, alter the message loop to call the IsDialogMessage function.
        /// </summary>
        WS_TABSTOP = 0x10000,

        /// <summary>The window is initially visible. This style can be turned on and off by using the ShowWindow or SetWindowPos function.</summary>
        WS_VISIBLE = 0x10000000,

        /// <summary>The window has a vertical scroll bar.</summary>
        WS_VSCROLL = 0x200000
    }

    public enum WindowLongParam : int
    {
        /// <summary>Sets a new address for the window procedure.</summary>
        /// <remarks>You cannot change this attribute if the window does not belong to the same process as the calling thread.</remarks>
        GWL_WNDPROC = -4,

        /// <summary>Sets a new application instance handle.</summary>
        GWLP_HINSTANCE = -6,

        GWLP_HWNDPARENT = -8,

        /// <summary>Sets a new identifier of the child window.</summary>
        /// <remarks>The window cannot be a top-level window.</remarks>
        GWL_ID = -12,

        /// <summary>Sets a new window style.</summary>
        GWL_STYLE = -16,

        /// <summary>Sets a new extended window style.</summary>
        /// <remarks>See <see cref="ExWindowStyles"/>.</remarks>
        GWL_EXSTYLE = -20,

        /// <summary>Sets the user data associated with the window.</summary>
        /// <remarks>This data is intended for use by the application that created the window. Its value is initially zero.</remarks>
        GWL_USERDATA = -21,

        /// <summary>Sets the return value of a message processed in the dialog box procedure.</summary>
        /// <remarks>Only applies to dialog boxes.</remarks>
        DWLP_MSGRESULT = 0,

        /// <summary>Sets new extra information that is private to the application, such as handles or pointers.</summary>
        /// <remarks>Only applies to dialog boxes.</remarks>
        DWLP_USER = 8,

        /// <summary>Sets the new address of the dialog box procedure.</summary>
        /// <remarks>Only applies to dialog boxes.</remarks>
        DWLP_DLGPROC = 4
    }

    [Flags()]
    public enum SetWindowPosFlags : uint
    {
        /// <summary>If the calling thread and the thread that owns the window are attached to different input queues, 
        /// the system posts the request to the thread that owns the window. This prevents the calling thread from 
        /// blocking its execution while other threads process the request.</summary>
        /// <remarks>SWP_ASYNCWINDOWPOS</remarks>
        AsynchronousWindowPosition = 0x4000,
        /// <summary>Prevents generation of the WM_SYNCPAINT message.</summary>
        /// <remarks>SWP_DEFERERASE</remarks>
        DeferErase = 0x2000,
        /// <summary>Draws a frame (defined in the window's class description) around the window.</summary>
        /// <remarks>SWP_DRAWFRAME</remarks>
        DrawFrame = 0x0020,
        /// <summary>Applies new frame styles set using the SetWindowLong function. Sends a WM_NCCALCSIZE message to 
        /// the window, even if the window's size is not being changed. If this flag is not specified, WM_NCCALCSIZE 
        /// is sent only when the window's size is being changed.</summary>
        /// <remarks>SWP_FRAMECHANGED</remarks>
        FrameChanged = 0x0020,
        /// <summary>Hides the window.</summary>
        /// <remarks>SWP_HIDEWINDOW</remarks>
        HideWindow = 0x0080,
        /// <summary>Does not activate the window. If this flag is not set, the window is activated and moved to the 
        /// top of either the topmost or non-topmost group (depending on the setting of the hWndInsertAfter 
        /// parameter).</summary>
        /// <remarks>SWP_NOACTIVATE</remarks>
        DoNotActivate = 0x0010,
        /// <summary>Discards the entire contents of the client area. If this flag is not specified, the valid 
        /// contents of the client area are saved and copied back into the client area after the window is sized or 
        /// repositioned.</summary>
        /// <remarks>SWP_NOCOPYBITS</remarks>
        DoNotCopyBits = 0x0100,
        /// <summary>Retains the current position (ignores X and Y parameters).</summary>
        /// <remarks>SWP_NOMOVE</remarks>
        IgnoreMove = 0x0002,
        /// <summary>Does not change the owner window's position in the Z order.</summary>
        /// <remarks>SWP_NOOWNERZORDER</remarks>
        DoNotChangeOwnerZOrder = 0x0200,
        /// <summary>Does not redraw changes. If this flag is set, no repainting of any kind occurs. This applies to 
        /// the client area, the nonclient area (including the title bar and scroll bars), and any part of the parent 
        /// window uncovered as a result of the window being moved. When this flag is set, the application must 
        /// explicitly invalidate or redraw any parts of the window and parent window that need redrawing.</summary>
        /// <remarks>SWP_NOREDRAW</remarks>
        DoNotRedraw = 0x0008,
        /// <summary>Same as the SWP_NOOWNERZORDER flag.</summary>
        /// <remarks>SWP_NOREPOSITION</remarks>
        DoNotReposition = 0x0200,
        /// <summary>Prevents the window from receiving the WM_WINDOWPOSCHANGING message.</summary>
        /// <remarks>SWP_NOSENDCHANGING</remarks>
        DoNotSendChangingEvent = 0x0400,
        /// <summary>Retains the current size (ignores the cx and cy parameters).</summary>
        /// <remarks>SWP_NOSIZE</remarks>
        IgnoreResize = 0x0001,
        /// <summary>Retains the current Z order (ignores the hWndInsertAfter parameter).</summary>
        /// <remarks>SWP_NOZORDER</remarks>
        IgnoreZOrder = 0x0004,
        /// <summary>Displays the window.</summary>
        /// <remarks>SWP_SHOWWINDOW</remarks>
        ShowWindow = 0x0040,
    }

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

    public enum ShowWindowCommands
    {
        /// <summary>
        /// Hides the window and activates another window.
        /// </summary>
        Hide = 0,

        /// <summary>
        /// Activates and displays a window. If the window is minimized or
        /// maximized, the system restores it to its original size and position.
        /// An application should specify this flag when displaying the window
        /// for the first time.
        /// </summary>
        Normal = 1,

        /// <summary>
        /// Activates the window and displays it as a minimized window.
        /// </summary>
        ShowMinimized = 2,

        /// <summary>
        /// Maximizes the specified window.
        /// </summary>
        Maximize = 3, // is this the right value?

        /// <summary>
        /// Activates the window and displays it as a maximized window.
        /// </summary>
        ShowMaximized = 3,

        /// <summary>
        /// Displays a window in its most recent size and position. This value
        /// is similar to <see cref="Win32.ShowWindowCommand.Normal"/>, except
        /// the window is not activated.
        /// </summary>
        ShowNoActivate = 4,

        /// <summary>
        /// Activates the window and displays it in its current size and position.
        /// </summary>
        Show = 5,

        /// <summary>
        /// Minimizes the specified window and activates the next top-level
        /// window in the Z order.
        /// </summary>
        Minimize = 6,

        /// <summary>
        /// Displays the window as a minimized window. This value is similar to
        /// <see cref="Win32.ShowWindowCommand.ShowMinimized"/>, except the
        /// window is not activated.
        /// </summary>
        ShowMinNoActive = 7,

        /// <summary>
        /// Displays the window in its current size and position. This value is
        /// similar to <see cref="Win32.ShowWindowCommand.Show"/>, except the
        /// window is not activated.
        /// </summary>
        ShowNA = 8,

        /// <summary>
        /// Activates and displays the window. If the window is minimized or
        /// maximized, the system restores it to its original size and position.
        /// An application should specify this flag when restoring a minimized window.
        /// </summary>
        Restore = 9,

        /// <summary>
        /// Sets the show state based on the SW_* value specified in the
        /// STARTUPINFO structure passed to the CreateProcess function by the
        /// program that started the application.
        /// </summary>
        ShowDefault = 10,

        /// <summary>
        ///  <b>Windows 2000/XP:</b> Minimizes a window, even if the thread
        /// that owns the window is not responding. This flag should only be
        /// used when minimizing windows from a different thread.
        /// </summary>
        ForceMinimize = 11
    }
}
