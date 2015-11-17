namespace Terminals.Connections
{
    internal interface IToolbarExtenderFactory
    {
        IToolbarExtender CreateToolbarExtender(ICurrenctConnectionProvider provider);
    }
}