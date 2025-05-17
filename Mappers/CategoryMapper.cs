using ErpConnector.DTOs;
using ErpConnector.Models;
using ErpConnector.Mappers.IMappers;

namespace ErpConnector.Mappers
{
    /// <summary>
    /// Implementation of category mapping logic
    /// </summary>
    public class CategoryMapper : ICategoryMapper
    {
        /// <summary>
        /// Maps a Category DTO from API to a Category domain model with all required default values
        /// </summary>
        /// <param name="dto">Category data from API</param>
        /// <returns>Mapped Category with default values applied</returns>
        /// <exception cref="ArgumentNullException">Thrown when dto is null</exception>
        public Category MapToCategory(CategoryFromApiDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            return new Category
            {
                Name = dto.Name ?? "Unnamed Category",
                MetaKeywords = "",
                MetaTitle = "",
                PageSizeOptions = "",
                Description = "",
                MetaDescription = "",
                CategoryTemplateId = 1,
                ParentCategoryId = 0,
                PictureId = 0,
                PageSize = 10,
                AllowCustomersToSelectPageSize = false,
                ShowOnHomepage = false,
                IncludeInTopMenu = false,
                SubjectToAcl = false,
                LimitedToStores = false,
                Published = true,
                Deleted = false,
                DisplayOrder = 0,
                CreatedOnUtc = DateTime.UtcNow,
                UpdatedOnUtc = DateTime.UtcNow,
                PriceRangeFiltering = false,
                PriceFrom = 0m,
                PriceTo = 0m,
                ManuallyPriceRange = false,
                RestrictFromVendors = false,
                ApiId = dto.Slug ?? ""
            };
        }
    }
}
