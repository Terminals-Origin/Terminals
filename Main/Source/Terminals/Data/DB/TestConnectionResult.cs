namespace Terminals.Data.DB
{
    /// <summary>
    /// Container, which explains results of try to connect operations.
    /// </summary>
    internal class TestConnectionResult
    {
        /// <summary>
        /// Gets true, if try was successful and master password was successfully validated; otherwise false.
        /// </summary>
        internal bool Successful { get; private set; }

        /// <summary>
        /// Gets not empty string, if connection wasn't successful to explain the purpose.
        /// </summary>
        internal string ErroMessage { get; private set; }

        /// <summary>
        /// Initializes new successful connection result
        /// </summary>
        internal TestConnectionResult()
        {
            this.Successful = true;
        }

        /// <summary>
        /// Initializes new not successful connection result.
        /// </summary>
        internal TestConnectionResult(string errorMessage)
        {
            this.Successful = false;
            this.ErroMessage = errorMessage;
        }

        public override string ToString()
        {
            return string.Format("TestConnectionResult:Successful={0},ErrorMessage={1}", this.Successful, this.ErroMessage);
        }
    }
}
