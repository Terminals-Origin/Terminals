namespace Terminals.Forms.Controls
{
    internal interface IKeyModifiers
    {
        /// <summary>
        /// Gets true, if user holds the shift key; otherwise false.
        /// </summary>
        bool WithShift { get; }

        /// <summary>
        /// Gets true, if user holds Control key; otherwise false.
        /// </summary>
        bool WithControl { get; }
    }
}