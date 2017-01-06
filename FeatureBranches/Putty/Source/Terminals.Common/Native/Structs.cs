using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Terminals.Native
{
    #region Structs

    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int left;
        public int top;
        public int right;
        public int bottom;

        public RECT(int left, int top, int right, int bottom)
        {
            this.left = left;
            this.top = top;
            this.right = right;
            this.bottom = bottom;
        }

        public Rectangle Rect
        {
            get { return new Rectangle(this.left, this.top, this.right - this.left, this.bottom - this.top); }
        }

        public static RECT FromXYWH(int x, int y, int width, int height)
        {
            return new RECT(x, y, x + width, y + height);
        }

        public static RECT FromRectangle(Rectangle rect)
        {
            return new RECT(rect.Left, rect.Top, rect.Right, rect.Bottom);
        }
    }

    [Serializable, StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct TEXTMETRIC
    {
        public int tmHeight;
        public int tmAscent;
        public int tmDescent;
        public int tmInternalLeading;
        public int tmExternalLeading;
        public int tmAveCharWidth;
        public int tmMaxCharWidth;
        public int tmWeight;
        public int tmOverhang;
        public int tmDigitizedAspectX;
        public int tmDigitizedAspectY;
        public char tmFirstChar;
        public char tmLastChar;
        public char tmDefaultChar;
        public char tmBreakChar;
        public byte tmItalic;
        public byte tmUnderlined;
        public byte tmStruckOut;
        public byte tmPitchAndFamily;
        public byte tmCharSet;
    }

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

    #endregion
}
