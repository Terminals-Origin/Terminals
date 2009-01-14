using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Unified
{
    public class StreamHelper
    {
        public static string StreamToString(System.IO.Stream Stream)
        {
            if (Stream == null) return null;
            return System.Text.Encoding.Default.GetString(StreamToBytes(Stream));
        }

        public static byte[] StreamToBytes(System.IO.Stream Stream)
        {
            if (Stream == null) return null;

            byte[] buffer = null;
            if (Stream.Position > 0 && Stream.CanSeek) Stream.Seek(0, System.IO.SeekOrigin.Begin);
            buffer = new byte[Stream.Length];
            Stream.Read(buffer, 0, (int)buffer.Length);
            return buffer;
        }

        public static System.Drawing.Icon ImageToIcon(System.Drawing.Image image)
        {
            return System.Drawing.Icon.FromHandle(((Bitmap)image).GetHicon());
        }

        public static System.Drawing.Image IconToImage(Icon icon)
        {
            return System.Drawing.Image.FromHbitmap(icon.ToBitmap().GetHbitmap());
        }
    }
}