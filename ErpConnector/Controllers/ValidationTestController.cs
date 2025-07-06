using ErpConnector.DTOs;
using ErpConnector.Services.IServices;
using Microsoft.Extensions.Logging;

namespace ErpConnector.Controllers
{
    public class ValidationTestController
    {
        private readonly ILogger<ValidationTestController> _logger;
        private readonly IValidationService _validationService;

        public ValidationTestController(ILogger<ValidationTestController> logger, IValidationService validationService)
        {
            _logger = logger;
            _validationService = validationService;
        }

        public async Task<bool> TestValidation()
        {
            try
            {
                Console.WriteLine("🧪 Testing FluentValidation...");
                Console.WriteLine("=====================================");

                // Test valid product
                Console.WriteLine("\n✅ Testing VALID Product:");
                var validProduct = new ProductFromApiDto
                {
                    Id = 1,
                    Title = "Test Product",
                    Price = 29.99m,
                    Description = "A great product for testing",
                    Category = "Electronics"
                };

                var productResult = _validationService.ValidateProduct(validProduct);
                if (_validationService.IsValid<ProductFromApiDto>(productResult, out var productErrors))
                {
                    Console.WriteLine($"✅ Product '{validProduct.Title}' is VALID");
                }
                else
                {
                    Console.WriteLine($"❌ Product validation failed: {string.Join(", ", productErrors)}");
                }

                // Test invalid product
                Console.WriteLine("\n❌ Testing INVALID Product:");
                var invalidProduct = new ProductFromApiDto
                {
                    Id = -1, // Invalid ID
                    Title = "", // Empty title
                    Price = -5.00m, // Negative price
                    Description = "", // Empty description (but this should be OK as it's optional)
                    Category = "" // Empty category
                };

                var invalidProductResult = _validationService.ValidateProduct(invalidProduct);
                if (!_validationService.IsValid<ProductFromApiDto>(invalidProductResult, out var invalidProductErrors))
                {
                    Console.WriteLine($"❌ Product validation failed as expected:");
                    foreach (var error in invalidProductErrors)
                    {
                        Console.WriteLine($"   • {error}");
                    }
                }

                // Test valid category
                Console.WriteLine("\n✅ Testing VALID Category:");
                var validCategory = new CategoryFromApiDto
                {
                    Name = "Electronics",
                    Slug = "electronics"
                };

                var categoryResult = _validationService.ValidateCategory(validCategory);
                if (_validationService.IsValid<CategoryFromApiDto>(categoryResult, out var categoryErrors))
                {
                    Console.WriteLine($"✅ Category '{validCategory.Name}' is VALID");
                }
                else
                {
                    Console.WriteLine($"❌ Category validation failed: {string.Join(", ", categoryErrors)}");
                }

                // Test invalid category
                Console.WriteLine("\n❌ Testing INVALID Category:");
                var invalidCategory = new CategoryFromApiDto
                {
                    Name = "", // Empty name
                    Slug = "Invalid Slug With Spaces!" // Invalid slug format
                };

                var invalidCategoryResult = _validationService.ValidateCategory(invalidCategory);
                if (!_validationService.IsValid<CategoryFromApiDto>(invalidCategoryResult, out var invalidCategoryErrors))
                {
                    Console.WriteLine($"❌ Category validation failed as expected:");
                    foreach (var error in invalidCategoryErrors)
                    {
                        Console.WriteLine($"   • {error}");
                    }
                }

                Console.WriteLine("\n=====================================");
                Console.WriteLine("✅ Validation testing completed successfully!");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during validation testing");
                Console.WriteLine($"❌ Validation testing failed: {ex.Message}");
                return false;
            }
        }
    }
}
