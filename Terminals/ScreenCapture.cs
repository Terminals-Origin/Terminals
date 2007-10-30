using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;

namespace Terminals
{
    [System.Security.SuppressUnmanagedCodeSecurity()]
    [System.Runtime.InteropServices.ComVisible(false)]
    internal sealed class NativeMethods
    {
        private NativeMethods() { }

        [StructLayout(LayoutKind.Sequential)]
        internal struct RECT
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

        [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        public static extern bool GetWindowRect(IntPtr hwnd, out RECT rect);

        [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        public static extern bool BringWindowToTop(IntPtr hWnd);

        public static Rectangle GetWindowRect(IntPtr hwnd)
        {
            RECT rect = new RECT();
            GetWindowRect(hwnd, out rect);
            return rect.Rect;
        }
    }

    public class ImageFormatHandler
    {
        public enum ImageFormatTypes
        {
            imgNone,
            imgBMP,
            imgEMF,
            imgEXIF,
            imgGIF,
            imgICON,
            imgJPEG,
            imgPNG,
            imgTIFF,
            imgWMF
        };

        private long encodingQuality = 50;

        private EncoderValue encodingRenderMethod = EncoderValue.RenderProgressive;

        private EncoderValue encodingScanMethod = EncoderValue.ScanMethodInterlaced;

        private long encodingColorDepth = 24;

        private EncoderValue encodingCompression = EncoderValue.CompressionLZW;

        private ImageFormatTypes defaultFormat;

        private ImageCodecInfo[] availableEncoders;

        private ImageCodecInfo[] availableDecoders;

        public ImageFormatTypes DefaultFormat
        {
            get { return defaultFormat; }
            set { defaultFormat = value; }
        }

        public ImageFormatHandler()
        {
            availableEncoders = ImageCodecInfo.GetImageEncoders();
            availableDecoders = ImageCodecInfo.GetImageDecoders();
        }

        public static ImageFormat GetImageFormat(ImageFormatTypes type)
        {
            switch (type)
            {
                case ImageFormatTypes.imgBMP:
                    return ImageFormat.Bmp;
                case ImageFormatTypes.imgEMF:
                    return ImageFormat.Emf;
                case ImageFormatTypes.imgEXIF:
                    return ImageFormat.Exif;
                case ImageFormatTypes.imgGIF:
                    return ImageFormat.Gif;
                case ImageFormatTypes.imgICON:
                    return ImageFormat.Icon;
                case ImageFormatTypes.imgJPEG:
                    return ImageFormat.Jpeg;
                case ImageFormatTypes.imgPNG:
                    return ImageFormat.Png;
                case ImageFormatTypes.imgTIFF:
                    return ImageFormat.Tiff;
                case ImageFormatTypes.imgWMF:
                    return ImageFormat.Wmf;
                default:
                    return null;
            }
        }

        public static String GetMimeType(ImageFormatTypes type)
        {
            String s = null;
            switch (type)
            {
                case ImageFormatTypes.imgBMP:
                    s = "bmp";
                    break;
                case ImageFormatTypes.imgEMF:
                    s = "x-emf";
                    break;
                case ImageFormatTypes.imgGIF:
                    s = "gif";
                    break;
                case ImageFormatTypes.imgICON:
                    s = "x-icon";
                    break;
                case ImageFormatTypes.imgJPEG:
                    s = "jpeg";
                    break;
                case ImageFormatTypes.imgPNG:
                    s = "png";
                    break;
                case ImageFormatTypes.imgTIFF:
                    s = "tiff";
                    break;
                case ImageFormatTypes.imgWMF:
                    s = "x-wmf";
                    break;
            }
            if (!String.IsNullOrEmpty(s))
                s = String.Format("image/{0}", s);

            return s;
        }

        public static ImageFormatTypes GetImageFormat(ImageFormat type)
        {
            if (type == ImageFormat.Bmp)
                return ImageFormatTypes.imgBMP;
            else
                if (type == ImageFormat.Emf)
                    return ImageFormatTypes.imgEMF;
                else
                    if (type == ImageFormat.Exif)
                        return ImageFormatTypes.imgEXIF;
                    else
                        if (type == ImageFormat.Gif)
                            return ImageFormatTypes.imgGIF;
                        else
                            if (type == ImageFormat.Icon)
                                return ImageFormatTypes.imgICON;
                            else
                                if (type == ImageFormat.Jpeg)
                                    return ImageFormatTypes.imgJPEG;
                                else
                                    if (type == ImageFormat.Png)
                                        return ImageFormatTypes.imgPNG;
                                    else
                                        if (type == ImageFormat.Tiff)
                                            return ImageFormatTypes.imgTIFF;
                                        else
                                            if (type == ImageFormat.Wmf)
                                                return ImageFormatTypes.imgWMF;
                                            else
                                                return ImageFormatTypes.imgNone;
        }

        public virtual ImageCodecInfo GetCodecInfo(ImageFormatTypes type)
        {
            ImageCodecInfo[] encoders;

            String mimeType = GetMimeType(type);

            if (!String.IsNullOrEmpty(mimeType))
            {
                for (int i = 0; i < 2; i++)
                {
                    if (i == 0)
                        encoders = availableEncoders;
                    else
                        encoders = availableDecoders;

                    foreach (ImageCodecInfo info in encoders)
                    {
                        if (info.MimeType == mimeType)
                            return info;
                    }
                }
            }
            return null;
        }

        public virtual EncoderParameters GetEncoderParameters(ImageFormatTypes type, out ImageCodecInfo info)
        {
            EncoderParameters parameters = null;
            info = GetCodecInfo(type);
            if (info != null)
            {
                System.Drawing.Imaging.Encoder encode;
                EncoderParameter param;
                switch (type)
                {
                    case ImageFormatTypes.imgGIF:
                        parameters = new EncoderParameters(2);
                        encode = System.Drawing.Imaging.Encoder.Version;
                        param = new EncoderParameter(encode, (long)EncoderValue.VersionGif89);
                        parameters.Param[0] = param;

                        encode = System.Drawing.Imaging.Encoder.ScanMethod;
                        param = new EncoderParameter(encode, (long)encodingScanMethod);
                        parameters.Param[1] = param;
                        break;

                    case ImageFormatTypes.imgJPEG:
                        parameters = new EncoderParameters(2);
                        encode = System.Drawing.Imaging.Encoder.RenderMethod;
                        param = new EncoderParameter(encode, (long)encodingRenderMethod);
                        parameters.Param[0] = param;

                        encode = System.Drawing.Imaging.Encoder.Quality;
                        param = new EncoderParameter(encode, encodingQuality);
                        parameters.Param[1] = param;
                        break;

                    case ImageFormatTypes.imgPNG:
                        parameters = new EncoderParameters(2);
                        encode = System.Drawing.Imaging.Encoder.RenderMethod;
                        param = new EncoderParameter(encode, (long)encodingRenderMethod);
                        parameters.Param[0] = param;

                        encode = System.Drawing.Imaging.Encoder.ScanMethod;
                        param = new EncoderParameter(encode, (long)encodingScanMethod);
                        parameters.Param[1] = param;
                        break;

                    case ImageFormatTypes.imgTIFF:
                        parameters = new EncoderParameters(2);
                        encode = System.Drawing.Imaging.Encoder.ColorDepth;
                        param = new EncoderParameter(encode, encodingColorDepth);
                        parameters.Param[0] = param;

                        encode = System.Drawing.Imaging.Encoder.Compression;
                        param = new EncoderParameter(encode, (long)encodingCompression);
                        parameters.Param[1] = param;
                        break;
                }
            }
            return parameters;
        }

        public virtual String GetDefaultFilenameExtension(ImageFormatTypes type)
        {
            String ext = "";
            ImageCodecInfo info = GetCodecInfo(type);

            if (info != null)
            {
                String[] extensions = info.FilenameExtension.Split(new char[] { ';' });
                ext = extensions[0];
                if (ext.StartsWith("*."))
                    ext = ext.Substring(2);
            }
            return ext;
        }
    }

	public class ScreenCapture
	{
		public enum CaptureType
		{
			VirtualScreen,
			PrimaryScreen,
			WorkingArea,
			AllScreens
		};

		private Bitmap image;
		private Bitmap[] images = null;
		private PrintDocument doc = new PrintDocument();

        private ImageFormatHandler formatHandler = null;
		
        public ImageFormatHandler FormatHandler
		{
			set { formatHandler = value; }
		}

		public ScreenCapture()
		{
			doc.PrintPage += new PrintPageEventHandler( printPage );
			formatHandler = new ImageFormatHandler();
		}

		public ScreenCapture( ImageFormatHandler formatHandler )
		{
			doc.PrintPage += new PrintPageEventHandler( printPage );

			this.formatHandler = formatHandler;
		}

		public virtual Bitmap Capture( Form window, String filename, ImageFormatHandler.ImageFormatTypes format )
		{
			return Capture( window, filename, format, false );
		}

		public virtual Bitmap Capture( Form window, String filename, ImageFormatHandler.ImageFormatTypes format, bool onlyClient )
		{
			Capture( window, onlyClient );
			Save( filename, format );
			return images[0];
		}

		public virtual Bitmap Capture( IntPtr handle, String filename, ImageFormatHandler.ImageFormatTypes format )
		{
			Capture( handle );
			Save( filename, format );
			return images[0];
		}

        public virtual Bitmap CaptureControl( Control window, String filename, ImageFormatHandler.ImageFormatTypes format )
		{
			CaptureControl( window );
			Save( filename, format );
			return images[0];
		}

		public virtual Bitmap CaptureControl( Control window )
		{
			Rectangle rc = window.RectangleToScreen( window.DisplayRectangle );
			return capture( window, rc );
		}

		public virtual Bitmap Capture( Form window, bool onlyClient )
		{
			if ( !onlyClient )
				return Capture( window );

			Rectangle rc = window.RectangleToScreen( window.ClientRectangle );
			return capture( window, rc );
		}

		public virtual Bitmap Capture( Form window )
		{
			Rectangle rc = new Rectangle( window.Location, window.Size );
			return capture( window, rc );
		}

		private Bitmap capture( Control window, Rectangle rc )
		{
			Bitmap memoryImage = null;
			images = new Bitmap[1];

			try
			{
				using ( Graphics graphics = window.CreateGraphics() )
				{
					memoryImage = new Bitmap( rc.Width, rc.Height, graphics );

					using ( Graphics memoryGrahics = Graphics.FromImage( memoryImage ) )
					{
						memoryGrahics.CopyFromScreen( rc.X, rc.Y, 0, 0, rc.Size, CopyPixelOperation.SourceCopy );
					}
				}
			}
			catch ( Exception ex )
			{
                Terminals.Logging.Log.Info("", ex);
				MessageBox.Show( ex.ToString(), "Capture failed", MessageBoxButtons.OK, MessageBoxIcon.Error );
			}
			images[0] = memoryImage;
			return memoryImage;
		}

		public virtual Bitmap Capture( IntPtr handle )
		{
			NativeMethods.BringWindowToTop( handle );
			CaptureHandleDelegateHandler dlg = new CaptureHandleDelegateHandler( CaptureHandle );
			IAsyncResult result = dlg.BeginInvoke( handle, null, null );
			return dlg.EndInvoke( result );
		}

		protected virtual Bitmap CaptureHandle( IntPtr handle )
		{
			Bitmap memoryImage = null;
			images = new Bitmap[1];
			try
			{
				using ( Graphics graphics = Graphics.FromHwnd( handle ) )
				{
					Rectangle rc = NativeMethods.GetWindowRect( handle );

					if ( (int)graphics.VisibleClipBounds.Width > 0 && (int)graphics.VisibleClipBounds.Height > 0 )
					{
						memoryImage = new Bitmap( rc.Width, rc.Height, graphics );

						using ( Graphics memoryGrahics = Graphics.FromImage( memoryImage ) )
						{
							memoryGrahics.CopyFromScreen( rc.X, rc.Y, 0, 0, rc.Size, CopyPixelOperation.SourceCopy );
						}
					}
				}
			}
			catch ( Exception ex )
			{
                Terminals.Logging.Log.Info("", ex);
				MessageBox.Show( ex.ToString(), "Capture failed", MessageBoxButtons.OK, MessageBoxIcon.Error );
			}
			images[0] = memoryImage;
			return memoryImage; 
		}

		public virtual Bitmap[] Capture( CaptureType typeOfCapture, String filename, ImageFormatHandler.ImageFormatTypes format )
		{
			Capture( typeOfCapture );
			Save( filename, format );
			return images;
		}

		public virtual Bitmap[] Capture( CaptureType typeOfCapture )
		{
			Bitmap memoryImage;
			int count = 1;

			try
			{
				Screen[] screens = Screen.AllScreens;
				Rectangle rc;
				switch ( typeOfCapture )
				{
					case CaptureType.PrimaryScreen:
						rc = Screen.PrimaryScreen.Bounds;
						break;
					case CaptureType.VirtualScreen:
						rc = SystemInformation.VirtualScreen;
						break;
					case CaptureType.WorkingArea:
						rc = Screen.PrimaryScreen.WorkingArea;
						break;
					case CaptureType.AllScreens:
						count = screens.Length;
						typeOfCapture = CaptureType.WorkingArea;
						rc = screens[0].WorkingArea;
						break;
					default:
						rc = SystemInformation.VirtualScreen;
						break;
				}
				images = new Bitmap[count];

				for ( int index = 0; index < count; index++ )
				{
					if ( index > 0 )
						rc = screens[index].WorkingArea;

					memoryImage = new Bitmap( rc.Width, rc.Height, PixelFormat.Format32bppArgb );

					using ( Graphics memoryGrahics = Graphics.FromImage( memoryImage ) )
					{
						memoryGrahics.CopyFromScreen( rc.X, rc.Y, 0, 0, rc.Size, CopyPixelOperation.SourceCopy );
					}
					images[index] = memoryImage;
				}
			}
			catch ( Exception ex )
			{
                Terminals.Logging.Log.Info("", ex);
				MessageBox.Show( ex.ToString(), "Capture failed", MessageBoxButtons.OK, MessageBoxIcon.Error );
			}
			return images;
		}

		public virtual void Print()
		{
			if ( images != null )
			{
				try
				{
					for ( int i = 0; i < images.Length; i++ )
					{
						image = images[i];
						doc.DefaultPageSettings.Landscape = ( image.Width > image.Height );
						doc.Print();
					}
				}
				catch ( Exception ex )
				{
                    Terminals.Logging.Log.Info("", ex);
					MessageBox.Show( ex.ToString(), "Capture failed", MessageBoxButtons.OK, MessageBoxIcon.Error );
				}
			}
		}

		private void printPage( object sender, PrintPageEventArgs e )
		{
			RectangleF rc = doc.DefaultPageSettings.Bounds;
			float ratio = (float)image.Height / (float)( image.Width != 0 ? image.Width : 1 );

			rc.Height = rc.Height - doc.DefaultPageSettings.Margins.Top - doc.DefaultPageSettings.Margins.Bottom;
			rc.Y = rc.Y + doc.DefaultPageSettings.Margins.Top;

			rc.Width = rc.Width - doc.DefaultPageSettings.Margins.Left - doc.DefaultPageSettings.Margins.Right;
			rc.X = rc.X + doc.DefaultPageSettings.Margins.Left;

			if ( rc.Height / rc.Width > ratio )
				rc.Height = rc.Width * ratio;
			else
				rc.Width = rc.Height / ( ratio != 0 ? ratio : 1 );

			e.Graphics.DrawImage( image, rc );
		}

		public virtual void Save( String filename, ImageFormatHandler.ImageFormatTypes format )
		{
			String directory = Path.GetDirectoryName( filename );
			String name = Path.GetFileNameWithoutExtension( filename );
			String ext = Path.GetExtension( filename );

			ext = formatHandler.GetDefaultFilenameExtension( format );

			if ( ext.Length == 0 )
			{
				format = ImageFormatHandler.ImageFormatTypes.imgPNG;
				ext = "png";
			}

			try
			{
				ImageCodecInfo info;
				EncoderParameters parameters = formatHandler.GetEncoderParameters( format, out info );

				for ( int i = 0; i < images.Length; i++ )
				{
					if ( images.Length > 1 )
					{
						filename = String.Format( "{0}\\{1}.{2:D2}.{3}", directory, name, i + 1, ext );
					}
					else
					{
						filename = String.Format( "{0}\\{1}.{2}", directory, name, ext );
					}
					image = images[i];

					if ( parameters != null )
					{
						image.Save( filename, info, parameters );
					}
					else
					{
						image.Save( filename, ImageFormatHandler.GetImageFormat( format ) );
					}
				}
			}
			catch ( Exception ex )
			{
                Terminals.Logging.Log.Info("", ex);
				string s = string.Format("Saving image to [{0}] in format [{1}].\n{2}", filename, format.ToString(), ex.ToString() );
				MessageBox.Show( s, "Capture failed", MessageBoxButtons.OK, MessageBoxIcon.Error );
			}
		}
	}

    public delegate Bitmap CaptureHandleDelegateHandler(IntPtr handle);

}
