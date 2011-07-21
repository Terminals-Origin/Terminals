using System.Globalization;
using System.Drawing;

namespace Terminals
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
    internal const string DEFAULT_FONT = "[Font: Name=Courier New, Size=10, Units=3, GdiCharSet=0, GdiVerticalFont=False, Style=0]";

    private const string FONT_FORMAT = "[Font: Name={0}, Size={1}, Units={2}, GdiCharSet={3}, GdiVerticalFont={4}, Style={5}]";

    internal static string ToString(Font source)
    {
      string size = source.Size.ToString(CultureInfo.InvariantCulture); // to force always use dot, culture independent
      return string.Format(FONT_FORMAT, source.Name, size,
        (byte)source.Unit, source.GdiCharSet, source.GdiVerticalFont, (byte)source.Style);
                }

    internal static Font FromString(string source)
    {
      int sizeIndex = source.IndexOf("Size=", 0);
      string name = ParseName(source, sizeIndex);

      int unitsIndex = source.IndexOf("Units=", 0);
      float size = ParseSize(source, sizeIndex, unitsIndex);

      int charSetIndex = source.IndexOf("GdiCharSet=", 0);
      GraphicsUnit units = ParseUnits(source, unitsIndex, charSetIndex);

      int verticalIndex = source.IndexOf("GdiVerticalFont=", 0);
      byte gdi = ParseGdi(source, charSetIndex, verticalIndex);

      int styleIndex = source.IndexOf("Style=", 0);
      bool vertical = ParseVertical(source, verticalIndex, styleIndex);

      FontStyle fontStyle = ParseFontStyle(source, styleIndex);

      return new Font(name, size, fontStyle, units, gdi, vertical);
    }

    private static string ParseName(string fontText, int sizeIndex)
    {
      int nameIndex = 12; // see the default formating
      return fontText.Substring(nameIndex, sizeIndex - nameIndex - 2); // 2 means space and comma
    }

    private static float ParseSize(string fontText, int sizeIndex, int unitsIndex)
    {
      string sizeText = fontText.Substring(sizeIndex + 5, unitsIndex - sizeIndex - 7);
      if (!sizeText.Contains(",")) // recover from previous trubles
      {
        return System.Convert.ToSingle(sizeText, CultureInfo.InvariantCulture);
      }
            
      return 8.25f;
    }

    private static GraphicsUnit ParseUnits(string fontText, int unitsIndex, int charSetIndex)
    {
      string unitText = fontText.Substring(unitsIndex + 6, charSetIndex - unitsIndex - 8);
      return (GraphicsUnit)System.Convert.ToByte(unitText);
    }

    private static byte ParseGdi(string fontText, int charSetIndex, int verticalIndex)
    {
      string gdiCharText = fontText.Substring(charSetIndex + 11, verticalIndex - charSetIndex - 13);
      return System.Convert.ToByte(gdiCharText);
    }

    private static bool ParseVertical(string fontText, int verticalIndex, int styleIndex)
    {
      string verticalText = "0";
      if (styleIndex < 0) // style wasnt provided in previous versions
      {
        verticalText  = fontText.Substring(verticalIndex + 16, fontText.Length - 17 - verticalIndex);
      }
      else
      {
        verticalText = fontText.Substring(verticalIndex + 16, styleIndex - verticalIndex - 18);
      }
      return System.Convert.ToBoolean(verticalText);
    }

    private static FontStyle ParseFontStyle(string fontText, int styleIndex)
    {
      if (styleIndex > 0)
      {
        string styleText = fontText.Substring(styleIndex + 6, fontText.Length - 7 - styleIndex);
        return (FontStyle)System.Convert.ToByte(styleText);
      }
      return FontStyle.Regular;
    }
    }
}
