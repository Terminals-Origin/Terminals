namespace Terminals.Data.DB
{
    /// <summary>
    /// Database context access property to allow entities work within their data context
    /// </summary>
    interface IEntityContext
    {
        /// <summary>
        /// Gets or sets the database proxy instance to which entity belonges to.
        /// Null in case the entity is new, not added to context yet.
        /// </summary>
        DataBase Database { get; set; }
    }
}
