using ErpConnector.Models;
using ErpConnector.Processors.IProcessor;
using ErpConnector.DTOs;

namespace ErpConnector.Processors
{
    public class CategoryProcessor : ICategoryProcessor
    {
        public Category Map(CategoryFromApiDto categoryDto)
        {
            return new Category
            {
                Name = categoryDto.Name ?? "Unnamed Category",
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
                ApiId = categoryDto.Slug ?? ""
            };
        }
    }
}
