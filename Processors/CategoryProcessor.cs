using ErpConnector.Models;
using ErpConnector.Processors.IProcessor;

namespace ErpConnector.Processors
{
    public class CategoryProcessor : ICategoryProcessor
    {
        public Category ApplyDefaultCategoryValues(Category category)
        {
            // Set default values for fields not provided by API
            category.MetaKeywords ??= "";
            category.MetaTitle ??= "";
            category.PageSizeOptions ??= "";
            category.Description ??= "";
            category.MetaDescription ??= "";

            category.CategoryTemplateId = 1; // Default template id
            category.ParentCategoryId = 0;
            category.PictureId = 0;
            category.PageSize = 10; // Default page size
            category.AllowCustomersToSelectPageSize = false;
            category.ShowOnHomepage = false;
            category.IncludeInTopMenu = false;
            category.SubjectToAcl = false;
            category.LimitedToStores = false;
            category.Published = true;
            category.Deleted = false;
            category.DisplayOrder = 0;
            category.CreatedOnUtc = DateTime.UtcNow;
            category.UpdatedOnUtc = DateTime.UtcNow;
            category.PriceRangeFiltering = false;
            category.PriceFrom = 0m;
            category.PriceTo = 0m;
            category.ManuallyPriceRange = false;
            category.RestrictFromVendors = false;

            return category;
        }
    }
}
