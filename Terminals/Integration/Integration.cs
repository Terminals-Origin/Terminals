using System;
using System.Collections.Generic;
using System.Text;

namespace Terminals.Integration
{
    public class Integration : Terminals.Integration.Import.IImport
    {

        #region IImport Members
        private static List<Type> Importers = null;
        public FavoriteConfigurationElementCollection ImportFavorites(string Filename)
        {
            string FileExtension = System.IO.Path.GetExtension(Filename).ToLower();
            Terminals.Integration.Import.IImport import = FindImportType(FileExtension);
            if(import==null)return null;
            return import.ImportFavorites(Filename);
        }
        private Terminals.Integration.Import.IImport FindImportType(string Extension)
        {
            Terminals.Integration.Import.IImport import = null;
            if(Importers == null)
            {
                System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location));
                string[] patterns = new string[] { "*.dll", "*.exe"};
                foreach(string pattern in patterns)
                {
                    foreach(System.IO.FileInfo fi in dir.GetFiles(pattern))
                    {
                        try
                        {
                            System.Reflection.Assembly asm = System.Reflection.Assembly.LoadFile(fi.FullName);
                            if(asm != null)
                            {                                
                                foreach(Type t in asm.GetTypes())
                                {
                                    try
                                    {
                                        if(typeof(Terminals.Integration.Import.IImport).IsAssignableFrom(t) && t.IsClass) 
                                        {
                                            if(Importers == null) Importers = new List<Type>();
                                            Importers.Add(t);
                                        }
                                    }
                                    catch (Exception exc) { Terminals.Logging.Log.Error("Error iterating Assemblies for Importer Classes", exc); }
                                }
                            }
                        }
                        catch(Exception exc)
                        {
                            Terminals.Logging.Log.Error("Error loading Assembly from Bin Folder" + fi.FullName, exc);
                            //do nothing
                        }

                    }
                }
            }
            if(Importers != null)
            {
                foreach(Type t in Importers)
                {
                    Terminals.Integration.Import.IImport i = (t.Assembly.CreateInstance(t.FullName) as Terminals.Integration.Import.IImport);
                    if(i != null && i.KnownExtension!=null)
                    {
                        if(i.KnownExtension == Extension)
                        {
                            import = i;
                            break;
                        }
                    }
                }
            }
            return import;
        }
        public string KnownExtension
        {
            get { return null; }
        }

        #endregion
    }
}
