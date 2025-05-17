using ErpConnector.DTOs;
using ErpConnector.Models;

namespace ErpConnector.Mappers.IMappers
{
    /// <summary>
    /// Interface for product mapping operations
    /// </summary>
    public interface IProductMapper
    {
        /// <summary>
        /// Maps a product DTO from API to a Product domain model
        /// </summary>
        /// <param name="dto">Product data from API</param>
        /// <returns>Mapped Product with default values applied</returns>
        Product MapToProduct(ProductFromApiDto dto);
    }
}
