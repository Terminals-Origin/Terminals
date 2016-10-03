namespace Terminals.Connections
{
    internal class PluginDefinition
    {
        internal string Description { get; private set; }

        internal string FullPath { get; private set; }

        public PluginDefinition(string fullPath, string description)
        {
            this.FullPath = fullPath;
            this.Description = description;
        }

        public override string ToString()
        {
            return this.Description;
        }
    }
}