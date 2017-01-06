namespace Terminals.Connections
{
    public interface IExtraDetection
    {
        /// <summary>
        /// Returns true, if extra detection is valid service on the target ipAddress and port;
        /// otherwise false.
        /// </summary>
        bool IsValid(string ipAddress, int port);
    }
}