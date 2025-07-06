using ErpConnector.DTOs;
using ErpConnector.Models;
using ErpConnector.Mappers.IMappers;

namespace ErpConnector.Mappers
{
    /// <summary>
    /// Implementation of product mapping logic
    /// </summary>
    public class ProductMapper : IProductMapper
    {
        /// <summary>
        /// Maps a product DTO from API to a Product domain model with all required default values
        /// </summary>
        /// <param name="dto">Product data from API</param>
        /// <returns>Mapped Product with default values applied</returns>
        /// <exception cref="ArgumentNullException">Thrown when dto is null</exception>
        public Product MapToProduct(ProductFromApiDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            return new Product
            {
                // Mapped properties from DTO
                Sku = dto.Id.ToString(),
                ApiId = dto.Id,
                Price = dto.Price,
                Name = dto.Title ?? string.Empty,
                Category = dto.Category ?? string.Empty,
                FullDescription = dto.Description ?? string.Empty,
                
                // Default values for all required properties
                MetaKeywords = string.Empty,
                MetaTitle = string.Empty,
                ManufacturerPartNumber = string.Empty,
                Gtin = string.Empty,
                RequiredProductIds = string.Empty,
                AllowedQuantities = string.Empty,
                ProductTypeId = 5,
                ParentGroupedProductId = 0,
                VisibleIndividually = true,
                ShortDescription = string.Empty,
                AdminComment = string.Empty,
                ProductTemplateId = 1,
                VendorId = 1,
                ShowOnHomepage = false,
                MetaDescription = string.Empty,
                AllowCustomerReviews = true,
                SubjectToAcl = false,
                LimitedToStores = false,
                IsGiftCard = false,
                GiftCardTypeId = 0,
                OverriddenGiftCardAmount = null,
                RequireOtherProducts = false,
                AutomaticallyAddRequiredProducts = false,
                IsDownload = false,
                DownloadId = 0,
                UnlimitedDownloads = false,
                MaxNumberOfDownloads = 0,
                DownloadExpirationDays = null,
                DownloadActivationTypeId = 0,
                HasSampleDownload = false,
                SampleDownloadId = 0,
                HasUserAgreement = false,
                UserAgreementText = string.Empty,
                IsRecurring = false,
                RecurringCycleLength = 0,
                RecurringCyclePeriodId = 0,
                RecurringTotalCycles = 0,
                IsRental = false,
                RentalPriceLength = 0,
                RentalPricePeriodId = 0,
                IsShipEnabled = true,
                IsFreeShipping = false,
                ShipSeparately = false,
                AdditionalShippingCharge = 0,
                DeliveryDateId = 0,
                IsTaxExempt = false,
                TaxCategoryId = 0,
                ManageInventoryMethodId = 1,
                ProductAvailabilityRangeId = 0,
                UseMultipleWarehouses = false,
                WarehouseId = 0,
                StockQuantity = 10,
                DisplayStockAvailability = true,
                DisplayStockQuantity = true,
                MinStockQuantity = 1,
                LowStockActivityId = 1,
                NotifyAdminForQuantityBelow = 1,
                BackorderModeId = 0,
                AllowBackInStockSubscriptions = true,
                OrderMinimumQuantity = 1,
                OrderMaximumQuantity = 100,
                AllowAddingOnlyExistingAttributeCombinations = false,
                DisplayAttributeCombinationImagesOnly = false,
                NotReturnable = false,
                DisableBuyButton = false,
                DisableWishlistButton = false,
                AvailableForPreOrder = false,
                PreOrderAvailabilityStartDateTimeUtc = null,
                CallForPrice = false,
                OldPrice = 0,
                ProductCost = 0,
                CustomerEntersPrice = false,
                MinimumCustomerEnteredPrice = 0,
                MaximumCustomerEnteredPrice = 1000,
                BasepriceEnabled = false,
                BasepriceAmount = 0,
                BasepriceUnitId = 0,
                BasepriceBaseAmount = 0,
                BasepriceBaseUnitId = 0,
                MarkAsNew = false,
                MarkAsNewStartDateTimeUtc = null,
                MarkAsNewEndDateTimeUtc = null,
                Weight = 0,
                Length = 0,
                Width = 0,
                Height = 0,
                AvailableStartDateTimeUtc = null,
                AvailableEndDateTimeUtc = null,
                DisplayOrder = 0,
                Published = true,
                Deleted = false,
                CreatedOnUtc = DateTime.UtcNow,
                UpdatedOnUtc = DateTime.UtcNow,
                
                // Any additional business logic for mapping can be added here
                // Example: ApprovedRatingSum = CalculateRating(dto)
            };
        }
        
        /// <summary>
        /// Determines if a product should be published based on its availability status
        /// </summary>
        /// <param name="availabilityStatus">The availability status string</param>
        /// <returns>True if the product should be published, false otherwise</returns>
        private bool GetPublished(string availabilityStatus)
        {
            if (!string.IsNullOrEmpty(availabilityStatus))
            {
                if (availabilityStatus == "In Stock")
                {
                    return true;
                }
            }
            
            return false;
        }
    }
}
