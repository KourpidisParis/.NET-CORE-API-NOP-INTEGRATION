using ErpConnector.DTOs;
using ErpConnector.Models;
using ErpConnector.Mappers.IMappers;
using ErpConnector.Repository.IRepository;
using ErpConnector.Services;
using ErpConnector.Services.IServices;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using FluentAssertions;
using FluentValidation.Results;

namespace ErpConnector.Tests.Services
{
    public class NopProductServiceTests
    {
        private readonly Mock<ILogger<NopProductService>> _mockLogger;
        private readonly Mock<INopProductRepository> _mockProductRepository;
        private readonly Mock<INopLocalizedPropertyService> _mockLocalizedPropertyService;
        private readonly Mock<IProductMapper> _mockProductMapper;
        private readonly Mock<IValidationService> _mockValidationService;
        private readonly NopProductService _productService;

        public NopProductServiceTests()
        {
            _mockLogger = new Mock<ILogger<NopProductService>>();
            _mockProductRepository = new Mock<INopProductRepository>();
            _mockLocalizedPropertyService = new Mock<INopLocalizedPropertyService>();
            _mockProductMapper = new Mock<IProductMapper>();
            _mockValidationService = new Mock<IValidationService>();

            _productService = new NopProductService(
                _mockLogger.Object,
                _mockProductRepository.Object,
                _mockLocalizedPropertyService.Object,
                _mockProductMapper.Object,
                _mockValidationService.Object
            );
        }

        #region SyncProducts Tests

        [Fact]
        public async Task SyncProducts_WithValidProducts_ShouldProcessSuccessfully()
        {
            // Arrange
            var products = new List<ProductFromApiDto>
            {
                new() { Id = 1, Title = "Product 1", Price = 10.99m, Description = "Desc 1", Category = "Cat1" }
            };

            var productModel = new Product { ApiId = 1, Name = "Product 1", Price = 10.99m, Category = "Cat1" };
            var validationResult = new ValidationResult();

            SetupMocks(products[0], productModel, validationResult, true);
            _mockProductRepository.Setup(x => x.GetProductIdByExternalId("1")).ReturnsAsync((int?)null);
            _mockProductRepository.Setup(x => x.InsertProduct(It.IsAny<Product>())).ReturnsAsync(1);

            // Act
            await _productService.SyncProducts(products);

            // Assert
            _mockProductRepository.Verify(x => x.InsertProduct(It.IsAny<Product>()), Times.Once);
            _mockValidationService.Verify(x => x.ValidateProduct(It.IsAny<ProductFromApiDto>()), Times.Once);
        }

        [Fact]
        public async Task SyncProducts_WithExistingProduct_ShouldUpdateProduct()
        {
            // Arrange
            var products = new List<ProductFromApiDto>
            {
                new() { Id = 1, Title = "Product 1", Price = 10.99m, Description = "Desc 1", Category = "Cat1" }
            };

            var productModel = new Product { ApiId = 1, Name = "Product 1", Price = 10.99m, Category = "Cat1" };
            var validationResult = new ValidationResult();

            SetupMocks(products[0], productModel, validationResult, true);
            _mockProductRepository.Setup(x => x.GetProductIdByExternalId("1")).ReturnsAsync(1);

            // Act
            await _productService.SyncProducts(products);

            // Assert
            _mockProductRepository.Verify(x => x.UpdateProduct(It.IsAny<Product>(), 1), Times.Once);
            _mockProductRepository.Verify(x => x.InsertProduct(It.IsAny<Product>()), Times.Never);
        }

        [Fact]
        public async Task SyncProducts_WithInvalidProduct_ShouldSkipProductWithoutThrowing()
        {
            // Arrange
            var products = new List<ProductFromApiDto>
            {
                new() { Id = 1, Title = "", Price = -1, Description = "Desc 1", Category = "Cat1" }
            };

            var validationResult = new ValidationResult();
            validationResult.Errors.Add(new ValidationFailure("Title", "Title is required"));

            // Setup the validation to return false for IsValid
            _mockValidationService.Setup(x => x.ValidateProduct(It.IsAny<ProductFromApiDto>()))
                .Returns(validationResult);
            
            // Setup IsValid to return false and provide error list
            var errorList = new List<string> { "Title is required" };
            _mockValidationService.Setup(x => x.IsValid<ProductFromApiDto>(validationResult, out errorList))
                .Returns(false);

            // Act - Should complete without throwing since validation errors don't count as processing errors
            await _productService.SyncProducts(products);

            // Assert
            _mockProductRepository.Verify(x => x.InsertProduct(It.IsAny<Product>()), Times.Never);
            _mockProductRepository.Verify(x => x.UpdateProduct(It.IsAny<Product>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task SyncProducts_WithNullApiId_ShouldSkipProduct()
        {
            // Arrange
            var products = new List<ProductFromApiDto>
            {
                new() { Id = 1, Title = "Product 1", Price = 10.99m, Description = "Desc 1", Category = "Cat1" }
            };

            var productModel = new Product { ApiId = null, Name = "Product 1", Price = 10.99m };
            var validationResult = new ValidationResult();

            SetupMocks(products[0], productModel, validationResult, true);

            // Act
            await _productService.SyncProducts(products);

            // Assert
            _mockProductRepository.Verify(x => x.InsertProduct(It.IsAny<Product>()), Times.Never);
            _mockProductRepository.Verify(x => x.UpdateProduct(It.IsAny<Product>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task SyncProducts_WithDatabaseError_ShouldThrowException()
        {
            // Arrange
            var products = new List<ProductFromApiDto>
            {
                new() { Id = 1, Title = "Product 1", Price = 10.99m, Description = "Desc 1", Category = "Cat1" }
            };

            var productModel = new Product { ApiId = 1, Name = "Product 1", Price = 10.99m, Category = "Cat1" };
            var validationResult = new ValidationResult();

            SetupMocks(products[0], productModel, validationResult, true);
            _mockProductRepository.Setup(x => x.GetProductIdByExternalId("1"))
                .ThrowsAsync(new InvalidOperationException("Database connection failed"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => _productService.SyncProducts(products));
            
            exception.Message.Should().Contain("error");
        }

        [Fact]
        public async Task SyncProducts_WithMultipleProducts_ShouldProcessAll()
        {
            // Arrange
            var products = new List<ProductFromApiDto>
            {
                new() { Id = 1, Title = "Product 1", Price = 10.99m, Description = "Desc 1", Category = "Cat1" },
                new() { Id = 2, Title = "Product 2", Price = 20.99m, Description = "Desc 2", Category = "Cat2" }
            };

            var productModel1 = new Product { ApiId = 1, Name = "Product 1", Price = 10.99m, Category = "Cat1" };
            var productModel2 = new Product { ApiId = 2, Name = "Product 2", Price = 20.99m, Category = "Cat2" };
            var validationResult = new ValidationResult();

            SetupMocks(products[0], productModel1, validationResult, true);
            SetupMocks(products[1], productModel2, validationResult, true);

            _mockProductRepository.Setup(x => x.GetProductIdByExternalId("1")).ReturnsAsync((int?)null);
            _mockProductRepository.Setup(x => x.GetProductIdByExternalId("2")).ReturnsAsync((int?)null);
            _mockProductRepository.Setup(x => x.InsertProduct(It.IsAny<Product>())).ReturnsAsync(1);

            // Act
            await _productService.SyncProducts(products);

            // Assert
            _mockProductRepository.Verify(x => x.InsertProduct(It.IsAny<Product>()), Times.Exactly(2));
        }

        #endregion

        #region MapProductToCategory Tests

        [Fact]
        public async Task MapProductToCategory_WithValidData_ShouldCreateMapping()
        {
            // Arrange
            var product = new Product { ApiId = 1, Category = "Electronics" };
            var productId = 1;

            _mockProductRepository.Setup(x => x.GetCategoryIdByApiId("Electronics")).ReturnsAsync(10);
            _mockProductRepository.Setup(x => x.GetProductCategoryMapping(productId, 10)).ReturnsAsync(false);

            // Act
            await _productService.MapProductToCategory(product, productId);

            // Assert
            _mockProductRepository.Verify(x => x.InsertProductCategoryMapping(productId, 10), Times.Once);
        }

        [Fact]
        public async Task MapProductToCategory_WithExistingMapping_ShouldNotCreateDuplicate()
        {
            // Arrange
            var product = new Product { ApiId = 1, Category = "Electronics" };
            var productId = 1;

            _mockProductRepository.Setup(x => x.GetCategoryIdByApiId("Electronics")).ReturnsAsync(10);
            _mockProductRepository.Setup(x => x.GetProductCategoryMapping(productId, 10)).ReturnsAsync(true);

            // Act
            await _productService.MapProductToCategory(product, productId);

            // Assert
            _mockProductRepository.Verify(x => x.InsertProductCategoryMapping(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task MapProductToCategory_WithInvalidProductId_ShouldReturn()
        {
            // Arrange
            var product = new Product { ApiId = 1, Category = "Electronics" };
            var productId = 0;

            // Act
            await _productService.MapProductToCategory(product, productId);

            // Assert
            _mockProductRepository.Verify(x => x.GetCategoryIdByApiId(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task MapProductToCategory_WithNullProduct_ShouldReturn()
        {
            // Arrange
            Product? product = null;
            var productId = 1;

            // Act
            await _productService.MapProductToCategory(product!, productId);

            // Assert
            _mockProductRepository.Verify(x => x.GetCategoryIdByApiId(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task MapProductToCategory_WithNonExistentCategory_ShouldReturn()
        {
            // Arrange
            var product = new Product { ApiId = 1, Category = "NonExistent" };
            var productId = 1;

            _mockProductRepository.Setup(x => x.GetCategoryIdByApiId("NonExistent")).ReturnsAsync((int?)null);

            // Act
            await _productService.MapProductToCategory(product, productId);

            // Assert
            _mockProductRepository.Verify(x => x.InsertProductCategoryMapping(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task MapProductToCategory_WithDatabaseError_ShouldThrowException()
        {
            // Arrange
            var product = new Product { ApiId = 1, Category = "Electronics" };
            var productId = 1;

            _mockProductRepository.Setup(x => x.GetCategoryIdByApiId("Electronics"))
                .ThrowsAsync(new InvalidOperationException("Database connection failed"));

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(
                () => _productService.MapProductToCategory(product, productId));
        }

        #endregion

        #region Helper Methods

        private void SetupMocks(ProductFromApiDto productDto, Product productModel, ValidationResult validationResult, bool isValid)
        {
            _mockValidationService.Setup(x => x.ValidateProduct(productDto)).Returns(validationResult);
            
            var errorList = new List<string>();
            _mockValidationService.Setup(x => x.IsValid<ProductFromApiDto>(validationResult, out errorList))
                .Returns(isValid);
            
            _mockProductMapper.Setup(x => x.MapToProduct(productDto)).Returns(productModel);
            
            if (isValid && productModel.ApiId.HasValue)
            {
                _mockProductRepository.Setup(x => x.GetCategoryIdByApiId(productModel.Category ?? "")).ReturnsAsync(1);
                _mockProductRepository.Setup(x => x.GetProductCategoryMapping(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(false);
                _mockProductRepository.Setup(x => x.InsertProductCategoryMapping(It.IsAny<int>(), It.IsAny<int>())).Returns(Task.CompletedTask);
                _mockLocalizedPropertyService.Setup(x => x.HandleLocalizedProperty(It.IsAny<LocalizedProperty>())).Returns(Task.CompletedTask);
            }
        }

        #endregion
    }
}