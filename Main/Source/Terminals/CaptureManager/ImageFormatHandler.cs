using System;
using System.Drawing.Imaging;

namespace Terminals.CaptureManager
{
    internal class ImageFormatHandler
    {
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
                Encoder encode;
                EncoderParameter param;
                switch (type)
                {
                    case ImageFormatTypes.imgGIF:
                        parameters = new EncoderParameters(2);
                        encode = Encoder.Version;
                        param = new EncoderParameter(encode, (long)EncoderValue.VersionGif89);
                        parameters.Param[0] = param;

                        encode = Encoder.ScanMethod;
                        param = new EncoderParameter(encode, (long)this.encodingScanMethod);
                        parameters.Param[1] = param;
                        break;

                    case ImageFormatTypes.imgJPEG:
                        parameters = new EncoderParameters(2);
                        encode = Encoder.RenderMethod;
                        param = new EncoderParameter(encode, (long)this.encodingRenderMethod);
                        parameters.Param[0] = param;

                        encode = Encoder.Quality;
                        param = new EncoderParameter(encode, this.encodingQuality);
                        parameters.Param[1] = param;
                        break;

                    case ImageFormatTypes.imgPNG:
                        parameters = new EncoderParameters(2);
                        encode = Encoder.RenderMethod;
                        param = new EncoderParameter(encode, (long)this.encodingRenderMethod);
                        parameters.Param[0] = param;

                        encode = Encoder.ScanMethod;
                        param = new EncoderParameter(encode, (long)this.encodingScanMethod);
                        parameters.Param[1] = param;
                        break;

                    case ImageFormatTypes.imgTIFF:
                        parameters = new EncoderParameters(2);
                        encode = Encoder.ColorDepth;
                        param = new EncoderParameter(encode, this.encodingColorDepth);
                        parameters.Param[0] = param;

                        encode = Encoder.Compression;
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
}
