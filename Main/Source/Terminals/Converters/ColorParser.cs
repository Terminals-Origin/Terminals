using System;
using System.Drawing;

namespace Terminals.Converters
{
    /// <summary>
    /// Encapsulates Color text representation conversions
    /// </summary>
    internal static class ColorParser
    {
        internal static Color FromString(string color)
        {
            Color knownColor = Color.FromName(color);
            if (knownColor.IsKnownColor)
            {
                return knownColor;
            }

            return GetColorFromUnKnownName(color);
        }

        private static Color GetColorFromUnKnownName(string color)
        {
            try
            {
                int a = GetColorPart(color, 0);
                int r = GetColorPart(color, 1);
                int g = GetColorPart(color, 2);
                int b = GetColorPart(color, 3);
                return Color.FromArgb(a, r, g, b);
            }
            catch (Exception)
            {
                return Color.Black;
            }
        }

        /// <summary>
        /// Extracts the chanel number representation from color string.
        /// </summary>
        /// <param name="color"></param>
        /// <param name="partIndex"></param>
        /// <returns></returns>
        private static int GetColorPart(string color, int partIndex)
        {
            int index = partIndex * 2;
            string part = color.Substring(index, 2);
            return Convert.ToInt32(part, 16);
        }

        internal static string ToString(Color color)
        {
            return color.Name;
        }
    }
}
