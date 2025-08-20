
namespace MiniERP_Suministros.Core.Services.Shop
{
    /// <summary>
    /// Represents errors that occur with product category related operations.
    /// </summary>
    public class ProductCategoryException : Exception
    {
        /// <summary>Initializes a new instance of the <see cref="ProductCategoryException" /> class.</summary>
        public ProductCategoryException() : base("A ProductCategory Exception has occurred.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductCategoryException" /> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public ProductCategoryException(string? message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductCategoryException" /> class with a specified error message
        /// and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception, or a null reference (<see langword="Nothing" /> 
        /// in Visual Basic) if no inner exception is specified.
        /// </param>
        public ProductCategoryException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
