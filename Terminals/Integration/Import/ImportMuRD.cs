using System;
using System.Collections.Generic;
using System.Text;

namespace Terminals.Integration.Import
{
    public class ImportMuRD : IImport
    {
        #region IImport Members

        FavoriteConfigurationElementCollection IImport.ImportFavorites(string Filename)
        {
            FavoriteConfigurationElementCollection coll = new FavoriteConfigurationElementCollection();
            if(System.IO.File.Exists(Filename))
            {
                FavoriteConfigurationElement fav = null;
                string[] lines = System.IO.File.ReadAllLines(Filename);
                foreach(string line in lines)
                {
                    if(line.StartsWith("[") && line.EndsWith("]"))
                    {
                        if(fav == null)
                        {
                            fav = new FavoriteConfigurationElement();
                            fav.Name = line.Substring(1, line.Length - 2);
                        }
                        else
                        {
                            coll.Add(fav);
                            fav = new FavoriteConfigurationElement();
                        }
                    }
                    else
                    {
                        if(line == "")
                        {
                            coll.Add(fav);
                            fav = new FavoriteConfigurationElement();
                        }
                        else
                        {
                            string propertyName = line.Substring(0, line.IndexOf("="));
                            string pValue = line.Substring(line.IndexOf("=") + 1);
                            switch(propertyName)
                            {
                                case "ServerName":
                                    fav.ServerName = pValue;
                                    break;
                                case "Port":
                                    int p = 3389;
                                    int.TryParse(pValue, out p);
                                    fav.Port = p;
                                    break;
                                case "Username":
                                    fav.UserName = pValue;
                                    break;
                                case "Domain":
                                    fav.DomainName = pValue;
                                    break;
                                case "Comment":
                                    break;
                                case "Password":
                                    break;
                                case "DesktopWidth":
                                    fav.DesktopSize = DesktopSize.AutoScale;
                                    break;
                                case "DesktopHeight":
                                    fav.DesktopSize = DesktopSize.AutoScale;
                                    break;
                                case "ColorDepth":
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
                                case "UseSmartSizing":
                                    if(pValue == "1") fav.DesktopSize = DesktopSize.AutoScale;
                                    break;
                                case "FullScreenMonitor":
                                    if(pValue == "1") fav.DesktopSize = DesktopSize.FullScreen;
                                    break;
                                case "ConsoleConnection":
                                    fav.ConnectToConsole = false;
                                    if(pValue == "1") fav.ConnectToConsole = true;
                                    break;
                                case "UseCompression":
                                    break;
                                case "BitmapPersistence":
                                    break;
                                case "DisableWallpaper":
                                    fav.ShowDesktopBackground = false;
                                    if(pValue == "1") fav.ShowDesktopBackground = true;
                                    break;
                                case "DisableFullWindowDrag":
                                    break;
                                case "DisableMenuAnimations":
                                    break;
                                case "DisableTheming":
                                    break;
                                case "DisableCursorShadow":
                                    break;
                                case "DisableCursorSettings":
                                    break;
                                case "RedirectSmartCards":
                                    fav.RedirectSmartCards = false;
                                    if(pValue == "1") fav.RedirectSmartCards = true;
                                    break;
                                case "RedirectDrives":
                                    fav.RedirectDrives = false;
                                    if(pValue == "1") fav.RedirectDrives = true;
                                    break;
                                case "RedirectPorts":
                                    fav.RedirectPorts = false;
                                    if(pValue == "1") fav.RedirectPorts = true;
                                    break;
                                case "RedirectPrinters":
                                    fav.RedirectPrinters = false;
                                    if(pValue == "1") fav.RedirectPrinters = true;
                                    break;
                                case "EnableWindowsKey":
                                    break;
                                case "KeyboardHookMode":
                                    break;
                                case "AudioRedirectionMode":
                                    if(pValue == "0") fav.Sounds = RemoteSounds.Redirect;
                                    if(pValue == "1") fav.Sounds = RemoteSounds.PlayOnServer;
                                    if(pValue == "2") fav.Sounds = RemoteSounds.DontPlay;
                                    break;
                                case "AlternateShell":
                                    break;
                                case "ShellWorkingDir":
                                    break;
                                case "AuthenticationLevel":
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
            }

            return coll;
        }
        public string KnownExtension
        {
            get { return ".mrc"; }
        }

        #endregion
    }
}