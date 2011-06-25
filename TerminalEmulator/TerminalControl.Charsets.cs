using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace WalburySoftware
{
    public partial class TerminalEmulator : Control
    {
        private class uc_Chars
        {
            public enum Sets
            {
                None,
                DECSG,
                DECTECH,
                DECS,
                ASCII,
                ISOLatin1S,
                NRCUK,
                NRCFinnish,
                NRCFrench,
                NRCFrenchCanadian,
                NRCGerman,
                NRCItalian,
                NRCNorDanish,
                NRCPortuguese,
                NRCSpanish,
                NRCSwedish,
                NRCSwiss
            }

            public struct uc_CharSet
            {
                public uc_CharSet(Int32 p1, Int16 p2)
                {
                    Location = p1;
                    UnicodeNo = p2;
                }

                public Int32 Location;
                public Int16 UnicodeNo;
            }

            public uc_Chars.Sets Set;

            public uc_Chars(uc_Chars.Sets p1)
            {
                this.Set = p1;
            }

            public static Char Get(Char CurChar, uc_Chars.Sets GL, uc_Chars.Sets GR)
            {
                uc_CharSet[] CurSet;

                // I'm assuming the left hand in use table will always have a 00-7F char set in it
                if (Convert.ToInt32(CurChar) < 128)
                {
                    switch (GL)
                    {
                        case uc_Chars.Sets.ASCII:
                            CurSet = uc_Chars.ASCII;
                            break;

                        case uc_Chars.Sets.DECSG:
                            CurSet = uc_Chars.DECSG;
                            break;

                        case uc_Chars.Sets.NRCUK:
                            CurSet = uc_Chars.NRCUK;
                            break;

                        case uc_Chars.Sets.NRCFinnish:
                            CurSet = uc_Chars.NRCFinnish;
                            break;

                        case uc_Chars.Sets.NRCFrench:
                            CurSet = uc_Chars.NRCFrench;
                            break;

                        case uc_Chars.Sets.NRCFrenchCanadian:
                            CurSet = uc_Chars.NRCFrenchCanadian;
                            break;

                        case uc_Chars.Sets.NRCGerman:
                            CurSet = uc_Chars.NRCGerman;
                            break;

                        case uc_Chars.Sets.NRCItalian:
                            CurSet = uc_Chars.NRCItalian;
                            break;

                        case uc_Chars.Sets.NRCNorDanish:
                            CurSet = uc_Chars.NRCNorDanish;
                            break;

                        case uc_Chars.Sets.NRCPortuguese:
                            CurSet = uc_Chars.NRCPortuguese;
                            break;

                        case uc_Chars.Sets.NRCSpanish:
                            CurSet = uc_Chars.NRCSpanish;
                            break;

                        case uc_Chars.Sets.NRCSwedish:
                            CurSet = uc_Chars.NRCSwedish;
                            break;

                        case uc_Chars.Sets.NRCSwiss:
                            CurSet = uc_Chars.NRCSwiss;
                            break;

                        default:
                            CurSet = uc_Chars.ASCII;
                            break;
                    }
                }
                // I'm assuming the right hand in use table will always have a 80-FF char set in it
                else
                {
                    switch (GR)
                    {
                        case uc_Chars.Sets.ISOLatin1S:
                            CurSet = uc_Chars.ISOLatin1S;
                            break;

                        case uc_Chars.Sets.DECS:
                            CurSet = uc_Chars.DECS;
                            break;

                        default:
                            CurSet = uc_Chars.DECS;
                            break;
                    }
                }

                for (Int32 i = 0; i < CurSet.Length; i++)
                {
                    if (CurSet[i].Location == Convert.ToInt32(CurChar))
                    {
                        Byte[] CurBytes = BitConverter.GetBytes(CurSet[i].UnicodeNo);
                        Char[] NewChars = Encoding.Unicode.GetChars(CurBytes);

                        return NewChars[0];
                    }

                }

                return CurChar;
            }

            public static uc_CharSet[] DECSG =
            {
                new uc_CharSet (0x5F, 0x0020), // Blank (I've used space here so you may want to change this)
                //            new uc_CharSet (0x60, 0x25C6), // Filled Diamond 
                new uc_CharSet (0x61, 0x0000), // Pi over upsidedown Pi ?  
                new uc_CharSet (0x62, 0x2409), // HT symbol 
                new uc_CharSet (0x63, 0x240C), // LF Symbol  
                new uc_CharSet (0x64, 0x240D), // CR Symbol  
                new uc_CharSet (0x65, 0x240A), // LF Symbol  
                new uc_CharSet (0x66, 0x00B0), // Degree  
                new uc_CharSet (0x67, 0x00B1), // Plus over Minus  
                new uc_CharSet (0x68, 0x2424), // NL Symbol  
                new uc_CharSet (0x69, 0x240B), // VT Symbol 
                //            new uc_CharSet (0x6A, 0x2518), // Bottom Right Box 
                //            new uc_CharSet (0x6B, 0x2510), // Top Right Box
                //            new uc_CharSet (0x6C, 0x250C), // TopLeft Box
                //            new uc_CharSet (0x6D, 0x2514), // Bottom Left Box
                //            new uc_CharSet (0x6E, 0x253C), // Cross Piece
                new uc_CharSet (0x6F, 0x23BA), // Scan Line 1
                new uc_CharSet (0x70, 0x25BB), // Scan Line 3
                //            new uc_CharSet (0x71, 0x2500), // Horizontal Line (scan line 5 as well?)
                new uc_CharSet (0x72, 0x23BC), // Scan Line 7 
                new uc_CharSet (0x73, 0x23BD), // Scan Line 9 
                //            new uc_CharSet (0x74, 0x251C), // Left Tee Piece
                //            new uc_CharSet (0x75, 0x2524), // Right Tee Piece
                //            new uc_CharSet (0x76, 0x2534), // Bottom Tee Piece
                //            new uc_CharSet (0x77, 0x252C), // Top Tee Piece
                //            new uc_CharSet (0x78, 0x2502), // Vertical Line
                new uc_CharSet (0x79, 0x2264), // Less than or equal  
                new uc_CharSet (0x7A, 0x2265), // Greater than or equal 
                new uc_CharSet (0x7B, 0x03A0), // Capital Pi
                new uc_CharSet (0x7C, 0x2260), // Not Equal 
                new uc_CharSet (0x7D, 0x00A3), // Pound Sign 
                new uc_CharSet (0x7E, 0x00B7), // Middle Dot 
            };

            public static uc_CharSet[] DECS =
            {
                new uc_CharSet (0xA8, 0x0020), // Currency Sign
                new uc_CharSet (0xD7, 0x0152), // latin small ligature OE 
                new uc_CharSet (0xDD, 0x0178), // Capital Y with diaeresis
                new uc_CharSet (0xF7, 0x0153), // latin small ligature oe 
                new uc_CharSet (0xFD, 0x00FF), // Lowercase y with diaeresis
            };

            public static uc_CharSet[] ASCII = // same as Basic Latin
            {
                new uc_CharSet (0x00, 0x0000), //
            };

            public static uc_CharSet[] NRCUK = // UK National Replacement
            {
                new uc_CharSet (0x23, 0x00A3), //
            };

            public static uc_CharSet[] NRCFinnish = // Finnish National Replacement
            {
                new uc_CharSet (0x5B, 0x00C4), // A with diearesis
                new uc_CharSet (0x5C, 0x00D6), // O with diearesis
                new uc_CharSet (0x5D, 0x00C5), // A with hollow dot above
                new uc_CharSet (0x5E, 0x00DC), // U with diearesis
                new uc_CharSet (0x60, 0x00E9), // e with accute accent
                new uc_CharSet (0x7B, 0x00E4), // a with diearesis
                new uc_CharSet (0x7C, 0x00F6), // o with diearesis
                new uc_CharSet (0x7D, 0x00E5), // a with hollow dot above
                new uc_CharSet (0x7E, 0x00FC), // u with diearesis
            };

            public static uc_CharSet[] NRCFrench = // French National Replacement
            {
                new uc_CharSet (0x23, 0x00A3), // Pound Sign
                new uc_CharSet (0x40, 0x00E0), // a with grav accent
                new uc_CharSet (0x5B, 0x00B0), // Degree Symbol
                new uc_CharSet (0x5C, 0x00E7), // little cedila
                new uc_CharSet (0x5D, 0x00A7), // funny s (technical term)
                new uc_CharSet (0x7B, 0x00E9), // e with accute accent
                new uc_CharSet (0x7C, 0x00F9), // u with grav accent
                new uc_CharSet (0x7D, 0x00E8), // e with grav accent
                new uc_CharSet (0x7E, 0x00A8), // diearesis
            };

            public static uc_CharSet[] NRCFrenchCanadian = // French Canadian National Replacement
            {
                new uc_CharSet (0x40, 0x00E0), // a with grav accent
                new uc_CharSet (0x5B, 0x00E2), // a with circumflex
                new uc_CharSet (0x5C, 0x00E7), // little cedila
                new uc_CharSet (0x5D, 0x00EA), // e with circumflex
                new uc_CharSet (0x5E, 0x00CE), // i with circumflex
                new uc_CharSet (0x60, 0x00F4), // o with circumflex
                new uc_CharSet (0x7B, 0x00E9), // e with accute accent
                new uc_CharSet (0x7C, 0x00F9), // u with grav accent
                new uc_CharSet (0x7D, 0x00E8), // e with grav accent
                new uc_CharSet (0x7E, 0x00FB), // u with circumflex
            };

            public static uc_CharSet[] NRCGerman = // German National Replacement
            {
                new uc_CharSet (0x40, 0x00A7), // funny s
                new uc_CharSet (0x5B, 0x00C4), // A with diearesis
                new uc_CharSet (0x5C, 0x00D6), // O with diearesis
                new uc_CharSet (0x5D, 0x00DC), // U with diearesis
                new uc_CharSet (0x7B, 0x00E4), // a with diearesis
                new uc_CharSet (0x7C, 0x00F6), // o with diearesis
                new uc_CharSet (0x7D, 0x00FC), // u with diearesis
                new uc_CharSet (0x7E, 0x00DF), // funny B
            };

            public static uc_CharSet[] NRCItalian = // Italian National Replacement
            {
                new uc_CharSet (0x23, 0x00A3), // pound sign
                new uc_CharSet (0x40, 0x00A7), // funny s
                new uc_CharSet (0x5B, 0x00B0), // Degree Symbol
                new uc_CharSet (0x5C, 0x00E7), // little cedilla
                new uc_CharSet (0x5D, 0x00E9), // e with accute accent
                new uc_CharSet (0x60, 0x00F9), // u with grav accent
                new uc_CharSet (0x7B, 0x00E0), // a with grav accent
                new uc_CharSet (0x7C, 0x00F2), // o with grav accent
                new uc_CharSet (0x7D, 0x00E8), // e with grav accent
                new uc_CharSet (0x7E, 0x00CC), // I with grav accent
            };

            public static uc_CharSet[] NRCNorDanish = // Norwegian Danish National Replacement
            {
                new uc_CharSet (0x5B, 0x00C6), // AE ligature
                new uc_CharSet (0x5C, 0x00D8), // O with strikethough
                new uc_CharSet (0x5D, 0x00D8), // O with strikethough
                new uc_CharSet (0x5D, 0x00C5), // A with hollow dot above
                new uc_CharSet (0x7B, 0x00E6), // ae ligature
                new uc_CharSet (0x7C, 0x00F8), // o with strikethough
                new uc_CharSet (0x7D, 0x00F8), // o with strikethough
                new uc_CharSet (0x7D, 0x00E5), // a with hollow dot above
            };

            public static uc_CharSet[] NRCPortuguese = // Portuguese National Replacement
            {
                new uc_CharSet (0x5B, 0x00C3), // A with tilde
                new uc_CharSet (0x5C, 0x00C7), // big cedilla
                new uc_CharSet (0x5D, 0x00D5), // O with tilde
                new uc_CharSet (0x7B, 0x00E3), // a with tilde
                new uc_CharSet (0x7C, 0x00E7), // little cedilla
                new uc_CharSet (0x7D, 0x00F6), // o with tilde
            };

            public static uc_CharSet[] NRCSpanish = // Spanish National Replacement
            {
                new uc_CharSet (0x23, 0x00A3), // pound sign
                new uc_CharSet (0x40, 0x00A7), // funny s
                new uc_CharSet (0x5B, 0x00A1), // I with dot
                new uc_CharSet (0x5C, 0x00D1), // N with tilde
                new uc_CharSet (0x5D, 0x00BF), // Upside down question mark
                new uc_CharSet (0x7B, 0x0060), // back single quote
                new uc_CharSet (0x7C, 0x00B0), // Degree Symbol
                new uc_CharSet (0x7D, 0x00F1), // n with tilde
                new uc_CharSet (0x7E, 0x00E7), // small cedilla
            };

            public static uc_CharSet[] NRCSwedish = // Swedish National Replacement
            {
                new uc_CharSet (0x40, 0x00C9), // E with acute
                new uc_CharSet (0x5B, 0x00C4), // A with diearesis
                new uc_CharSet (0x5C, 0x00D6), // O with diearesis
                new uc_CharSet (0x5D, 0x00C5), // A with hollow dot above
                new uc_CharSet (0x5E, 0x00DC), // U with diearesis
                new uc_CharSet (0x60, 0x00E9), // e with accute accent
                new uc_CharSet (0x7B, 0x00E4), // a with diearesis
                new uc_CharSet (0x7C, 0x00F6), // o with diearesis
                new uc_CharSet (0x7D, 0x00E5), // a with hollow dot above
                new uc_CharSet (0x7E, 0x00FC), // u with diearesis
            };

            public static uc_CharSet[] NRCSwiss = // Swiss National Replacement
            {
                new uc_CharSet (0x23, 0x00F9), // u with grav accent
                new uc_CharSet (0x40, 0x00E0), // a with grav accent
                new uc_CharSet (0x5B, 0x00E9), // e with accute accent
                new uc_CharSet (0x5C, 0x00E7), // small cedilla
                new uc_CharSet (0x5D, 0x00EA), // e with circumflex
                new uc_CharSet (0x5E, 0x00CE), // i with circumflex
                new uc_CharSet (0x5F, 0x00E8), // e with grav accent
                new uc_CharSet (0x60, 0x00F4), // o with circumflex
                new uc_CharSet (0x7B, 0x00E4), // a with diearesis
                new uc_CharSet (0x7C, 0x00F6), // o with diearesis
                new uc_CharSet (0x7D, 0x00FC), // u with diearesis
                new uc_CharSet (0x7E, 0x00FB), // u with circumflex
            };

            public static uc_CharSet[] ISOLatin1S = // Same as Latin-1 Supplemental
            {
                new uc_CharSet (0x00, 0x0000) //
            };
        }
    }
}
