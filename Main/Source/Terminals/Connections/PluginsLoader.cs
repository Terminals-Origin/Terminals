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
            try
            {
                Assembly pluginAssembly = Assembly.LoadFrom(pluginFile);
                var types = FindAllPluginTypes(pluginAssembly);
                return types.Select(Activator.CreateInstance).OfType<IConnectionPlugin>();
            }
            catch (Exception exception)
            {
                string message = string.Format("Unable to load plugins from '{0}'.", pluginFile);
                Logging.Info(message, exception);
                return new IConnectionPlugin[0];
            }
        }

        private static IEnumerable<Type> FindAllPluginTypes(Assembly pluginAssembly)
        {
            return pluginAssembly.GetTypes()
                .Where(t => !t.IsAbstract && !t.IsInterface && t.GetInterface(typeof(IConnectionPlugin).FullName) != null);
        }

        private static string[] FindPluginDirectories()
        {
            try
            {
                string applicationDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string pluginsDirectory = Path.Combine(applicationDirectory, "Plugins");
                return Directory.GetDirectories(pluginsDirectory);
            }
            catch (Exception exception)
            {
                Logging.Info("Unable to open plugins directory.", exception);
                return new string[0];
            }
        }

        private static List<string> FindAllPluginAssemblies(string[] pluginDirectories)
        {
            return pluginDirectories.SelectMany(FindPluginAssemblies)
                .ToList();
        }

        private static string[] FindPluginAssemblies(string pluginDirectory)
        {
            try
            {
                return Directory.GetFiles(pluginDirectory, "Terminals.Plugins.*.dll");
            }
            catch (Exception exception)
            {
                string message = string.Format("Unable to load plugins list from '{0}'.", pluginDirectory);
                Logging.Info(message, exception);
                return new string[0];
            }
        }
    }
}