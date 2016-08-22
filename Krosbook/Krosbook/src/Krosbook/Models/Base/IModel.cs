namespace Krosbook.Models.Base
{
    /// <summary>
    /// Interface for Db models.
    /// </summary>
    public interface IModel
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        int Id { get; set; }
    }
}
