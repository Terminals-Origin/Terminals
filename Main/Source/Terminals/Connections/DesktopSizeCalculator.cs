using System;
using System.Drawing;
using System.Windows.Forms;
using Terminals.Data;

namespace Terminals.Connections
{
    internal static class DesktopSizeCalculator
    {
        private const int MAX_WIDTH = 4096;
        private const int MAX_HEIGHT = 2048;

        public static Size GetSize(Connection connection, IFavorite favorite)
        {
            switch (favorite.Display.DesktopSize)
            {
                case DesktopSize.x640:
                    return new Size(640, 480);
                case DesktopSize.x800:
                    return new Size(800, 600);
                case DesktopSize.x1024:
                    return new Size(1024, 768);
                case DesktopSize.x1152:
                    return new Size(1152, 864);
                case DesktopSize.x1280:
                    return new Size(1280, 1024);
                case DesktopSize.FullScreen:
                    return CalculateFullScreenSize(connection);
                case DesktopSize.FitToWindow:
                case DesktopSize.AutoScale:
                    return CalculateAutoScaleSize(connection);
                default:
                    return CalculateDefaultSize(favorite);
            }
        }

        private static Size CalculateDefaultSize(IFavorite favorite)
        {
            IDisplayOptions display = favorite.Display;
            return new Size(display.Width, display.Height);
        }

        private static Size CalculateAutoScaleSize(Connection connection)
        {
            Control parent = connection.Parent;
            return GetMaxAvailableSize(parent.Width, parent.Height);
        }

        private static Size CalculateFullScreenSize(Connection connection)
        {
            Rectangle controlBounds = Screen.FromControl(connection).Bounds;
            var width = controlBounds.Width - 13;
            var height = controlBounds.Height - 1;
            return GetMaxAvailableSize(width, height);
        }

        private static Size GetMaxAvailableSize(int width, int height)
        {
            width = Math.Min(MAX_WIDTH, width);
            height = Math.Min(MAX_HEIGHT, height);
            return new Size(width, height);
        }
    }
}