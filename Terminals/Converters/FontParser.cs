using System;
using System.Globalization;
using System.Drawing;

namespace Terminals.Converters
{
    /// <summary>
    /// Converts font to string and viceversa in example form:
    /// "[Font: Name=Microsoft Sans Serif, Size=8.25, Units=3, GdiCharSet=0, GdiVerticalFont=False]"
    /// </summary>
    internal class FontParser
    {
        /// <summary>
        /// Gets the font used for new values. The values is regular "Courier New" with Size 10.
        /// </summary>
        internal const String DEFAULT_FONT = "[Font: Name=Courier New, Size=10, Units=3, GdiCharSet=0, GdiVerticalFont=False, Style=0]";

        private const String FONT_FORMAT = "[Font: Name={0}, Size={1}, Units={2}, GdiCharSet={3}, GdiVerticalFont={4}, Style={5}]";

        internal static String ToString(Font source)
        {
            String size = source.Size.ToString(CultureInfo.InvariantCulture); // to force always use dot, culture independent
            return String.Format(FONT_FORMAT, source.Name, size,
            (Byte)source.Unit, source.GdiCharSet, source.GdiVerticalFont, (Byte)source.Style);
        }

        internal static Font FromString(String source)
        {
            Int32 sizeIndex = source.IndexOf("Size=", 0);
            String name = ParseName(source, sizeIndex);

            Int32 unitsIndex = source.IndexOf("Units=", 0);
            Single size = ParseSize(source, sizeIndex, unitsIndex);

            Int32 charSetIndex = source.IndexOf("GdiCharSet=", 0);
            GraphicsUnit units = ParseUnits(source, unitsIndex, charSetIndex);

            Int32 verticalIndex = source.IndexOf("GdiVerticalFont=", 0);
            Byte gdi = ParseGdi(source, charSetIndex, verticalIndex);

            Int32 styleIndex = source.IndexOf("Style=", 0);
            Boolean vertical = ParseVertical(source, verticalIndex, styleIndex);

            FontStyle fontStyle = ParseFontStyle(source, styleIndex);

            return new Font(name, size, fontStyle, units, gdi, vertical);
        }

        private static String ParseName(String fontText, Int32 sizeIndex)
        {
            Int32 nameIndex = 12; // see the default formating
            return fontText.Substring(nameIndex, sizeIndex - nameIndex - 2); // 2 means space and comma
        }

        private static float ParseSize(String fontText, Int32 sizeIndex, Int32 unitsIndex)
        {
            String sizeText = fontText.Substring(sizeIndex + 5, unitsIndex - sizeIndex - 7);
            if (!sizeText.Contains(",")) // recover from previous trubles
            {
                return Convert.ToSingle(sizeText, CultureInfo.InvariantCulture);
            }
            
            return 8.25f;
        }

        private static GraphicsUnit ParseUnits(String fontText, Int32 unitsIndex, Int32 charSetIndex)
        {
            String unitText = fontText.Substring(unitsIndex + 6, charSetIndex - unitsIndex - 8);
            return (GraphicsUnit)Convert.ToByte(unitText);
        }

        private static Byte ParseGdi(String fontText, Int32 charSetIndex, Int32 verticalIndex)
        {
            String gdiCharText = fontText.Substring(charSetIndex + 11, verticalIndex - charSetIndex - 13);
            return Convert.ToByte(gdiCharText);
        }

        private static Boolean ParseVertical(String fontText, Int32 verticalIndex, Int32 styleIndex)
        {
            String verticalText = "0";
            if (styleIndex < 0) // style wasnt provided in previous versions
            {
                verticalText  = fontText.Substring(verticalIndex + 16, fontText.Length - 17 - verticalIndex);
            }
            else
            {
                verticalText = fontText.Substring(verticalIndex + 16, styleIndex - verticalIndex - 18);
            }

            return Convert.ToBoolean(verticalText);
        }

        private static FontStyle ParseFontStyle(String fontText, Int32 styleIndex)
        {
            if (styleIndex > 0)
            {
                String styleText = fontText.Substring(styleIndex + 6, fontText.Length - 7 - styleIndex);
                return (FontStyle)Convert.ToByte(styleText);
            }
    
            return FontStyle.Regular;
        }
    }
}
