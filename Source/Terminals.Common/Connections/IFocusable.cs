namespace Terminals.Connections
{
    public interface IFocusable
    {
        bool ContainsFocus { get; }

        void Focus();
    }
}