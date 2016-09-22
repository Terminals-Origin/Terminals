using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Terminals.Connections
{
    internal class PluginsLoader
    {
        public IEnumerable<IConnectionPlugin> Load()
        {
            string[] pluginDirectories = FindPluginDirectories();
            List<string> allPluginAssemblies = FindAllPluginAssemblies(pluginDirectories);
            return allPluginAssemblies.SelectMany(LoadAssemblyPlugins).ToList();
        }

        private static IEnumerable<IConnectionPlugin> LoadAssemblyPlugins(string pluginFile)
        {
            Assembly pluginAssembly = Assembly.LoadFrom(pluginFile);
            var types = FindAllPluginTypes(pluginAssembly);
            return types.Select(Activator.CreateInstance).OfType<IConnectionPlugin>();
        }

        private static IEnumerable<Type> FindAllPluginTypes(Assembly pluginAssembly)
        {
            return pluginAssembly.GetTypes()
                .Where(t => !t.IsAbstract && !t.IsInterface && t.GetInterface(typeof(IConnectionPlugin).FullName) != null);
        }

        private static string[] FindPluginDirectories()
        {
            string applicationDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string pluginsDirectory = Path.Combine(applicationDirectory, "Plugins");
            return Directory.GetDirectories(pluginsDirectory);
        }

        private static List<string> FindAllPluginAssemblies(string[] pluginDirectories)
        {
            return pluginDirectories.SelectMany(FindPluginAssemblies)
                .ToList();
        }

        private static string[] FindPluginAssemblies(string pluginDirectory)
        {
            return Directory.GetFiles(pluginDirectory, "Terminals.Plugins.*.dll");
        }
    }
}