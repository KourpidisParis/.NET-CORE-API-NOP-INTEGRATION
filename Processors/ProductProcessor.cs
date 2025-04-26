using System.Reflection.Metadata.Ecma335;
using ErpConnector.Models;
using ErpConnector.Processors.IProcessor;

namespace ErpConnector.Processors
{
    public class ProductProcessor : IProductProcessor
    {
        public Product ApplyDefaultProductValues(Product product)
        {
            // product.Name ??= string.Empty;
            product.MetaKeywords ??= string.Empty;
            product.MetaTitle ??= string.Empty;
            // product.Sku ??= string.Empty;
            product.ManufacturerPartNumber ??= string.Empty;
            product.Gtin ??= string.Empty;
            product.RequiredProductIds ??= string.Empty;
            product.AllowedQuantities ??= string.Empty;
            product.ProductTypeId = 5;
            product.ParentGroupedProductId = 0;
            product.VisibleIndividually = true;
            product.ShortDescription ??= string.Empty;
            // product.FullDescription ??= string.Empty;
            product.AdminComment ??= string.Empty;
            product.ProductTemplateId = 1;
            product.VendorId = 1;
            product.ShowOnHomepage = false;
            product.MetaDescription ??= string.Empty;
            product.AllowCustomerReviews = true;
            product.SubjectToAcl = false;
            product.LimitedToStores = false;
            product.IsGiftCard = false;
            product.RequireOtherProducts = false;
            product.AutomaticallyAddRequiredProducts = false;
            product.IsDownload = false;
            product.UnlimitedDownloads = false;
            product.HasSampleDownload = false;
            product.HasUserAgreement = false;
            product.UserAgreementText ??= string.Empty;
            product.IsRecurring = false;
            product.IsRental = false;
            product.IsShipEnabled = true;
            product.IsFreeShipping = false;
            product.ShipSeparately = false;
            product.AdditionalShippingCharge = 0;
            product.IsTaxExempt = false;
            product.ManageInventoryMethodId = 1;
            product.StockQuantity = 10;
            product.DisplayStockAvailability = true;
            product.DisplayStockQuantity = true;
            product.MinStockQuantity = 1;
            product.LowStockActivityId = 1;
            product.NotifyAdminForQuantityBelow = 1;
            product.BackorderModeId = 0;
            product.AllowBackInStockSubscriptions = true;
            product.OrderMinimumQuantity = 1;
            product.OrderMaximumQuantity = 100;
            product.AllowAddingOnlyExistingAttributeCombinations = false;
            product.DisplayAttributeCombinationImagesOnly = false;
            product.NotReturnable = false;
            product.DisableBuyButton = false;
            product.DisableWishlistButton = false;
            product.AvailableForPreOrder = false;
            product.CallForPrice = false;
            // product.Price = 0;
            product.OldPrice = 0;
            product.ProductCost = 0;
            product.CustomerEntersPrice = false;
            product.MinimumCustomerEnteredPrice = 0;
            product.MaximumCustomerEnteredPrice = 1000;
            product.BasepriceEnabled = false;
            product.BasepriceAmount = 0;
            product.BasepriceBaseAmount = 0;
            product.MarkAsNew = false;
            product.Weight = 0;
            product.Length = 0;
            product.Width = 0;
            product.Height = 0;
            product.DisplayOrder = 0;
            product.Published = true;
            product.Deleted = false;
            product.CreatedOnUtc = product.CreatedOnUtc == default ? DateTime.UtcNow : product.CreatedOnUtc;
            product.UpdatedOnUtc = DateTime.UtcNow;

            return product;
        }

        private bool GetPublished(string availabilityStatus)
        {
            if(!string.IsNullOrEmpty(availabilityStatus))
            {
                if(availabilityStatus == "In Stock")
                {
                    return true;
                }
            }
            
            return false;
        }
    }
}
