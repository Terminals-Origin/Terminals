using Terminals.Forms.Controls;

namespace Tests.UserInterface
{
    /// <summary>
    /// Customizable stub to simulate key modifiers pressed during drag and drop in tree
    /// </summary>
    internal class TestKeyModifiers : IKeyModifiers
    {
        public bool WithShift { get; set; }

        public bool WithControl { get; set; }
    }
}