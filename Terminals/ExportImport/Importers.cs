using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Terminals.Integration.Import
{
    internal static class Importers
    {
        private static Dictionary<string, IImport> importers = null;

        internal static string GetImportersDialogFilter()
        {
            // LoadImportersFromAssemblies();
            LoadImporters();

            StringBuilder stringBuilder = new StringBuilder();
            // work with copy because it is modified
            Dictionary<string, IImport> extraImporters = new Dictionary<string, IImport>(importers);
            AddTerminalsImporter(extraImporters, stringBuilder);

            foreach (KeyValuePair<string, IImport> importer in extraImporters)
            {
                AddImporterFilter(stringBuilder, importer.Value);
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Forces terminals importer to be on first place
        /// </summary>
        private static void AddTerminalsImporter(Dictionary<string, IImport> extraImporters, StringBuilder stringBuilder)
        {
            if (extraImporters.ContainsKey(ImportTerminals.TERMINALS_FILEEXTENSION))
            {
                IImport terminalsImporter = extraImporters[ImportTerminals.TERMINALS_FILEEXTENSION];
                AddImporterFilter(stringBuilder, terminalsImporter);
                extraImporters.Remove(ImportTerminals.TERMINALS_FILEEXTENSION); 
            }
        }

        private static void AddImporterFilter(StringBuilder stringBuilder, IImport importer)
        {
            if (stringBuilder.Length != 0)
            {
                stringBuilder.Append("|");
            }

            stringBuilder.Append(importer.Name);
            stringBuilder.Append(" (*");
            stringBuilder.Append(importer.KnownExtension);
            stringBuilder.Append(")|*");
            stringBuilder.Append(importer.KnownExtension); // already in lowercase
        }

        internal static List<FavoriteConfigurationElement> ImportFavorites(string Filename)
        {
            IImport importer = FindImporter(Filename);

            if (importer == null)
                return new List<FavoriteConfigurationElement>();

            return importer.ImportFavorites(Filename);
        }

        private static IImport FindImporter(string fileName)
        {
            //LoadImportersFromAssemblies();
            LoadImporters();

            string extension = Path.GetExtension(fileName);
            if (extension == null)
            {
                return null;
            }

            string knownExtension = extension.ToLower();

            if (importers.ContainsKey(knownExtension))
                return importers[knownExtension];

            return null;
        }

        private static void LoadImporters()
        {
            if (importers == null)
            {
                importers = new Dictionary<string, IImport>();
                importers.Add(ImportTerminals.TERMINALS_FILEEXTENSION, new ImportTerminals());
                importers.Add(ImportRDP.FILE_EXTENSION, new ImportRDP());
                importers.Add(ImportvRD.FILE_EXTENSION, new ImportvRD());
                importers.Add(ImportMuRD.FILE_EXTENSION, new ImportMuRD());
            }
        }

        /// <summary>
        /// Disabled because of performance, there is no need to search all libraries,
        /// because importers are implemented only in Terminals
        /// </summary>
        private static void LoadImportersFromAssemblies()
        {
            if (importers == null)
            {
                importers = new Dictionary<string, IImport>();
                DirectoryInfo dir = new DirectoryInfo(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
                string[] patterns = new string[] { "*.dll", "*.exe" };

                foreach (string pattern in patterns)
                {
                    foreach (FileInfo assemblyFile in dir.GetFiles(pattern))
                    {
                        LoadAssemblyImporters(assemblyFile.FullName);
                    }
                }
            }
        }

        private static void LoadAssemblyImporters(string assemblyFileFullName)
        {
            try
            {
                Assembly assembly = Assembly.LoadFile(assemblyFileFullName);
                if (assembly != null)
                {
                    LoadAssemblyImporters(assembly);
                }
            }
            catch (Exception exc) //do nothing
            {
                Logging.Log.Error("Error loading Assembly from Bin Folder" + assemblyFileFullName, exc);
            }
        }

        private static void LoadAssemblyImporters(Assembly assembly)
        {
            foreach (Type type in assembly.GetTypes())
            {
                LoadAssemblyImporter(type);
            }
        }

        private static void LoadAssemblyImporter(Type type)
        {
            try
            {
                if (typeof(IImport).IsAssignableFrom(type) && type.IsClass)
                {
                    IImport importer = type.Assembly.CreateInstance(type.FullName) as IImport;
                    AddImporter(importer);
                }
            }
            catch (Exception exc)
            {
                Logging.Log.Error("Error iterating Assemblies for Importer Classes", exc);
            }
        }

        private static void AddImporter(IImport importer)
        {
            if (importer != null)
            {
                string extension = importer.KnownExtension.ToLower();
                if (ShouldAddImporterExtension(extension))
                {
                    importers.Add(extension, importer);
                }
            }
        }

        private static bool ShouldAddImporterExtension(string extension)
        {
            return !String.IsNullOrEmpty(extension) &&
                   !importers.ContainsKey(extension);
        }
    }
}
