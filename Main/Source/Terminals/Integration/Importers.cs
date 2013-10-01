using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Terminals.Integration.Import
{
    internal class Importers : Integration<IImport>
    {
        internal string GetProvidersDialogFilter()
        {
            // LoadImportersFromAssemblies();
            LoadProviders();

            StringBuilder stringBuilder = new StringBuilder();
            // work with copy because it is modified
            Dictionary<string, IImport> extraImporters = new Dictionary<string, IImport>(providers);
            AddTerminalsImporter(extraImporters, stringBuilder);

            foreach (KeyValuePair<string, IImport> importer in extraImporters)
            {
                AddProviderFilter(stringBuilder, importer.Value);
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Forces terminals importer to be on first place
        /// </summary>
        private void AddTerminalsImporter(Dictionary<string, IImport> extraImporters, StringBuilder stringBuilder)
        {
            if (extraImporters.ContainsKey(ImportTerminals.TERMINALS_FILEEXTENSION))
            {
                IImport terminalsImporter = extraImporters[ImportTerminals.TERMINALS_FILEEXTENSION];
                AddProviderFilter(stringBuilder, terminalsImporter);
                extraImporters.Remove(ImportTerminals.TERMINALS_FILEEXTENSION); 
            }
        }

        /// <summary>
        /// Loads a new collection of favorites from source file.
        /// The newly created favorites aren't imported into configuration.
        /// </summary>
        internal List<FavoriteConfigurationElement> ImportFavorites(String Filename)
        {
            IImport importer = FindProvider(Filename);

            if (importer == null)
                return new List<FavoriteConfigurationElement>();

            return importer.ImportFavorites(Filename);
        }

        internal List<FavoriteConfigurationElement> ImportFavorites(String[] files)
        {
            var favorites =  new List<FavoriteConfigurationElement>();
            foreach (string file in files)
            {
                favorites.AddRange(ImportFavorites(file));
            }
            return favorites;
        }

        protected override void LoadProviders()
        {
            if (providers == null)
            {
                providers = new Dictionary<string, IImport>();
                providers.Add(ImportTerminals.TERMINALS_FILEEXTENSION, new ImportTerminals());
                providers.Add(ImportRDP.FILE_EXTENSION, new ImportRDP());
                providers.Add(ImportvRD.FILE_EXTENSION, new ImportvRD());
                providers.Add(ImportMuRD.FILE_EXTENSION, new ImportMuRD());
            }
        }

        /// <summary>
        /// Disabled because of performance, there is no need to search all libraries,
        /// because importers are implemented only in Terminals
        /// </summary>
        private void LoadImportersFromAssemblies()
        {
            if (providers == null)
            {
                providers = new Dictionary<string, IImport>();
                DirectoryInfo dir = new DirectoryInfo(Program.Info.Location);
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

        private void LoadAssemblyImporters(string assemblyFileFullName)
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
                Logging.Error("Error loading Assembly from Bin Folder" + assemblyFileFullName, exc);
            }
        }

        private void LoadAssemblyImporters(Assembly assembly)
        {
            foreach (Type type in assembly.GetTypes())
            {
                LoadAssemblyImporter(type);
            }
        }

        private void LoadAssemblyImporter(Type type)
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
                Logging.Error("Error iterating Assemblies for Importer Classes", exc);
            }
        }

        private void AddImporter(IImport importer)
        {
            if (importer != null)
            {
                string extension = importer.KnownExtension.ToLower();
                if (ShouldAddImporterExtension(extension))
                {
                    providers.Add(extension, importer);
                }
            }
        }

        private bool ShouldAddImporterExtension(string extension)
        {
            return !String.IsNullOrEmpty(extension) &&
                   !providers.ContainsKey(extension);
        }
    }
}
