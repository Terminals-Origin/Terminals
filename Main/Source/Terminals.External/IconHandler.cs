/*
 * IconHandler - by Gil Schmidt
 * 
 * - Originaly was writtin for a p2p project called Kibutz.
 * - The IconFromExtension might give you some problems if it won't find an
 *   icon for it( try/catch should be used).
 *
 * [Updated]
 * 
 * - Added Shell function for getting file extension after getting some commet
 *   about it, check link below.
 *   (http://www.codeguru.com/Csharp/Csharp/cs_misc/icons/article.php/c4261/)
 * - IconFromResource was added for having all the common needed functions for 
 *   handling icons.
 * - Readded DestroyIcon and adding GetManagedIcon, after verifying it's needed (thanks DrGui and Kenneth Broendum).
 * - Removed the IconFromExtension that pulled the icon from the registry (IconFromExtensionShell is default now).
 * - Fixed IconCount mistake (if the number of icon in the file was 0 and the
 *   index that was selected was also 0 the ExtractIconEx still got to run and
 *   crashed (thanks SickLab).
 * - Fixed iIcon (it was changed from IntPtr to int. on x64 systems IntPtr is 
 *   8 bytes which caused the mistake to popup) parameter in SHFILEINFO struct 
 *   (Thanks Stefan Mayr).
 * 
 * contact me at: Gil_Smdt@Hotmail.com
 * 
*/

using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Reflection;
using System.IO;

namespace IconHandler
{
	internal struct SHFILEINFO 
	{
		public IntPtr hIcon;		
		public int iIcon;		
		public uint dwAttributes;	
		[MarshalAs( UnmanagedType.ByValTStr, SizeConst = 260 )]
		public string szDisplayName;
		[MarshalAs( UnmanagedType.ByValTStr, SizeConst = 80 )]
		public string szTypeName;
	};

    public enum IconSize : uint
	{
		Large = 0x0,  //32x32
		Small = 0x1 //16x16		
	}

	//the function that will extract the icons from a file
    public class IconHandler
	{
		const uint SHGFI_ICON = 0x100;
		const uint SHGFI_USEFILEATTRIBUTES	= 0x10;

		[DllImport("Shell32", CharSet=CharSet.Auto)]
		internal extern static int ExtractIconEx (
			[MarshalAs(UnmanagedType.LPTStr)] 
			string lpszFile,                //size of the icon
			int nIconIndex,                 //index of the icon (in case we have more then 1 icon in the file
			IntPtr[] phIconLarge,           //32x32 icon
			IntPtr[] phIconSmall,           //16x16 icon
			int nIcons);                    //how many to get

		[DllImport("shell32.dll")]
		static extern IntPtr SHGetFileInfo(
			string pszPath,				//path
			uint dwFileAttributes,		//attributes
			ref SHFILEINFO psfi,		//struct pointer
			uint cbSizeFileInfo,		//size
			uint uFlags);	//flags

        //we need this function to release the unmanaged resource,
        //the unmanaged resource will be copies to a managed one and it will be returned.
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        extern static bool DestroyIcon(IntPtr handle);

		//will return an array of icons 
		public static Icon[] IconsFromFile(string Filename,IconSize Size)
		{
            try
            {
                int IconCount = ExtractIconEx(Filename, -1, null, null, 0); //checks how many icons.
                IntPtr[] IconPtr = new IntPtr[IconCount];
                Icon TempIcon;

                //extracts the icons by the size that was selected.
                if (Size == IconSize.Small)
                    ExtractIconEx(Filename, 0, null, IconPtr, IconCount);
                else
                    ExtractIconEx(Filename, 0, IconPtr, null, IconCount);

                Icon[] IconList = new Icon[IconCount];

                //gets the icons in a list.
                for (int i = 0; i < IconCount; i++)
                {
                    TempIcon = (Icon)Icon.FromHandle(IconPtr[i]);
                    IconList[i] = GetManagedIcon(ref TempIcon);
                }

                return IconList;
            }
            catch (Exception exc)
            {
                //Terminals.Logging.Error("Could not load icons from file:" + Filename, exc);
                return null;
            }
		}

		//extract one selected by index icon from a file.
		public static Icon IconFromFile(string Filename,IconSize Size,int Index)
		{
			int IconCount = ExtractIconEx(Filename,-1,null,null,0); //checks how many icons.
			if (IconCount <= 0 || Index >= IconCount) return null; // no icons were found.

            Icon TempIcon;
			IntPtr[] IconPtr = new IntPtr[1];

			//extracts the icon that we want in the selected size.
			if (Size == IconSize.Small)
				ExtractIconEx(Filename,Index,null,IconPtr,1);
			else
				ExtractIconEx(Filename,Index,IconPtr,null,1);

            TempIcon = Icon.FromHandle(IconPtr[0]);

			return GetManagedIcon(ref TempIcon);
		}
		public static Icon IconFromExtension(string Extension,IconSize Size)
		{
			try
			{
                Icon TempIcon;

                //add '.' if nessesry
				if (Extension[0] != '.') Extension = '.' + Extension;

				//temp struct for getting file shell info
				SHFILEINFO TempFileInfo = new SHFILEINFO();
				
				SHGetFileInfo(
					Extension,
					0, 
					ref TempFileInfo,
					(uint)Marshal.SizeOf(TempFileInfo),
					SHGFI_ICON | SHGFI_USEFILEATTRIBUTES | (uint) Size);

                TempIcon = (Icon) Icon.FromHandle(TempFileInfo.hIcon);
				return GetManagedIcon(ref TempIcon);
			}
			catch (Exception e)
			{
                //Terminals.Logging.Error("error while trying to get icon for " + Extension, e);
				return null;
			}
		}

        /*
         * this is the built in funcion provided by .Net framework for getting a file icon,
         * i thought it's worth mentioning.
         * 
        public static Icon IconFromExtension(string Filename)
        {
            return Icon.ExtractAssociatedIcon(Filename);
        }
        */

		public static Icon IconFromResource(string ResourceName)
		{
			Assembly TempAssembly = Assembly.GetCallingAssembly();

			return new Icon(TempAssembly.GetManifestResourceStream(ResourceName));
		}

		public static void SaveIconFromImage(Image SourceImage,string IconFilename,IconSize DestenationIconSize)
		{
			Size NewIconSize = DestenationIconSize == IconSize.Large ? new Size(32,32) : new Size(16,16);

			Bitmap RawImage = new Bitmap(SourceImage,NewIconSize);
			Icon TempIcon = Icon.FromHandle(RawImage.GetHicon());
			FileStream NewIconStream = new FileStream(IconFilename,FileMode.Create);

			TempIcon.Save(NewIconStream);

			NewIconStream.Close();
		}

		public static void SaveIcon(Icon SourceIcon,string IconFilename)
		{
			FileStream NewIconStream = new FileStream(IconFilename,FileMode.Create);

			SourceIcon.Save(NewIconStream);

			NewIconStream.Close();																						
		}

        public static Icon GetManagedIcon(ref Icon UnmanagedIcon)
        {
            Icon ManagedIcon = (Icon) UnmanagedIcon.Clone();

            DestroyIcon(UnmanagedIcon.Handle);

            return ManagedIcon;
        }
	}
}

/* EOF */
