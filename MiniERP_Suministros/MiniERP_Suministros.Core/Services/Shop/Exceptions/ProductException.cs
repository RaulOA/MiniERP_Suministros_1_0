
namespace MiniERP_Suministros.Core.Services.Shop
{
    /// <summary>
    /// Represents errors that occur with product related operations.
    /// </summary>
    public class ProductException : Exception
    {
        public ProductException() : base("A Product Exception has occurred.") { }
        public ProductException(string? message) : base(message) { }
        public ProductException(string? message, Exception? innerException) : base(message, innerException) { }
    }
}
