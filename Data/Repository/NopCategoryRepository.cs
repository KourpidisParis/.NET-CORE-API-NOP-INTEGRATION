using ErpConnector.Data;
using ErpConnector.Models;
using ErpConnector.Repository.IRepository;

namespace ErpConnector.Repository
{
    public class NopCategoryRepository : INopCategoryRepository
    {
        private readonly DataContextDapper _dapper;

        public NopCategoryRepository(DataContextDapper dapper)
        {
            _dapper = dapper;
        }

        public async Task<int?> GetCategoryIdByExternalId(string apiId)
        {
            var id = _dapper.LoadDataSingle<int?>(
                "SELECT TOP 1 Id FROM [nop].[dbo].[Category] WHERE ApiId = @ApiId",
                new { ApiId = apiId });

            return await Task.FromResult(id);
        }

        public async Task InsertCategory(Category category)
        {
            string sql = @"
                INSERT INTO [nop].[dbo].[Category] 
                (Name, MetaKeywords, MetaTitle, PageSizeOptions, Description, CategoryTemplateId, 
                 MetaDescription, ParentCategoryId, PictureId, PageSize, AllowCustomersToSelectPageSize, 
                 ShowOnHomepage, IncludeInTopMenu, SubjectToAcl, LimitedToStores, Published, Deleted, 
                 DisplayOrder, CreatedOnUtc, UpdatedOnUtc, PriceRangeFiltering, PriceFrom, PriceTo, 
                 ManuallyPriceRange, RestrictFromVendors, ApiId)
                VALUES
                (@Name, @MetaKeywords, @MetaTitle, @PageSizeOptions, @Description, @CategoryTemplateId, 
                 @MetaDescription, @ParentCategoryId, @PictureId, @PageSize, @AllowCustomersToSelectPageSize, 
                 @ShowOnHomepage, @IncludeInTopMenu, @SubjectToAcl, @LimitedToStores, @Published, @Deleted, 
                 @DisplayOrder, @CreatedOnUtc, @UpdatedOnUtc, @PriceRangeFiltering, @PriceFrom, @PriceTo, 
                 @ManuallyPriceRange, @RestrictFromVendors, @ApiId);";

            _dapper.Execute(sql, category);
            await Task.CompletedTask;
        }

        public async Task UpdateCategory(Category category, int id)
        {
            string sql = @"
                UPDATE [nop].[dbo].[Category]
                SET Name = @Name,
                    MetaKeywords = @MetaKeywords,
                    MetaTitle = @MetaTitle,
                    PageSizeOptions = @PageSizeOptions,
                    Description = @Description,
                    CategoryTemplateId = @CategoryTemplateId,
                    MetaDescription = @MetaDescription,
                    ParentCategoryId = @ParentCategoryId,
                    PictureId = @PictureId,
                    PageSize = @PageSize,
                    AllowCustomersToSelectPageSize = @AllowCustomersToSelectPageSize,
                    ShowOnHomepage = @ShowOnHomepage,
                    IncludeInTopMenu = @IncludeInTopMenu,
                    SubjectToAcl = @SubjectToAcl,
                    LimitedToStores = @LimitedToStores,
                    Published = @Published,
                    Deleted = @Deleted,
                    DisplayOrder = @DisplayOrder,
                    CreatedOnUtc = @CreatedOnUtc,
                    UpdatedOnUtc = @UpdatedOnUtc,
                    PriceRangeFiltering = @PriceRangeFiltering,
                    PriceFrom = @PriceFrom,
                    PriceTo = @PriceTo,
                    ManuallyPriceRange = @ManuallyPriceRange,
                    RestrictFromVendors = @RestrictFromVendors
                WHERE Id = @Id;";

            _dapper.Execute(sql, new
            {
                category.Name,
                category.MetaKeywords,
                category.MetaTitle,
                category.PageSizeOptions,
                category.Description,
                category.CategoryTemplateId,
                category.MetaDescription,
                category.ParentCategoryId,
                category.PictureId,
                category.PageSize,
                category.AllowCustomersToSelectPageSize,
                category.ShowOnHomepage,
                category.IncludeInTopMenu,
                category.SubjectToAcl,
                category.LimitedToStores,
                category.Published,
                category.Deleted,
                category.DisplayOrder,
                category.CreatedOnUtc,
                category.UpdatedOnUtc,
                category.PriceRangeFiltering,
                category.PriceFrom,
                category.PriceTo,
                category.ManuallyPriceRange,
                category.RestrictFromVendors,
                Id = id
            });

            await Task.CompletedTask;
        }
    }
}
