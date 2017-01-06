namespace Terminals.Forms.OptionPanels
{
    internal class SelectedPlugin
    {
        internal string Description { get; private set; }

        internal string FullPath { get; private set; }

        public bool Enabled { get; set; }

        public SelectedPlugin(string description, string fullPath, bool isEnabled)
        {
            this.FullPath = fullPath;
            this.Description = description;
            this.Enabled = isEnabled;
        }

        public override string ToString()
        {
            return this.Description;
        }
    }
}