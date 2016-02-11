namespace Terminals.Connections
{
    public interface IToolbarExtenderFactory
    {
        IToolbarExtender CreateToolbarExtender(ICurrenctConnectionProvider provider);
    }
}