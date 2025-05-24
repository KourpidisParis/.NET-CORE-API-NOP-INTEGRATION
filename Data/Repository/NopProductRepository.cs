using ErpConnector.Data;
using ErpConnector.Repository.IRepository;
using ErpConnector.Models;

namespace ErpConnector.Repository
{
    public class NopProductRepository : INopProductRepository
    {
        private readonly DataContextDapper _dapper;
        public NopProductRepository(DataContextDapper dapper)
        {
            _dapper = dapper;
        }

        //Product Base Section
        public async Task<int?> GetProductIdByExternalId(string apiId)
        {
           var id = _dapper.LoadDataSingle<int?>(
                "SELECT TOP 1 Id FROM [nop].[dbo].[Product] WHERE ApiId = @ApiId",
                new { ApiId = apiId });

            return await Task.FromResult(id);        
        }

        public async Task<int> InsertProduct(Product product)
        {
            string sql = @"
                INSERT INTO [nop].[dbo].[Product] (
                    Name, MetaKeywords, MetaTitle, Sku, ManufacturerPartNumber, Gtin,
                    RequiredProductIds, AllowedQuantities, ProductTypeId, ParentGroupedProductId,
                    VisibleIndividually, ShortDescription, FullDescription, AdminComment,
                    ProductTemplateId, VendorId, ShowOnHomepage, MetaDescription, AllowCustomerReviews,
                    ApprovedRatingSum, NotApprovedRatingSum, ApprovedTotalReviews, NotApprovedTotalReviews,
                    SubjectToAcl, LimitedToStores, IsGiftCard, GiftCardTypeId, OverriddenGiftCardAmount,
                    RequireOtherProducts, AutomaticallyAddRequiredProducts, IsDownload, DownloadId,
                    UnlimitedDownloads, MaxNumberOfDownloads, DownloadExpirationDays, DownloadActivationTypeId,
                    HasSampleDownload, SampleDownloadId, HasUserAgreement, UserAgreementText,
                    IsRecurring, RecurringCycleLength, RecurringCyclePeriodId, RecurringTotalCycles,
                    IsRental, RentalPriceLength, RentalPricePeriodId, IsShipEnabled,
                    IsFreeShipping, ShipSeparately, AdditionalShippingCharge, DeliveryDateId,
                    IsTaxExempt, TaxCategoryId, ManageInventoryMethodId, ProductAvailabilityRangeId,
                    UseMultipleWarehouses, WarehouseId, StockQuantity, DisplayStockAvailability,
                    DisplayStockQuantity, MinStockQuantity, LowStockActivityId, NotifyAdminForQuantityBelow,
                    BackorderModeId, AllowBackInStockSubscriptions, OrderMinimumQuantity, OrderMaximumQuantity,
                    AllowAddingOnlyExistingAttributeCombinations, DisplayAttributeCombinationImagesOnly,
                    NotReturnable, DisableBuyButton, DisableWishlistButton, AvailableForPreOrder,
                    PreOrderAvailabilityStartDateTimeUtc, CallForPrice, Price, OldPrice, ProductCost,
                    CustomerEntersPrice, MinimumCustomerEnteredPrice, MaximumCustomerEnteredPrice,
                    BasepriceEnabled, BasepriceAmount, BasepriceUnitId, BasepriceBaseAmount, BasepriceBaseUnitId,
                    MarkAsNew, MarkAsNewStartDateTimeUtc, MarkAsNewEndDateTimeUtc,
                    Weight, Length, Width, Height,
                    AvailableStartDateTimeUtc, AvailableEndDateTimeUtc,
                    DisplayOrder, Published, Deleted,
                    CreatedOnUtc, UpdatedOnUtc, ApiId
                )
                VALUES (
                    @Name, @MetaKeywords, @MetaTitle, @Sku, @ManufacturerPartNumber, @Gtin,
                    @RequiredProductIds, @AllowedQuantities, @ProductTypeId, @ParentGroupedProductId,
                    @VisibleIndividually, @ShortDescription, @FullDescription, @AdminComment,
                    @ProductTemplateId, @VendorId, @ShowOnHomepage, @MetaDescription, @AllowCustomerReviews,
                    @ApprovedRatingSum, @NotApprovedRatingSum, @ApprovedTotalReviews, @NotApprovedTotalReviews,
                    @SubjectToAcl, @LimitedToStores, @IsGiftCard, @GiftCardTypeId, @OverriddenGiftCardAmount,
                    @RequireOtherProducts, @AutomaticallyAddRequiredProducts, @IsDownload, @DownloadId,
                    @UnlimitedDownloads, @MaxNumberOfDownloads, @DownloadExpirationDays, @DownloadActivationTypeId,
                    @HasSampleDownload, @SampleDownloadId, @HasUserAgreement, @UserAgreementText,
                    @IsRecurring, @RecurringCycleLength, @RecurringCyclePeriodId, @RecurringTotalCycles,
                    @IsRental, @RentalPriceLength, @RentalPricePeriodId, @IsShipEnabled,
                    @IsFreeShipping, @ShipSeparately, @AdditionalShippingCharge, @DeliveryDateId,
                    @IsTaxExempt, @TaxCategoryId, @ManageInventoryMethodId, @ProductAvailabilityRangeId,
                    @UseMultipleWarehouses, @WarehouseId, @StockQuantity, @DisplayStockAvailability,
                    @DisplayStockQuantity, @MinStockQuantity, @LowStockActivityId, @NotifyAdminForQuantityBelow,
                    @BackorderModeId, @AllowBackInStockSubscriptions, @OrderMinimumQuantity, @OrderMaximumQuantity,
                    @AllowAddingOnlyExistingAttributeCombinations, @DisplayAttributeCombinationImagesOnly,
                    @NotReturnable, @DisableBuyButton, @DisableWishlistButton, @AvailableForPreOrder,
                    @PreOrderAvailabilityStartDateTimeUtc, @CallForPrice, @Price, @OldPrice, @ProductCost,
                    @CustomerEntersPrice, @MinimumCustomerEnteredPrice, @MaximumCustomerEnteredPrice,
                    @BasepriceEnabled, @BasepriceAmount, @BasepriceUnitId, @BasepriceBaseAmount, @BasepriceBaseUnitId,
                    @MarkAsNew, @MarkAsNewStartDateTimeUtc, @MarkAsNewEndDateTimeUtc,
                    @Weight, @Length, @Width, @Height,
                    @AvailableStartDateTimeUtc, @AvailableEndDateTimeUtc,
                    @DisplayOrder, @Published, @Deleted,
                    @CreatedOnUtc, @UpdatedOnUtc, @ApiId
                );
                SELECT CAST(SCOPE_IDENTITY() AS INT)
            ";

            var newId = _dapper.LoadDataSingle<int>(sql, product);

            return await Task.FromResult(newId);
        }


        public async Task UpdateProduct(Product product, int id)
        {
            string sql = @"
                UPDATE [nop].[dbo].[Product]
                SET Name = @Name,
                    ShortDescription = @Description,
                    Price = @Price
                WHERE Id = @Id;";

            _dapper.Execute(sql, new
            {
                Name = product.Name,
                Description = product.FullDescription,
                Price = product.Price,
                Id = id
            });

            await Task.CompletedTask;
        }

        //Product to Category section
        public async Task<int?> GetCategoryIdByApiId(string? apiId)
        {
            var categoryId = _dapper.LoadDataSingle<int?>(
                "SELECT TOP 1 Id FROM [nop].[dbo].[Category] WHERE ApiId = @ApiId",
                new { ApiId = apiId });

            return await Task.FromResult(categoryId);
        }

        public async Task InsertProductCategoryMapping(int productId, int categoryId)
        {
            string sql = @"
                INSERT INTO [nop].[dbo].[Product_Category_Mapping] (
                    ProductId, CategoryId, IsFeaturedProduct, DisplayOrder
                )
                VALUES (
                    @ProductId, @CategoryId, 0, 0
                );";

            _dapper.Execute(sql, new { ProductId = productId, CategoryId = categoryId });

            await Task.CompletedTask;
        }

        public bool GetProductCategoryMapping(int productId, int categoryId)
        {
            string sql = @"SELECT COUNT(1) 
                          FROM [nop].[dbo].[Product_Category_Mapping] 
                          WHERE ProductId = @ProductId AND CategoryId = @CategoryId";
            
            var result = _dapper.LoadDataSingle<int>(sql, new { ProductId = productId, CategoryId = categoryId });

            return result > 0;
        }

        //Product Descriptions section
        public async Task<IEnumerable<LocalizedProperty>> GetAllLocalizedProperties()
        {
            string sql = @"
                SELECT *
                FROM [nop].[dbo].[LocalizedProperty]";
            
            var localizedProperties = _dapper.LoadData<LocalizedProperty>(sql);
            
            return await Task.FromResult(localizedProperties);
        }

        public async Task<int> InsertLocalizedProperty(LocalizedProperty localizedProperty)
        {
            string sql = @"
                INSERT INTO [nop].[dbo].[LocalizedProperty]
                (LocaleKeyGroup, LocaleKey, LocaleValue, LanguageId, EntityId)
                VALUES
                (@LocaleKeyGroup, @LocaleKey, @LocaleValue, @LanguageId, @EntityId);
                SELECT CAST(SCOPE_IDENTITY() AS INT)";
            
            var newId = _dapper.LoadDataSingle<int>(sql, localizedProperty);
            
            return await Task.FromResult(newId);
        }

        public async Task UpdateLocalizedProperty(int id, string localeValue)
        {
            string sql = @"
                UPDATE [nop].[dbo].[LocalizedProperty]
                SET LocaleValue = @LocaleValue
                WHERE Id = @Id";
            
            _dapper.Execute(sql, new { Id = id, LocaleValue = localeValue });
            
            await Task.CompletedTask;
        }

        public async Task<int?> GetLocalizedPropertyId(string localeKeyGroup, string localeKey, int entityId, int languageId)
        {
            string sql = @"
                SELECT Id FROM [nop].[dbo].[LocalizedProperty]
                WHERE LocaleKeyGroup = @LocaleKeyGroup
                AND LocaleKey = @LocaleKey
                AND EntityId = @EntityId
                AND LanguageId = @LanguageId";
            
            var id = _dapper.LoadDataSingle<int?>(sql, new { 
                LocaleKeyGroup = localeKeyGroup,
                LocaleKey = localeKey,
                EntityId = entityId,
                LanguageId = languageId
            });
            
            return await Task.FromResult(id);
        }
    }
}