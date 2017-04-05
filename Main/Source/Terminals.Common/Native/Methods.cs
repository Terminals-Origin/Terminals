using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Windows.Forms;

namespace Terminals.Native
{
    [SuppressUnmanagedCodeSecurity()]
    [ComVisible(false)]
    public class Methods
    {
        #region User32.dll

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll")]
        public static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, SetWindowPosFlags uFlags);

        [DllImport("user32.dll")]
        public static extern bool ClientToScreen(IntPtr hWnd, ref Point lpPoint);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SetFocus(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SetActiveWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int PostMessage(HandleRef hwnd, int msg, IntPtr wparam, IntPtr lparam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern bool ShowWindow(HandleRef hWnd, int nCmdShow);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern bool SetForegroundWindow(HandleRef hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr GetWindowDC(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDc);

        [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        public static extern bool GetWindowRect(IntPtr hwnd, out RECT rect);

        [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        public static extern bool BringWindowToTop(IntPtr hWnd);

        public static Rectangle GetClientRect(IntPtr hWnd)
        {
            RECT rect = new RECT();
            GetClientRect(hWnd, out rect);
            return rect.Rect;
        }
        public static Rectangle GetWindowRect(IntPtr hWnd)
        {
            RECT rect = new RECT();
            GetWindowRect(hWnd, out rect);
            return rect.Rect;
        }

        #endregion

        #region Gdi32.dll

        [DllImport("gdi32.dll", CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetTextMetrics(IntPtr hdc, out TEXTMETRIC lptm);

        [DllImport("gdi32.dll", CharSet = CharSet.Unicode)]
        public static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);

        [DllImport("gdi32.dll", CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteObject(IntPtr hdc);

        #endregion

        #region UxTheme.dll

        [DllImport("uxtheme.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
        public static extern int SetWindowTheme(IntPtr hWnd, String appName, String partList);

        #endregion

        #region Shell32.dll

        // Retrieves a pointer to the Shell's IMalloc interface.
        [DllImport("shell32.dll")]
        public static extern Int32 SHGetMalloc(
            out IntPtr hObject);

        // Address of a pointer that receives the Shell's IMalloc interface pointer. 

        // Retrieves the path of a folder as an PIDL.
        [DllImport("shell32.dll")]
        public static extern Int32 SHGetFolderLocation(
            IntPtr hwndOwner, // Handle to the owner window.
            Int32 nFolder, // A CSIDL value that identifies the folder to be located.
            IntPtr hToken, // Token that can be used to represent a particular user.
            UInt32 dwReserved, // Reserved.
            out IntPtr ppidl);

        // Address of a pointer to an item identifier list structure 
        // specifying the folder's location relative to the root of the namespace 
        // (the desktop). 

        // Converts an item identifier list to a file system path. 
        [DllImport("shell32.dll")]
        public static extern Int32 SHGetPathFromIDList(
            IntPtr pidl, // Address of an item identifier list that specifies a file or directory location 
            // relative to the root of the namespace (the desktop). 
            StringBuilder pszPath);

        // Address of a buffer to receive the file system path.

        // Takes the CSIDL of a folder and returns the pathname.
        [DllImport("shell32.dll")]
        public static extern Int32 SHGetFolderPath(
            IntPtr hwndOwner, // Handle to an owner window.
            Int32 nFolder, // A CSIDL value that identifies the folder whose path is to be retrieved.
            IntPtr hToken, // An access token that can be used to represent a particular user.
            UInt32 dwFlags, // Flags to specify which path is to be returned. It is used for cases where 
            // the folder associated with a CSIDL may be moved or renamed by the user. 
            StringBuilder pszPath);

        // Pointer to a null-terminated string which will receive the path.

        // Translates a Shell namespace object's display name into an item identifier list and returns the attributes 
        // of the object. This function is the preferred method to convert a string to a pointer to an item 
        // identifier list (PIDL). 
        [DllImport("shell32.dll")]
        public static extern Int32 SHParseDisplayName(
            [MarshalAs(UnmanagedType.LPWStr)] String pszName, // Pointer to a zero-terminated wide string that contains the display name 
            // to parse. 
            IntPtr pbc, // Optional bind context that controls the parsing operation. This parameter 
            // is normally set to NULL.
            out IntPtr ppidl, // Address of a pointer to a variable of type ITEMIDLIST that receives the item
            // identifier list for the object.
            UInt32 sfgaoIn, // ULONG value that specifies the attributes to query.
            out UInt32 psfgaoOut);

        // Pointer to a ULONG. On return, those attributes that are true for the 
        // object and were requested in sfgaoIn will be set. 

        // Retrieves the IShellFolder interface for the desktop folder, which is the root of the Shell's namespace. 
        [DllImport("shell32.dll")]
        public static extern Int32 SHGetDesktopFolder(
            out IntPtr ppshf);

        // Address that receives an IShellFolder interface pointer for the 
        // desktop folder.

        // This function takes the fully-qualified pointer to an item identifier list (PIDL) of a namespace object, 
        // and returns a specified interface pointer on the parent object.
        [DllImport("shell32.dll")]
        public static extern Int32 SHBindToParent(
            IntPtr pidl, // The item's PIDL. 
            [MarshalAs(UnmanagedType.LPStruct)] Guid riid, // The REFIID of one of the interfaces exposed by the item's parent object. 
            out IntPtr ppv, // A pointer to the interface specified by riid. You must release the object when 
            // you are finished. 
            ref IntPtr ppidlLast);

        // Displays a dialog box that enables the user to select a Shell folder. 
        [DllImport("shell32.dll")]
        public static extern IntPtr SHBrowseForFolder(
            ref BROWSEINFO lbpi);

        // Pointer to a BROWSEINFO structure that contains information used to display 
        // the dialog box. 

        // Performs an operation on a specified file.
        [DllImport("shell32.dll")]
        public static extern IntPtr ShellExecute(
            IntPtr hwnd, // Handle to a parent window.
            [MarshalAs(UnmanagedType.LPStr)] String lpOperation, // Pointer to a null-terminated string, referred to in this case as a verb, 
            // that specifies the action to be performed.
            [MarshalAs(UnmanagedType.LPStr)] String lpFile, // Pointer to a null-terminated string that specifies the file or object on which 
            // to execute the specified verb.
            [MarshalAs(UnmanagedType.LPStr)] String lpParameters, // If the lpFile parameter specifies an executable file, lpParameters is a pointer 
            // to a null-terminated string that specifies the parameters to be passed 
            // to the application.
            [MarshalAs(UnmanagedType.LPStr)] String lpDirectory, // Pointer to a null-terminated string that specifies the default directory. 
            Int32 nShowCmd);

        // Flags that specify how an application is to be displayed when it is opened.

        // Performs an action on a file. 
        [DllImport("shell32.dll")]
        public static extern Int32 ShellExecuteEx(
            ref SHELLEXECUTEINFO lpExecInfo);

        // Address of a SHELLEXECUTEINFO structure that contains and receives 
        // information about the application being executed. 

        // Copies, moves, renames, or deletes a file system object. 
        [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
        public static extern Int32 SHFileOperation(
            ref SHFILEOPSTRUCT lpFileOp);

        // Address of an SHFILEOPSTRUCT structure that contains information 
        // this function needs to carry out the specified operation. This 
        // parameter must contain a valid value that is not NULL. You are 
        // responsibile for validating the value. If you do not validate it, 
        // you will experience unexpected results. 

        // Notifies the system of an event that an application has performed. An application should use this function
        // if it performs an action that may affect the Shell. 
        [DllImport("shell32.dll")]
        public static extern void SHChangeNotify(
            UInt32 wEventId, // Describes the event that has occurred. the 
            // ShellChangeNotificationEvents enum contains a list of options.
            UInt32 uFlags, // Flags that indicate the meaning of the dwItem1 and dwItem2 parameters.
            IntPtr dwItem1, // First event-dependent value. 
            IntPtr dwItem2);

        // Second event-dependent value. 

        // Adds a document to the Shell's list of recently used documents or clears all documents from the list. 
        [DllImport("shell32.dll")]
        public static extern void SHAddToRecentDocs(
            UInt32 uFlags, // Flag that indicates the meaning of the pv parameter.
            IntPtr pv);

        // A pointer to either a null-terminated string with the path and file name 
        // of the document, or a PIDL that identifies the document's file object. 
        // Set this parameter to NULL to clear all documents from the list. 
        [DllImport("shell32.dll")]
        public static extern void SHAddToRecentDocs(
            UInt32 uFlags,
            [MarshalAs(UnmanagedType.LPWStr)] String pv);

        // Executes a command on a printer object. 
        [DllImport("shell32.dll")]
        public static extern Int32 SHInvokePrinterCommand
            (
            IntPtr hwnd, // Handle of the window that will be used as the parent of any windows 
            // or dialog boxes that are created during the operation.
            UInt32 uAction, // A value that determines the type of printer operation that will be 
            // performed.
            [MarshalAs(UnmanagedType.LPWStr)] String lpBuf1, // Address of a null_terminated string that contains additional 
            // information for the printer command. 
            [MarshalAs(UnmanagedType.LPWStr)] String lpBuf2, // Address of a null-terminated string that contains additional
            // information for the printer command. 
            Int32 fModal
            );

        //  value that determines whether SHInvokePrinterCommand should return
        // after initializing the command or wait until the command is completed.

        #endregion

        #region ShlwApi.dll
        
        // The item's PIDL relative to the parent folder. This PIDL can be used with many
        // of the methods supported by the parent folder's interfaces. If you set ppidlLast 
        // to NULL, the PIDL will not be returned. 

        // Accepts a STRRET structure returned by IShellFolder::GetDisplayNameOf that contains or points to a 
        // string, and then returns that string as a BSTR.
        [DllImport("shlwapi.dll")]
        public static extern Int32 StrRetToBSTR(
            ref STRRET pstr, // Pointer to a STRRET structure.
            IntPtr pidl, // Pointer to an ITEMIDLIST uniquely identifying a file object or subfolder relative
            // to the parent folder.
            [MarshalAs(UnmanagedType.BStr)] out String pbstr);

        // Pointer to a variable of type BSTR that contains the converted string.

        // Takes a STRRET structure returned by IShellFolder::GetDisplayNameOf, converts it to a string, and 
        // places the result in a buffer. 
        [DllImport("shlwapi.dll")]
        public static extern Int32 StrRetToBuf(
            ref STRRET pstr, // Pointer to the STRRET structure. When the function returns, this pointer will no
            // longer be valid.
            IntPtr pidl, // Pointer to the item's ITEMIDLIST structure.
            StringBuilder pszBuf, // Buffer to hold the display name. It will be returned as a null-terminated
            // string. If cchBuf is too small, the name will be truncated to fit. 
            UInt32 cchBuf);

        // Size of pszBuf, in characters. If cchBuf is too small, the string will be 
        // truncated to fit.

        #endregion
    }
}
