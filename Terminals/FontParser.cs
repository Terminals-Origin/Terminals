using System;
using System.Collections.Generic;
using System.Text;

namespace Terminals {
    class FontParser {
        public static System.Drawing.Font ParseFontName(string FontName) {
            //		FontName	"[Font: Name=Microsoft Sans Serif, Size=8.25, Units=3, GdiCharSet=0, GdiVerticalFont=False]"	string

            string Name="Microsoft Sans Serif";
            float Size=8.25f;
            string fn = FontName.Replace("[Font: ","");
            fn = fn.Replace("]","").Trim();
            string[] parts = fn.Split(',');
            foreach (string p in parts) {
                string[] fontItems = p.Split('=');
                switch (fontItems[0].ToLower().Trim()) {
                    case "name":
                        Name = fontItems[1];
                        break;
                    case "size":
                        float.TryParse(fontItems[1], out Size);
                        break;
                }
            }
            System.Drawing.Font f = new System.Drawing.Font(Name, Size);
            
            return f;
        }
    }
}
