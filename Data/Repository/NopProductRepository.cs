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

        public async Task<int?> GetCategoryIdByApiId(string apiId)
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
    }
}