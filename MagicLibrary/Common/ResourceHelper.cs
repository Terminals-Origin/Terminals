// *****************************************************************************
// 
//  (c) Crownwood Consulting Limited 2002 
//  All rights reserved. The software and associated documentation 
//  supplied hereunder are the proprietary information of Crownwood Consulting 
//	Limited, Haxey, North Lincolnshire, England and are supplied subject to 
//	licence terms.
// 
//  Magic Version 1.7 	www.dotnetmagic.com
// *****************************************************************************

using System;
using System.IO;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace Crownwood.Magic.Common
{
    public class ResourceHelper
    {
        public static Cursor LoadCursor(Type assemblyType, string cursorName)
        {
            // Get the assembly that contains the bitmap resource
            Assembly myAssembly = Assembly.GetAssembly(assemblyType);

            // Get the resource stream containing the images
            Stream iconStream = myAssembly.GetManifestResourceStream(cursorName);

            // Load the Icon from the stream
            return new Cursor(iconStream);
        }
    
        public static Icon LoadIcon(Type assemblyType, string iconName)
        {
            // Get the assembly that contains the bitmap resource
            Assembly myAssembly = Assembly.GetAssembly(assemblyType);

            // Get the resource stream containing the images
            Stream iconStream = myAssembly.GetManifestResourceStream(iconName);

            // Load the Icon from the stream
            return new Icon(iconStream);
        }

        public static Icon LoadIcon(Type assemblyType, string iconName, Size iconSize)
        {
            // Load the entire Icon requested (may include several different Icon sizes)
            Icon rawIcon = LoadIcon(assemblyType, iconName);
			
            // Create and return a new Icon that only contains the requested size
            return new Icon(rawIcon, iconSize); 
        }

        public static Bitmap LoadBitmap(Type assemblyType, string imageName)
        {
            return LoadBitmap(assemblyType, imageName, false, new Point(0,0));
        }

        public static Bitmap LoadBitmap(Type assemblyType, string imageName, Point transparentPixel)
        {
            return LoadBitmap(assemblyType, imageName, true, transparentPixel);
        }

        public static ImageList LoadBitmapStrip(Type assemblyType, string imageName, Size imageSize)
        {
            return LoadBitmapStrip(assemblyType, imageName, imageSize, false, new Point(0,0));
        }

        public static ImageList LoadBitmapStrip(Type assemblyType, 
                                                string imageName, 
                                                Size imageSize,
                                                Point transparentPixel)
        {
            return LoadBitmapStrip(assemblyType, imageName, imageSize, true, transparentPixel);
        }

        protected static Bitmap LoadBitmap(Type assemblyType, 
                                           string imageName, 
                                           bool makeTransparent, 
                                           Point transparentPixel)
        {
            // Get the assembly that contains the bitmap resource
            Assembly myAssembly = Assembly.GetAssembly(assemblyType);

            // Get the resource stream containing the images
            Stream imageStream = myAssembly.GetManifestResourceStream(imageName);

            // Load the bitmap from stream
            Bitmap image = new Bitmap(imageStream);

            if (makeTransparent)
            {
                Color backColor = image.GetPixel(transparentPixel.X, transparentPixel.Y);
    
                // Make backColor transparent for Bitmap
                image.MakeTransparent(backColor);
            }
			    
            return image;
        }

        protected static ImageList LoadBitmapStrip(Type assemblyType, 
                                                   string imageName, 
                                                   Size imageSize,
                                                   bool makeTransparent,
                                                   Point transparentPixel)
        {
            // Create storage for bitmap strip
            ImageList images = new ImageList();

            // Define the size of images we supply
            images.ImageSize = imageSize;

            // Get the assembly that contains the bitmap resource
            Assembly myAssembly = Assembly.GetAssembly(assemblyType);

            // Get the resource stream containing the images
            Stream imageStream = myAssembly.GetManifestResourceStream(imageName);

            // Load the bitmap strip from resource
            Bitmap pics = new Bitmap(imageStream);

            if (makeTransparent)
            {
                Color backColor = pics.GetPixel(transparentPixel.X, transparentPixel.Y);
    
                // Make backColor transparent for Bitmap
                pics.MakeTransparent(backColor);
            }
			    
            // Load them all !
            images.Images.AddStrip(pics);

            return images;
        }
    }
}