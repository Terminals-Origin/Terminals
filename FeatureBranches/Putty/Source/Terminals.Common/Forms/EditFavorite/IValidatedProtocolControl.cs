using System.ComponentModel;

namespace Terminals.Forms.EditFavorite
{
    public interface IValidatedProtocolControl
    {
        /// <summary>
        /// Control fires this event to let it self be validated, if contains valid integer.
        /// </summary>
        event CancelEventHandler IntegerValidationRequested;
    }
}