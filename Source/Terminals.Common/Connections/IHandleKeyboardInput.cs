namespace Terminals.Connections
{
    public interface IHandleKeyboardInput
    {
        /// <summary>
        /// Gets or sets flag indicating the connection allows forward user keyboard input
        /// </summary>
        bool GrabInput { get; set; }
    }
}