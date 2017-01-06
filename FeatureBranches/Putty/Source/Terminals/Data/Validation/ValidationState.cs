namespace Terminals.Data.Validation
{
    /// <summary>
    /// Custom business logic container to hold the error messages obtained during datamodel validations.
    /// Used to validate one data object.
    /// </summary>
    internal class ValidationState
    {
        internal string PropertyName { get; set; }

        /// <summary>
        /// Gets or sets a localized message to be shown as validation result
        /// </summary>
        internal string Message { get; set; }

        public override string ToString()
        {
            return string.Format("ValidationState:{0}=>{1}", this.PropertyName, this.Message);
        }
    }
}