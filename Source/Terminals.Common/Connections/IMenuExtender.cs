using System.Windows.Forms;

namespace Terminals.Connections
{
    public interface IMenuExtender
    {
        void Visit(MenuStrip menuStrip);
    }
}