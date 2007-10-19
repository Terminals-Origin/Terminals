using System;
using System.Collections.Generic;
using System.Text;

namespace Terminals.Integration.Import
{
    public class ImportRDP : IImport
    {
        #region IImport Members

        FavoriteConfigurationElementCollection IImport.ImportFavorites(string Filename)
        {
            string name = System.IO.Path.GetFileName(Filename).Replace(System.IO.Path.GetExtension(Filename),"");
            FavoriteConfigurationElementCollection coll = new FavoriteConfigurationElementCollection();
            if(System.IO.File.Exists(Filename))
            {
                FavoriteConfigurationElement fav = null;
                string[] lines = System.IO.File.ReadAllLines(Filename);
                fav = new FavoriteConfigurationElement();
                fav.Name = name;
                coll.Add(fav);
                foreach(string line in lines)
                {

                    string propertyName = line.Substring(0, line.IndexOf(":"));
                    string pValue = line.Substring(line.LastIndexOf(":") + 1);
                    switch(propertyName)
                    {
                        case "full address":
                            fav.ServerName = pValue;
                            
                            break;
                        case "server port":
                            int p = 3389;
                            int.TryParse(pValue, out p);
                            fav.Port = p;
                            break;
                        case "username":
                            fav.UserName = pValue;
                            break;
                        case "domain":
                            fav.DomainName = pValue;
                            break;
                        case "session bpp":
                            switch(pValue)
                            {
                                case "8":
                                    fav.Colors = Colors.Bits8;
                                    break;
                                case "16":
                                    fav.Colors = Colors.Bit16;
                                    break;
                                case "24":
                                    fav.Colors = Colors.Bits24;
                                    break;
                                case "32":
                                    fav.Colors = Colors.Bits32;
                                    break;
                                default:
                                    fav.Colors = Colors.Bit16;
                                    break;
                            }

                            break;
                        case "screen mode id":
                            fav.DesktopSize = DesktopSize.AutoScale;
                            if(pValue == "1") fav.DesktopSize = DesktopSize.FullScreen;
                            break;
                        case "connect to console":
                            fav.ConnectToConsole = false;
                            if(pValue == "1") fav.ConnectToConsole = true;
                            break;
                        case "disable wallpaper":
                            fav.DisableWallPaper = false;
                            if(pValue == "1") fav.DisableWallPaper = true;
                            break;
                        case "redirectsmartcards":
                            fav.RedirectSmartCards = false;
                            if(pValue == "1") fav.RedirectSmartCards = true;
                            break;
                        case "redirectdrives":
                            fav.RedirectDrives = false;
                            if(pValue == "1") fav.RedirectDrives = true;
                            break;
                        case "redirectcomports":
                            fav.RedirectPorts = false;
                            if(pValue == "1") fav.RedirectPorts = true;
                            break;
                        case "redirectprinters":
                            fav.RedirectPrinters = false;
                            if(pValue == "1") fav.RedirectPrinters = true;
                            break;
                        case "audiomode":
                            if(pValue == "0") fav.Sounds = RemoteSounds.Redirect;
                            if(pValue == "1") fav.Sounds = RemoteSounds.PlayOnServer;
                            if(pValue == "2") fav.Sounds = RemoteSounds.DontPlay;
                            break;
                        default:
                            break;
                    }

                }
            }
            return coll;
        }

        public string KnownExtension
        {
            get { return ".rdp"; }
        }

        #endregion
    }
}