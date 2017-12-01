namespace Terminals.Updates
{
    internal interface IVersionUpgrade
    {
        void Upgrade();

        bool NeedExecute();
    }
}