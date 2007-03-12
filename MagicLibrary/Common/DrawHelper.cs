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
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Crownwood.Magic.Win32;

namespace Crownwood.Magic.Common
{
    public class DrawHelper
    {
        public enum CommandState
        {
            Normal,
            HotTrack,
            Pushed
        }
    
        protected static IntPtr _halfToneBrush = IntPtr.Zero;

        public static void DrawReverseString(Graphics g, 
                                             String drawText, 
                                             Font drawFont, 
                                             Rectangle drawRect,
                                             Brush drawBrush,
                                             StringFormat drawFormat)
        {
            GraphicsContainer container = g.BeginContainer();

            // The text will be rotated around the origin (0,0) and so needs moving
            // back into position by using a transform
            g.TranslateTransform(drawRect.Left * 2 + drawRect.Width, 
                                 drawRect.Top * 2 + drawRect.Height);

            // Rotate the text by 180 degress to reverse the direction 
            g.RotateTransform(180);

            // Draw the string as normal and let then transforms do the work
            g.DrawString(drawText, drawFont, drawBrush, drawRect, drawFormat);

            g.EndContainer(container);
        }

        public static void DrawPlainRaised(Graphics g,
                                           Rectangle boxRect,
                                           Color baseColor)
        {
            using(Pen lighlight = new Pen(ControlPaint.LightLight(baseColor)),
                      dark = new Pen(ControlPaint.DarkDark(baseColor)))
            {                                            
                g.DrawLine(lighlight, boxRect.Left, boxRect.Bottom, boxRect.Left, boxRect.Top);
                g.DrawLine(lighlight, boxRect.Left, boxRect.Top, boxRect.Right, boxRect.Top);
                g.DrawLine(dark, boxRect.Right, boxRect.Top, boxRect.Right, boxRect.Bottom);
                g.DrawLine(dark, boxRect.Right, boxRect.Bottom, boxRect.Left, boxRect.Bottom);
            }
        }

        public static void DrawPlainSunken(Graphics g,
                                           Rectangle boxRect,
                                           Color baseColor)
        {
            using(Pen lighlight = new Pen(ControlPaint.LightLight(baseColor)),
                      dark = new Pen(ControlPaint.DarkDark(baseColor)))
            {                                            
                g.DrawLine(dark, boxRect.Left, boxRect.Bottom, boxRect.Left, boxRect.Top);
                g.DrawLine(dark, boxRect.Left, boxRect.Top, boxRect.Right, boxRect.Top);
                g.DrawLine(lighlight, boxRect.Right, boxRect.Top, boxRect.Right, boxRect.Bottom);
                g.DrawLine(lighlight, boxRect.Right, boxRect.Bottom, boxRect.Left, boxRect.Bottom);
            }
        }

        public static void DrawPlainRaisedBorder(Graphics g, 
                                                 Rectangle rect,
                                                 Color lightLight, 
                                                 Color baseColor,
                                                 Color dark, 
                                                 Color darkDark)
        {
            if ((rect.Width > 2) && (rect.Height > 2))
            {
                using(Pen ll = new Pen(lightLight),
                          b = new Pen(baseColor),
                          d = new Pen(dark),
                          dd = new Pen(darkDark))
                {
                    int left = rect.Left;
                    int top = rect.Top;
                    int right = rect.Right;
                    int bottom = rect.Bottom;

                    // Draw the top border
                    g.DrawLine(b, right-1, top, left, top);
                    g.DrawLine(ll, right-2, top+1, left+1, top+1);
                    g.DrawLine(b, right-3, top+2, left+2, top+2);

                    // Draw the left border
                    g.DrawLine(b, left, top, left, bottom-1);
                    g.DrawLine(ll, left+1, top+1, left+1, bottom-2);
                    g.DrawLine(b, left+2, top+2, left+2, bottom-3);
					
                    // Draw the right
                    g.DrawLine(dd, right-1, top+1, right-1, bottom-1);
                    g.DrawLine(d, right-2, top+2, right-2, bottom-2);
                    g.DrawLine(b, right-3, top+3, right-3, bottom-3);

                    // Draw the bottom
                    g.DrawLine(dd, right-1, bottom-1, left, bottom-1);
                    g.DrawLine(d, right-2, bottom-2, left+1, bottom-2);
                    g.DrawLine(b, right-3, bottom-3, left+2, bottom-3);
                }
            }
        }

        public static void DrawPlainRaisedBorderTopOrBottom(Graphics g, 
                                                            Rectangle rect,
                                                            Color lightLight, 
                                                            Color baseColor,
                                                            Color dark, 
                                                            Color darkDark,
                                                            bool drawTop)
        {
            if ((rect.Width > 2) && (rect.Height > 2))
            {
                using(Pen ll = new Pen(lightLight),
                          b = new Pen(baseColor),
                          d = new Pen(dark),
                          dd = new Pen(darkDark))
                {
                    int left = rect.Left;
                    int top = rect.Top;
                    int right = rect.Right;
                    int bottom = rect.Bottom;

                    if (drawTop)
                    {
                        // Draw the top border
                        g.DrawLine(b, right-1, top, left, top);
                        g.DrawLine(ll, right-1, top+1, left, top+1);
                        g.DrawLine(b, right-1, top+2, left, top+2);
                    }
                    else
                    {
                        // Draw the bottom
                        g.DrawLine(dd, right-1, bottom-1, left, bottom-1);
                        g.DrawLine(d, right-1, bottom-2, left, bottom-2);
                        g.DrawLine(b, right-1, bottom-3, left, bottom-3);
                    }
                }
            }
        }

        public static void DrawPlainSunkenBorder(Graphics g, 
                                                 Rectangle rect,
                                                 Color lightLight, 
                                                 Color baseColor,
                                                 Color dark, 
                                                 Color darkDark)
        {
            if ((rect.Width > 2) && (rect.Height > 2))
            {
                using(Pen ll = new Pen(lightLight),
                          b = new Pen(baseColor),
                          d = new Pen(dark),
                          dd = new Pen(darkDark))
                {
                    int left = rect.Left;
                    int top = rect.Top;
                    int right = rect.Right;
                    int bottom = rect.Bottom;

                    // Draw the top border
                    g.DrawLine(d, right-1, top, left, top);
                    g.DrawLine(dd, right-2, top+1, left+1, top+1);
                    g.DrawLine(b, right-3, top+2, left+2, top+2);

                    // Draw the left border
                    g.DrawLine(d, left, top, left, bottom-1);
                    g.DrawLine(dd, left+1, top+1, left+1, bottom-2);
                    g.DrawLine(b, left+2, top+2, left+2, bottom-3);
					
                    // Draw the right
                    g.DrawLine(ll, right-1, top+1, right-1, bottom-1);
                    g.DrawLine(b, right-2, top+2, right-2, bottom-2);
                    g.DrawLine(b, right-3, top+3, right-3, bottom-3);

                    // Draw the bottom
                    g.DrawLine(ll, right-1, bottom-1, left, bottom-1);
                    g.DrawLine(b, right-2, bottom-2, left+1, bottom-2);
                    g.DrawLine(b, right-3, bottom-3, left+2, bottom-3);
                }
            }
        }

        public static void DrawPlainSunkenBorderTopOrBottom(Graphics g, 
                                                            Rectangle rect,
                                                            Color lightLight, 
                                                            Color baseColor,
                                                            Color dark, 
                                                            Color darkDark,
                                                            bool drawTop)
        {
            if ((rect.Width > 2) && (rect.Height > 2))
            {
                using(Pen ll = new Pen(lightLight),
                          b = new Pen(baseColor),
                          d = new Pen(dark),
                          dd = new Pen(darkDark))
                {
                    int left = rect.Left;
                    int top = rect.Top;
                    int right = rect.Right;
                    int bottom = rect.Bottom;

                    if (drawTop)
                    {
                        // Draw the top border
                        g.DrawLine(d, right-1, top, left, top);
                        g.DrawLine(dd, right-1, top+1, left, top+1);
                        g.DrawLine(b, right-1, top+2, left, top+2);
                    }
                    else
                    {
                        // Draw the bottom
                        g.DrawLine(ll, right-1, bottom-1, left, bottom-1);
                        g.DrawLine(b, right-1, bottom-2, left, bottom-2);
                        g.DrawLine(b, right-1, bottom-3, left, bottom-3);
                    }
                }
            }
        }
        
        public static void DrawButtonCommand(Graphics g, 
                                             VisualStyle style, 
                                             Direction direction, 
                                             Rectangle drawRect,
                                             CommandState state,
                                             Color baseColor,
                                             Color trackLight,
                                             Color trackBorder)
        {
            Rectangle rect = new Rectangle(drawRect.Left, drawRect.Top, drawRect.Width - 1, drawRect.Height - 1);
        
            // Draw background according to style
            switch(style)
            {
                case VisualStyle.Plain:
                    // Draw background with back color
                    using(SolidBrush backBrush = new SolidBrush(baseColor))
                        g.FillRectangle(backBrush, rect);

                    // Modify according to state
                    switch(state)
                    {
                        case CommandState.HotTrack:
                            DrawPlainRaised(g, rect, baseColor);
                            break;
                        case CommandState.Pushed:
                            DrawPlainSunken(g, rect, baseColor);
                            break;
                    }
                    break;
                case VisualStyle.IDE:
                    // Draw according to state
                    switch(state)
                    {
                        case CommandState.Normal:
                            // Draw background with back color
                            using(SolidBrush backBrush = new SolidBrush(baseColor))
                                g.FillRectangle(backBrush, rect);
                            break;
                        case CommandState.HotTrack:
                            g.FillRectangle(Brushes.White, rect);

                            using(SolidBrush trackBrush = new SolidBrush(trackLight))
                                g.FillRectangle(trackBrush, rect);
                            
                            using(Pen trackPen = new Pen(trackBorder))
                                g.DrawRectangle(trackPen, rect);
                            break;
                        case CommandState.Pushed:
                            //TODO: draw in a darker background color
                            break;
                    }
                    break;
            }
        }
        
        public static void DrawSeparatorCommand(Graphics g, 
                                                VisualStyle style, 
                                                Direction direction, 
                                                Rectangle drawRect,
                                                Color baseColor)
        {
            // Drawing depends on the visual style required
            if (style == VisualStyle.IDE)
            {
                // Draw a single separating line
                using(Pen dPen = new Pen(ControlPaint.Dark(baseColor)))
                {            
                    if (direction == Direction.Horizontal)
                        g.DrawLine(dPen, drawRect.Left, drawRect.Top,
                                         drawRect.Left, drawRect.Bottom - 1);
                    else
                        g.DrawLine(dPen, drawRect.Left, drawRect.Top,
                                         drawRect.Right - 1, drawRect.Top);                    
                }
            }
            else
            {
                // Draw a dark/light combination of lines to give an indent
                using(Pen lPen = new Pen(ControlPaint.Dark(baseColor)),
                          llPen = new Pen(ControlPaint.LightLight(baseColor)))
                {							
                    if (direction == Direction.Horizontal)
                    {
                        g.DrawLine(lPen, drawRect.Left, drawRect.Top, drawRect.Left, drawRect.Bottom - 1);
                        g.DrawLine(llPen, drawRect.Left + 1, drawRect.Top, drawRect.Left + 1, drawRect.Bottom - 1);
                    }
                    else
                    {
                        g.DrawLine(lPen, drawRect.Left, drawRect.Top, drawRect.Right - 1, drawRect.Top);                    
                        g.DrawLine(llPen, drawRect.Left, drawRect.Top + 1, drawRect.Right - 1, drawRect.Top + 1);                    
                    }      
                }
            }
        }

        public static void DrawDragRectangle(Rectangle newRect, int indent)
        {
            DrawDragRectangles(new Rectangle[]{newRect}, indent);
        }

        public static void DrawDragRectangles(Rectangle[] newRects, int indent)
        {
            if (newRects.Length > 0)
            {
                // Create the first region
                IntPtr newRegion = CreateRectangleRegion(newRects[0], indent);

                for(int index=1; index<newRects.Length; index++)
                {
                    // Create the extra region
                    IntPtr extraRegion = CreateRectangleRegion(newRects[index], indent);

                    // Remove the intersection of the existing and extra regions
                    Gdi32.CombineRgn(newRegion, newRegion, extraRegion, (int)Win32.CombineFlags.RGN_XOR);

                    // Remove unwanted intermediate objects
                    Gdi32.DeleteObject(extraRegion);
                }

                // Get hold of the DC for the desktop
                IntPtr hDC = User32.GetDC(IntPtr.Zero);

                // Define the area we are allowed to draw into
                Gdi32.SelectClipRgn(hDC, newRegion);

                Win32.RECT rectBox = new Win32.RECT();
				 
                // Get the smallest rectangle that encloses region
                Gdi32.GetClipBox(hDC, ref rectBox);

                IntPtr brushHandler = GetHalfToneBrush();

                // Select brush into the device context
                IntPtr oldHandle = Gdi32.SelectObject(hDC, brushHandler);

                // Blit to screen using provided pattern brush and invert with existing screen contents
                Gdi32.PatBlt(hDC, 
                             rectBox.left, 
                             rectBox.top, 
                             rectBox.right - rectBox.left, 
                             rectBox.bottom - rectBox.top, 
                             (uint)RasterOperations.PATINVERT);

                // Put old handle back again
                Gdi32.SelectObject(hDC, oldHandle);

                // Reset the clipping region
                Gdi32.SelectClipRgn(hDC, IntPtr.Zero);

                // Remove unwanted region object
                Gdi32.DeleteObject(newRegion);

                // Must remember to release the HDC resource!
                User32.ReleaseDC(IntPtr.Zero, hDC);
            }
        }

        protected static IntPtr CreateRectangleRegion(Rectangle rect, int indent)
        {
            Win32.RECT newWinRect = new Win32.RECT();
            newWinRect.left = rect.Left;
            newWinRect.top = rect.Top;
            newWinRect.right = rect.Right;
            newWinRect.bottom = rect.Bottom;

            // Create region for whole of the new rectangle
            IntPtr newOuter = Gdi32.CreateRectRgnIndirect(ref newWinRect);

            // If the rectangle is to small to make an inner object from, then just use the outer
            if ((indent <= 0) || (rect.Width <= indent) || (rect.Height <= indent))
                return newOuter;

            newWinRect.left += indent;
            newWinRect.top += indent;
            newWinRect.right -= indent;
            newWinRect.bottom -= indent;

            // Create region for the unwanted inside of the new rectangle
            IntPtr newInner = Gdi32.CreateRectRgnIndirect(ref newWinRect);

            Win32.RECT emptyWinRect = new Win32.RECT();
            emptyWinRect.left = 0;
            emptyWinRect.top = 0;
            emptyWinRect.right = 0;
            emptyWinRect.bottom = 0;

            // Create a destination region
            IntPtr newRegion = Gdi32.CreateRectRgnIndirect(ref emptyWinRect);

            // Remove the intersection of the outer and inner
            Gdi32.CombineRgn(newRegion, newOuter, newInner, (int)Win32.CombineFlags.RGN_XOR);

            // Remove unwanted intermediate objects
            Gdi32.DeleteObject(newOuter);
            Gdi32.DeleteObject(newInner);

            // Return the resultant region object
            return newRegion;
        }

        protected static IntPtr GetHalfToneBrush()
        {
            if (_halfToneBrush == IntPtr.Zero)
            {	
                Bitmap bitmap = new Bitmap(8,8,PixelFormat.Format32bppArgb);

                Color white = Color.FromArgb(255,255,255,255);
                Color black = Color.FromArgb(255,0,0,0);

                bool flag=true;

                // Alternate black and white pixels across all lines
                for(int x=0; x<8; x++, flag = !flag)
                    for(int y=0; y<8; y++, flag = !flag)
                        bitmap.SetPixel(x, y, (flag ? white : black));

                IntPtr hBitmap = bitmap.GetHbitmap();

                Win32.LOGBRUSH brush = new Win32.LOGBRUSH();

                brush.lbStyle = (uint)Win32.BrushStyles.BS_PATTERN;
                brush.lbHatch = (uint)hBitmap;

                _halfToneBrush = Gdi32.CreateBrushIndirect(ref brush);
            }

            return _halfToneBrush;
        }
    }
}


