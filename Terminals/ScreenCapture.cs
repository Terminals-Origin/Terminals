using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Windows.Forms;
using System.IO;
using Terminals.Configuration;

namespace Terminals
{
    public delegate Bitmap CaptureHandleDelegateHandler(IntPtr handle);

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
        }

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
            get
            {
                return this.defaultFormat;
            }

            set
            {
                this.defaultFormat = value;
            }
        }

        public ImageFormatHandler()
        {
            this.availableEncoders = ImageCodecInfo.GetImageEncoders();
            this.availableDecoders = ImageCodecInfo.GetImageDecoders();
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

        public static string GetMimeType(ImageFormatTypes type)
        {
            string s = null;
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
                s = string.Format("image/{0}", s);

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

            string mimeType = GetMimeType(type);

            if (!string.IsNullOrEmpty(mimeType))
            {
                for (int i = 0; i < 2; i++)
                {
                    if (i == 0)
                        encoders = this.availableEncoders;
                    else
                        encoders = this.availableDecoders;

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
            info = this.GetCodecInfo(type);
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
                        param = new EncoderParameter(encode, (long)this.encodingScanMethod);
                        parameters.Param[1] = param;
                        break;

                    case ImageFormatTypes.imgJPEG:
                        parameters = new EncoderParameters(2);
                        encode = System.Drawing.Imaging.Encoder.RenderMethod;
                        param = new EncoderParameter(encode, (long)this.encodingRenderMethod);
                        parameters.Param[0] = param;

                        encode = System.Drawing.Imaging.Encoder.Quality;
                        param = new EncoderParameter(encode, this.encodingQuality);
                        parameters.Param[1] = param;
                        break;

                    case ImageFormatTypes.imgPNG:
                        parameters = new EncoderParameters(2);
                        encode = System.Drawing.Imaging.Encoder.RenderMethod;
                        param = new EncoderParameter(encode, (long)this.encodingRenderMethod);
                        parameters.Param[0] = param;

                        encode = System.Drawing.Imaging.Encoder.ScanMethod;
                        param = new EncoderParameter(encode, (long)this.encodingScanMethod);
                        parameters.Param[1] = param;
                        break;

                    case ImageFormatTypes.imgTIFF:
                        parameters = new EncoderParameters(2);
                        encode = System.Drawing.Imaging.Encoder.ColorDepth;
                        param = new EncoderParameter(encode, this.encodingColorDepth);
                        parameters.Param[0] = param;

                        encode = System.Drawing.Imaging.Encoder.Compression;
                        param = new EncoderParameter(encode, (long)this.encodingCompression);
                        parameters.Param[1] = param;
                        break;
                }
            }

            return parameters;
        }

        public virtual string GetDefaultFilenameExtension(ImageFormatTypes type)
        {
            string ext = string.Empty;
            ImageCodecInfo info = this.GetCodecInfo(type);

            if (info != null)
            {
                string[] extensions = info.FilenameExtension.Split(new char[] { ';' });
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
        }

        private Bitmap image;
        private Bitmap[] images = null;
        private PrintDocument doc = new PrintDocument();

        private ImageFormatHandler formatHandler = null;
        
        public ImageFormatHandler FormatHandler
        {
            set
            {
                this.formatHandler = value;
            }
        }

        public ScreenCapture()
        {
            this.doc.PrintPage += new PrintPageEventHandler(this.printPage);
            this.formatHandler = new ImageFormatHandler();
        }

        public ScreenCapture(ImageFormatHandler formatHandler)
        {
            this.doc.PrintPage += new PrintPageEventHandler(this.printPage);
            this.formatHandler = formatHandler;
        }

        public virtual Bitmap Capture(Form window, string filename, ImageFormatHandler.ImageFormatTypes format)
        {
            return this.Capture(window, filename, format, false);
        }

        public virtual Bitmap Capture(Form window, string filename, ImageFormatHandler.ImageFormatTypes format, bool onlyClient)
        {
            this.Capture(window, onlyClient);
            this.Save(filename, format);
            return this.images[0];
        }

        public virtual Bitmap Capture(IntPtr handle, string filename, ImageFormatHandler.ImageFormatTypes format)
        {
            this.Capture(handle);
            this.Save(filename, format);
            return this.images[0];
        }

        public virtual Bitmap CaptureControl(Control window, string filename, ImageFormatHandler.ImageFormatTypes format)
        {
            this.CaptureControl(window);

            if (Settings.EnableCaptureToFolder)
                this.Save(filename, format);

            return this.images[0];
        }

        public virtual Bitmap CaptureControl(Control window)
        {
            Rectangle rc = window.RectangleToScreen(window.DisplayRectangle);
            return this.capture(window, rc);
        }

        public virtual Bitmap Capture(Form window, bool onlyClient)
        {
            if (!onlyClient)
                return this.Capture(window);

            Rectangle rc = window.RectangleToScreen(window.ClientRectangle);
            return this.capture(window, rc);
        }

        public virtual Bitmap Capture(Form window)
        {
            Rectangle rc = new Rectangle(window.Location, window.Size);
            return this.capture(window, rc);
        }

        private Bitmap capture(Control window, Rectangle rc)
        {
            Bitmap memoryImage = null;
            this.images = new Bitmap[1];

            try
            {
                using (Graphics graphics = window.CreateGraphics())
                {
                    memoryImage = new Bitmap(rc.Width, rc.Height, graphics);

                    using (Graphics memoryGrahics = Graphics.FromImage(memoryImage))
                    {
                        memoryGrahics.CopyFromScreen(rc.X, rc.Y, 0, 0, rc.Size, CopyPixelOperation.SourceCopy);
                    }
                }
            }
            catch (Exception ex)
            {
                Terminals.Logging.Log.Error("Screen Capture Failed", ex);
                MessageBox.Show(ex.ToString(), "Capture failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            this.images[0] = memoryImage;
            return memoryImage;
        }

        public virtual Bitmap Capture(IntPtr handle)
        {
            Native.Methods.BringWindowToTop(handle);
            CaptureHandleDelegateHandler dlg = new CaptureHandleDelegateHandler(this.CaptureHandle);
            IAsyncResult result = dlg.BeginInvoke(handle, null, null);
            return dlg.EndInvoke(result);
        }

        protected virtual Bitmap CaptureHandle(IntPtr handle)
        {
            Bitmap memoryImage = null;
            this.images = new Bitmap[1];
            try
            {
                using (Graphics graphics = Graphics.FromHwnd(handle))
                {
                    Rectangle rc = Native.Methods.GetWindowRect(handle);

                    if ((int)graphics.VisibleClipBounds.Width > 0 && (int)graphics.VisibleClipBounds.Height > 0)
                    {
                        memoryImage = new Bitmap(rc.Width, rc.Height, graphics);

                        using (Graphics memoryGrahics = Graphics.FromImage(memoryImage))
                        {
                            memoryGrahics.CopyFromScreen(rc.X, rc.Y, 0, 0, rc.Size, CopyPixelOperation.SourceCopy);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Terminals.Logging.Log.Error("Capture Handle Failed", ex);
                MessageBox.Show(ex.ToString(), "Capture failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            this.images[0] = memoryImage;
            return memoryImage; 
        }

        public virtual Bitmap[] Capture(CaptureType typeOfCapture, string filename, ImageFormatHandler.ImageFormatTypes format)
        {
            this.Capture(typeOfCapture);
            this.Save(filename, format);
            return this.images;
        }

        public virtual Bitmap[] Capture(CaptureType typeOfCapture)
        {
            Bitmap memoryImage;
            int count = 1;

            try
            {
                Screen[] screens = Screen.AllScreens;
                Rectangle rc;
                switch (typeOfCapture)
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

                this.images = new Bitmap[count];

                for (int index = 0; index < count; index++)
                {
                    if (index > 0)
                        rc = screens[index].WorkingArea;

                    memoryImage = new Bitmap(rc.Width, rc.Height, PixelFormat.Format32bppArgb);

                    using (Graphics memoryGrahics = Graphics.FromImage(memoryImage))
                    {
                        memoryGrahics.CopyFromScreen(rc.X, rc.Y, 0, 0, rc.Size, CopyPixelOperation.SourceCopy);
                    }

                    this.images[index] = memoryImage;
                }
            }
            catch (Exception ex)
            {
                Terminals.Logging.Log.Error("Capture Failed", ex);
                MessageBox.Show(ex.ToString(), "Capture failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return this.images;
        }

        public virtual void Print()
        {
            if (this.images != null)
            {
                try
                {
                    for (int i = 0; i < this.images.Length; i++)
                    {
                        this.image = this.images[i];
                        this.doc.DefaultPageSettings.Landscape = (this.image.Width > this.image.Height);
                        this.doc.Print();
                    }
                }
                catch (Exception ex)
                {
                    Terminals.Logging.Log.Error("Print Failed", ex);
                    MessageBox.Show(ex.ToString(), "Capture failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void printPage(object sender, PrintPageEventArgs e)
        {
            RectangleF rc = this.doc.DefaultPageSettings.Bounds;
            float ratio = (float)this.image.Height / (float)(this.image.Width != 0 ? this.image.Width : 1);

            rc.Height = rc.Height - this.doc.DefaultPageSettings.Margins.Top - this.doc.DefaultPageSettings.Margins.Bottom;
            rc.Y = rc.Y + this.doc.DefaultPageSettings.Margins.Top;

            rc.Width = rc.Width - this.doc.DefaultPageSettings.Margins.Left - this.doc.DefaultPageSettings.Margins.Right;
            rc.X = rc.X + this.doc.DefaultPageSettings.Margins.Left;

            if (rc.Height / rc.Width > ratio)
                rc.Height = rc.Width * ratio;
            else
                rc.Width = rc.Height / (ratio != 0 ? ratio : 1);

            e.Graphics.DrawImage(this.image, rc);
        }

        public virtual void Save(string filename, ImageFormatHandler.ImageFormatTypes format)
        {
            string directory = Path.GetDirectoryName(filename);
            string name = Path.GetFileNameWithoutExtension(filename);
            string ext = Path.GetExtension(filename);

            ext = this.formatHandler.GetDefaultFilenameExtension(format);

            if (ext.Length == 0)
            {
                format = ImageFormatHandler.ImageFormatTypes.imgPNG;
                ext = "png";
            }

            try
            {
                ImageCodecInfo info;
                EncoderParameters parameters = this.formatHandler.GetEncoderParameters(format, out info);

                for (int i = 0; i < this.images.Length; i++)
                {
                    if (this.images.Length > 1)
                    {
                        filename = string.Format("{0}\\{1}.{2:D2}.{3}", directory, name, i + 1, ext);
                    }
                    else
                    {
                        filename = string.Format("{0}\\{1}.{2}", directory, name, ext);
                    }

                    this.image = this.images[i];

                    if (parameters != null)
                    {
                        this.image.Save(filename, info, parameters);
                    }
                    else
                    {
                        this.image.Save(filename, ImageFormatHandler.GetImageFormat(format));
                    }
                }
            }
            catch (Exception ex)
            {
                string s = string.Format("Saving image to [{0}] in format [{1}].\n{2}", filename, format.ToString(), ex.ToString());
                Terminals.Logging.Log.Error(s, ex);
                MessageBox.Show(s, "Capture failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
