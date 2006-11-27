using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Terminals
{
    public class SHFileOperationWrapper
    {
		public enum FileOperations
		{
			FO_MOVE           = 0x0001,		// Move the files specified in pFrom to the location specified in pTo. 
			FO_COPY           = 0x0002,		// Copy the files specified in the pFrom member to the location specified 
			// in the pTo member. 
			FO_DELETE         = 0x0003,		// Delete the files specified in pFrom. 
			FO_RENAME         = 0x0004		// Rename the file specified in pFrom. You cannot use this flag to rename 
			// multiple files with a single function call. Use FO_MOVE instead. 
		}

		[Flags]
		public enum ShellFileOperationFlags
		{
			FOF_MULTIDESTFILES         = 0x0001,	// The pTo member specifies multiple destination files (one for 
			// each source file) rather than one directory where all source 
			// files are to be deposited. 
			FOF_CONFIRMMOUSE           = 0x0002,	// Not currently used. 
			FOF_SILENT                 = 0x0004,	// Do not display a progress dialog box. 
			FOF_RENAMEONCOLLISION      = 0x0008,	// Give the file being operated on a new name in a move, copy, or 
			// rename operation if a file with the target name already exists. 
			FOF_NOCONFIRMATION         = 0x0010,	// Respond with "Yes to All" for any dialog box that is displayed. 
			FOF_WANTMAPPINGHANDLE      = 0x0020,	// If FOF_RENAMEONCOLLISION is specified and any files were renamed,
			// assign a name mapping object containing their old and new names 
			// to the hNameMappings member.
			FOF_ALLOWUNDO              = 0x0040,	// Preserve Undo information, if possible. If pFrom does not 
			// contain fully qualified path and file names, this flag is ignored. 
			FOF_FILESONLY              = 0x0080,	// Perform the operation on files only if a wildcard file 
			// name (*.*) is specified. 
			FOF_SIMPLEPROGRESS         = 0x0100,	// Display a progress dialog box but do not show the file names. 
			FOF_NOCONFIRMMKDIR         = 0x0200,	// Do not confirm the creation of a new directory if the operation
			// requires one to be created. 
			FOF_NOERRORUI              = 0x0400,	// Do not display a user interface if an error occurs. 
			FOF_NOCOPYSECURITYATTRIBS  = 0x0800,	// Do not copy the security attributes of the file.
			FOF_NORECURSION            = 0x1000,	// Only operate in the local directory. Don't operate recursively
			// into subdirectories.
			FOF_NO_CONNECTED_ELEMENTS  = 0x2000,	// Do not move connected files as a group. Only move the 
			// specified files. 
			FOF_WANTNUKEWARNING        = 0x4000,	// Send a warning if a file is being destroyed during a delete 
			// operation rather than recycled. This flag partially 
			// overrides FOF_NOCONFIRMATION.
			FOF_NORECURSEREPARSE       = 0x8000		// Treat reparse points as objects, not containers.

		}
		
		[Flags]
		public enum ShellChangeNotificationEvents : uint
		{
			SHCNE_RENAMEITEM          = 0x00000001,	// The name of a nonfolder item has changed. SHCNF_IDLIST or 
													// SHCNF_PATH must be specified in uFlags. dwItem1 contains the 
													// previous PIDL or name of the item. dwItem2 contains the new PIDL
													// or name of the item. 
			SHCNE_CREATE              = 0x00000002,	// A nonfolder item has been created. SHCNF_IDLIST or SHCNF_PATH 
													// must be specified in uFlags. dwItem1 contains the item that was 
													// created. dwItem2 is not used and should be NULL. 
			SHCNE_DELETE              = 0x00000004,	// A nonfolder item has been deleted. SHCNF_IDLIST or SHCNF_PATH
													// must be specified in uFlags. dwItem1 contains the item that was 
													// deleted. dwItem2 is not used and should be NULL. 
			SHCNE_MKDIR               = 0x00000008,	// A folder has been created. SHCNF_IDLIST or SHCNF_PATH must be 
													// specified in uFlags. dwItem1 contains the folder that was 
													// created. dwItem2 is not used and should be NULL. 
			SHCNE_RMDIR               = 0x00000010,	// A folder has been removed. SHCNF_IDLIST or SHCNF_PATH must be 
													// specified in uFlags. dwItem1 contains the folder that was 
													// removed. dwItem2 is not used and should be NULL. 
			SHCNE_MEDIAINSERTED       = 0x00000020,	// Storage media has been inserted into a drive. SHCNF_IDLIST or
													// SHCNF_PATH must be specified in uFlags. dwItem1 contains the root
													// of the drive that contains the new media. dwItem2 is not used 
													// and should be NULL. 
			SHCNE_MEDIAREMOVED        = 0x00000040,	// Storage media has been removed from a drive. SHCNF_IDLIST or 
													// SHCNF_PATH must be specified in uFlags. dwItem1 contains the root
													// of the drive from which the media was removed. dwItem2 is not 
													// used and should be NULL. 
			SHCNE_DRIVEREMOVED        = 0x00000080,	// A drive has been removed. SHCNF_IDLIST or SHCNF_PATH must be 
													// specified in uFlags. dwItem1 contains the root of the drive that
													// was removed. dwItem2 is not used and should be NULL. 
			SHCNE_DRIVEADD            = 0x00000100,	// A drive has been added. SHCNF_IDLIST or SHCNF_PATH must be 
													// specified in uFlags. dwItem1 contains the root of the drive that
													// was added. dwItem2 is not used and should be NULL. 
			SHCNE_NETSHARE            = 0x00000200,	// A folder on the local computer is being shared via the network.
													// SHCNF_IDLIST or SHCNF_PATH must be specified in uFlags. dwItem1
													// contains the folder that is being shared. dwItem2 is not used and
													// should be NULL. 
			SHCNE_NETUNSHARE          = 0x00000400,	// A folder on the local computer is no longer being shared via the
													// network. SHCNF_IDLIST or SHCNF_PATH must be specified in uFlags. 
													// dwItem1 contains the folder that is no longer being shared. 
													// dwItem2 is not used and should be NULL. 
			SHCNE_ATTRIBUTES          = 0x00000800,	// The attributes of an item or folder have changed. SHCNF_IDLIST
													// or SHCNF_PATH must be specified in uFlags. dwItem1 contains the
													// item or folder that has changed. dwItem2 is not used and should
													// be NULL. 
			SHCNE_UPDATEDIR           = 0x00001000,	// The contents of an existing folder have changed, but the folder
													// still exists and has not been renamed. SHCNF_IDLIST or SHCNF_PATH
													// must be specified in uFlags. dwItem1 contains the folder that 
													// has changed. dwItem2 is not used and should be NULL. If a folder
													// has been created, deleted, or renamed, use SHCNE_MKDIR, 
													// SHCNE_RMDIR, or SHCNE_RENAMEFOLDER, respectively, instead. 
			SHCNE_UPDATEITEM          = 0x00002000,	// An existing nonfolder item has changed, but the item still exists
													// and has not been renamed. SHCNF_IDLIST or SHCNF_PATH must be 
													// specified in uFlags. dwItem1 contains the item that has changed.
													// dwItem2 is not used and should be NULL. If a nonfolder item has 
													// been created, deleted, or renamed, use SHCNE_CREATE, 
													// SHCNE_DELETE, or SHCNE_RENAMEITEM, respectively, instead. 
			SHCNE_SERVERDISCONNECT    = 0x00004000,	// The computer has disconnected from a server. SHCNF_IDLIST or 
													// SHCNF_PATH must be specified in uFlags. dwItem1 contains the 
													// server from which the computer was disconnected. dwItem2 is not
													// used and should be NULL.
			SHCNE_UPDATEIMAGE         = 0x00008000,	// An image in the system image list has changed. SHCNF_DWORD must be 
													// specified in uFlags. dwItem1 contains the index in the system image 
													// list that has changed. dwItem2 is not used and should be NULL. 
			SHCNE_DRIVEADDGUI         = 0x00010000,	// A drive has been added and the Shell should create a new window
													// for the drive. SHCNF_IDLIST or SHCNF_PATH must be specified in 
													// uFlags. dwItem1 contains the root of the drive that was added. 
													// dwItem2 is not used and should be NULL. 
			SHCNE_RENAMEFOLDER        = 0x00020000,	// The name of a folder has changed. SHCNF_IDLIST or SHCNF_PATH must
													// be specified in uFlags. dwItem1 contains the previous pointer to
													// an item identifier list (PIDL) or name of the folder. dwItem2 
													// contains the new PIDL or name of the folder. 
			SHCNE_FREESPACE           = 0x00040000,	// The amount of free space on a drive has changed. SHCNF_IDLIST or
													// SHCNF_PATH must be specified in uFlags. dwItem1 contains the root
													// of the drive on which the free space changed. dwItem2 is not used
													// and should be NULL. 
			SHCNE_EXTENDED_EVENT      = 0x04000000,	// Not currently used. 
			SHCNE_ASSOCCHANGED        = 0x08000000,	// A file type association has changed. SHCNF_IDLIST must be 
													// specified in the uFlags parameter. dwItem1 and dwItem2 are not
													// used and must be NULL. 
			SHCNE_DISKEVENTS          = 0x0002381F,	// Specifies a combination of all of the disk event identifiers. 
			SHCNE_GLOBALEVENTS        = 0x0C0581E0,	// Specifies a combination of all of the global event identifiers. 
			SHCNE_ALLEVENTS           = 0x7FFFFFFF,	// All events have occurred. 
			SHCNE_INTERRUPT           = 0x80000000	// The specified event occurred as a result of a system interrupt.
													// As this value modifies other event values, it cannot be used alone.
		}


		public enum ShellChangeNotificationFlags
		{
			SHCNF_IDLIST		= 0x0000,	// dwItem1 and dwItem2 are the addresses of ITEMIDLIST structures that
											// represent the item(s) affected by the change. Each ITEMIDLIST must be 
											// relative to the desktop folder. 
			SHCNF_PATHA			= 0x0001,	// dwItem1 and dwItem2 are the addresses of null-terminated strings of 
											// maximum length MAX_PATH that contain the full path names of the items 
											// affected by the change. 
			SHCNF_PRINTERA		= 0x0002,	// dwItem1 and dwItem2 are the addresses of null-terminated strings that 
											// represent the friendly names of the printer(s) affected by the change. 
			SHCNF_DWORD			= 0x0003,	// The dwItem1 and dwItem2 parameters are DWORD values. 
			SHCNF_PATHW			= 0x0005,	// like SHCNF_PATHA but unicode string
			SHCNF_PRINTERW		= 0x0006,	// like SHCNF_PRINTERA but unicode string
			SHCNF_TYPE			= 0x00FF,	
			SHCNF_FLUSH			= 0x1000,	// The function should not return until the notification has been delivered 
											// to all affected components. As this flag modifies other data-type flags,
											// it cannot by used by itself.
			SHCNF_FLUSHNOWAIT	= 0x2000	// The function should begin delivering notifications to all affected 
											// components but should return as soon as the notification process has
											// begun. As this flag modifies other data-type flags, it cannot by used 
											// by itself.
		}

		// properties
		public FileOperations Operation;
		public IntPtr OwnerWindow;
		public ShellFileOperationFlags OperationFlags;
		public String ProgressTitle;
		public String[] SourceFiles;
		public String[] DestFiles;

		public SHFileOperationWrapper()
		{
			// set default properties
			Operation = FileOperations.FO_COPY;
			OwnerWindow = IntPtr.Zero;
			OperationFlags = ShellFileOperationFlags.FOF_ALLOWUNDO 
				| ShellFileOperationFlags.FOF_MULTIDESTFILES
				| ShellFileOperationFlags.FOF_NO_CONNECTED_ELEMENTS
				| ShellFileOperationFlags.FOF_WANTNUKEWARNING;
			ProgressTitle = "";
		}

		public bool DoOperation()
		{
			SHApi.SHFILEOPSTRUCT FileOpStruct = new SHApi.SHFILEOPSTRUCT();
			
			FileOpStruct.hwnd = OwnerWindow;
			FileOpStruct.wFunc = (uint)Operation;

			String multiSource = StringArrayToMultiString(SourceFiles);
			String multiDest = StringArrayToMultiString(DestFiles);
			FileOpStruct.pFrom = Marshal.StringToHGlobalUni(multiSource);
			FileOpStruct.pTo = Marshal.StringToHGlobalUni(multiDest);
			
			FileOpStruct.fFlags = (ushort)OperationFlags;
			FileOpStruct.lpszProgressTitle = ProgressTitle;
			FileOpStruct.fAnyOperationsAborted = 0;
			FileOpStruct.hNameMappings = IntPtr.Zero;

			int RetVal;
			RetVal = SHApi.SHFileOperation(ref FileOpStruct);
			
			SHApi.SHChangeNotify(
				(uint)ShellChangeNotificationEvents.SHCNE_ALLEVENTS,
				(uint)ShellChangeNotificationFlags.SHCNF_DWORD,
				IntPtr.Zero,
				IntPtr.Zero);

			if (RetVal!=0)
				return false;

			if (FileOpStruct.fAnyOperationsAborted != 0)
				return false;

			return true;
		}
		
		private String StringArrayToMultiString(String[] stringArray)
		{
			String multiString = "";

			if (stringArray == null)
				return "";

			for (int i=0 ; i<stringArray.Length ; i++)
				multiString += stringArray[i] + '\0';
			
			multiString += '\0';
			
			return multiString;
		}
    }
}
