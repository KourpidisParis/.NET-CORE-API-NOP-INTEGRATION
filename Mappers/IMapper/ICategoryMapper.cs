using ErpConnector.DTOs;
using ErpConnector.Models;

namespace ErpConnector.Mappers.IMappers
{
    /// <summary>
    /// Interface for product mapping operations
    /// </summary>
    public interface ICategoryMapper
    {
        /// <summary>
        /// Maps a category DTO from API to a Category domain model
        /// </summary>
        /// <param name="dto">Category data from API</param>
        /// <returns>Mapped Product with default values applied</returns>
        Category MapToCategory(CategoryFromApiDto dto);
    }
}
