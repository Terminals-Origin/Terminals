using System.Windows.Forms;
using Terminals.Data;
using Terminals.Forms.EditFavorite;

namespace Tests.UserInterface
{
    /// <summary>
    /// Because of issue with Control mocking. When calling Control.Add 
    /// the mock failes on Object null reference exception.
    /// </summary>
    internal class MockChildProtocolControl : Control, IProtocolOptionsControl
    {
        internal bool Loaded { get; private set; }

        internal bool Saved { get; private set; }

        public void LoadFrom(IFavorite favorite)
        {
            this.Loaded = true;
        }

        public void SaveTo(IFavorite favorite)
        {
            this.Saved = true;
        }
    }
}