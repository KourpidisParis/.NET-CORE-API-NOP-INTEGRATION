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
    public class NopCategoryServiceTests
    {
        private readonly Mock<ILogger<NopCategoryService>> _mockLogger;
        private readonly Mock<INopCategoryRepository> _mockCategoryRepository;
        private readonly Mock<ICategoryMapper> _mockCategoryMapper;
        private readonly Mock<IValidationService> _mockValidationService;
        private readonly NopCategoryService _categoryService;

        public NopCategoryServiceTests()
        {
            _mockLogger = new Mock<ILogger<NopCategoryService>>();
            _mockCategoryRepository = new Mock<INopCategoryRepository>();
            _mockCategoryMapper = new Mock<ICategoryMapper>();
            _mockValidationService = new Mock<IValidationService>();

            _categoryService = new NopCategoryService(
                _mockLogger.Object,
                _mockCategoryRepository.Object,
                _mockCategoryMapper.Object,
                _mockValidationService.Object
            );
        }

        #region SyncCategories Tests

        [Fact]
        public async Task SyncCategories_WithValidCategories_ShouldProcessSuccessfully()
        {
            // Arrange
            var categories = new List<CategoryFromApiDto>
            {
                new() { Name = "Electronics", Slug = "electronics" }
            };

            var categoryModel = new Category
            {
                Name = "Electronics",
                ApiId = "electronics",
                Description = "Electronics category"
            };

            var validationResult = new ValidationResult();

            SetupMocks(categories[0], categoryModel, validationResult, true);
            _mockCategoryRepository.Setup(x => x.GetCategoryIdByExternalId("electronics")).ReturnsAsync((int?)null);

            // Act
            await _categoryService.SyncCategories(categories);

            // Assert
            _mockCategoryRepository.Verify(x => x.InsertCategory(It.IsAny<Category>()), Times.Once);
            _mockValidationService.Verify(x => x.ValidateCategory(It.IsAny<CategoryFromApiDto>()), Times.Once);
        }

        [Fact]
        public async Task SyncCategories_WithExistingCategory_ShouldUpdateCategory()
        {
            // Arrange
            var categories = new List<CategoryFromApiDto>
            {
                new() { Name = "Electronics", Slug = "electronics" }
            };

            var categoryModel = new Category
            {
                Name = "Electronics",
                ApiId = "electronics",
                Description = "Electronics category"
            };

            var validationResult = new ValidationResult();

            SetupMocks(categories[0], categoryModel, validationResult, true);
            _mockCategoryRepository.Setup(x => x.GetCategoryIdByExternalId("electronics")).ReturnsAsync(1);

            // Act
            await _categoryService.SyncCategories(categories);

            // Assert
            _mockCategoryRepository.Verify(x => x.UpdateCategory(It.IsAny<Category>(), 1), Times.Once);
            _mockCategoryRepository.Verify(x => x.InsertCategory(It.IsAny<Category>()), Times.Never);
        }

        [Fact]
        public async Task SyncCategories_WithInvalidCategory_ShouldSkipCategoryWithoutThrowing()
        {
            // Arrange
            var categories = new List<CategoryFromApiDto>
            {
                new() { Name = "", Slug = "invalid-slug" }
            };

            var validationResult = new ValidationResult();
            validationResult.Errors.Add(new ValidationFailure("Name", "Name is required"));

            // Setup the validation to return false for IsValid
            _mockValidationService.Setup(x => x.ValidateCategory(It.IsAny<CategoryFromApiDto>()))
                .Returns(validationResult);
            
            // Setup IsValid to return false and provide error list
            var errorList = new List<string> { "Name is required" };
            _mockValidationService.Setup(x => x.IsValid<CategoryFromApiDto>(validationResult, out errorList))
                .Returns(false);

            // Act - Should complete without throwing since validation errors don't count as processing errors
            await _categoryService.SyncCategories(categories);

            // Assert
            _mockCategoryRepository.Verify(x => x.InsertCategory(It.IsAny<Category>()), Times.Never);
            _mockCategoryRepository.Verify(x => x.UpdateCategory(It.IsAny<Category>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task SyncCategories_WithNullOrEmptyApiId_ShouldSkipCategory()
        {
            // Arrange
            var categories = new List<CategoryFromApiDto>
            {
                new() { Name = "Valid Category", Slug = "valid-category" }
            };

            var categoryModel = new Category
            {
                Name = "Valid Category",
                ApiId = null, // null ApiId
                Description = "Valid category"
            };

            var validationResult = new ValidationResult();

            SetupMocks(categories[0], categoryModel, validationResult, true);

            // Act
            await _categoryService.SyncCategories(categories);

            // Assert
            _mockCategoryRepository.Verify(x => x.InsertCategory(It.IsAny<Category>()), Times.Never);
            _mockCategoryRepository.Verify(x => x.UpdateCategory(It.IsAny<Category>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task SyncCategories_WithEmptyApiId_ShouldSkipCategory()
        {
            // Arrange
            var categories = new List<CategoryFromApiDto>
            {
                new() { Name = "Valid Category", Slug = "valid-category" }
            };

            var categoryModel = new Category
            {
                Name = "Valid Category",
                ApiId = "", // empty ApiId
                Description = "Valid category"
            };

            var validationResult = new ValidationResult();

            SetupMocks(categories[0], categoryModel, validationResult, true);

            // Act
            await _categoryService.SyncCategories(categories);

            // Assert
            _mockCategoryRepository.Verify(x => x.InsertCategory(It.IsAny<Category>()), Times.Never);
            _mockCategoryRepository.Verify(x => x.UpdateCategory(It.IsAny<Category>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task SyncCategories_WithDatabaseError_ShouldThrowException()
        {
            // Arrange
            var categories = new List<CategoryFromApiDto>
            {
                new() { Name = "Electronics", Slug = "electronics" }
            };

            var categoryModel = new Category
            {
                Name = "Electronics",
                ApiId = "electronics",
                Description = "Electronics category"
            };

            var validationResult = new ValidationResult();

            SetupMocks(categories[0], categoryModel, validationResult, true);
            _mockCategoryRepository.Setup(x => x.GetCategoryIdByExternalId("electronics"))
                .ThrowsAsync(new InvalidOperationException("Database connection failed"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => _categoryService.SyncCategories(categories));

            exception.Message.Should().Contain("error");
        }

        [Fact]
        public async Task SyncCategories_WithMultipleCategories_ShouldProcessAll()
        {
            // Arrange
            var categories = new List<CategoryFromApiDto>
            {
                new() { Name = "Electronics", Slug = "electronics" },
                new() { Name = "Books", Slug = "books" },
                new() { Name = "Clothing", Slug = "clothing" }
            };

            var categoryModels = new[]
            {
                new Category { Name = "Electronics", ApiId = "electronics", Description = "Electronics category" },
                new Category { Name = "Books", ApiId = "books", Description = "Books category" },
                new Category { Name = "Clothing", ApiId = "clothing", Description = "Clothing category" }
            };

            var validationResult = new ValidationResult();

            for (int i = 0; i < categories.Count; i++)
            {
                SetupMocks(categories[i], categoryModels[i], validationResult, true);
                _mockCategoryRepository.Setup(x => x.GetCategoryIdByExternalId(categoryModels[i].ApiId!))
                    .ReturnsAsync((int?)null);
            }

            // Act
            await _categoryService.SyncCategories(categories);

            // Assert
            _mockCategoryRepository.Verify(x => x.InsertCategory(It.IsAny<Category>()), Times.Exactly(3));
            _mockValidationService.Verify(x => x.ValidateCategory(It.IsAny<CategoryFromApiDto>()), Times.Exactly(3));
        }

        [Fact]
        public async Task SyncCategories_WithMixedValidAndInvalidCategories_ShouldProcessValidOnesWithoutThrowing()
        {
            // Arrange
            var categories = new List<CategoryFromApiDto>
            {
                new() { Name = "Electronics", Slug = "electronics" }, // Valid
                new() { Name = "", Slug = "invalid" },                 // Invalid
                new() { Name = "Books", Slug = "books" }               // Valid
            };

            var validValidationResult = new ValidationResult();
            var invalidValidationResult = new ValidationResult();
            invalidValidationResult.Errors.Add(new ValidationFailure("Name", "Name is required"));

            var validCategoryModel1 = new Category { Name = "Electronics", ApiId = "electronics", Description = "Electronics" };
            var validCategoryModel2 = new Category { Name = "Books", ApiId = "books", Description = "Books" };

            // Setup for valid categories
            SetupMocks(categories[0], validCategoryModel1, validValidationResult, true);
            SetupMocks(categories[2], validCategoryModel2, validValidationResult, true);

            // Setup for invalid category  
            _mockValidationService.Setup(x => x.ValidateCategory(categories[1])).Returns(invalidValidationResult);
            var errorList = new List<string> { "Name is required" };
            _mockValidationService.Setup(x => x.IsValid<CategoryFromApiDto>(invalidValidationResult, out errorList))
                .Returns(false);

            _mockCategoryRepository.Setup(x => x.GetCategoryIdByExternalId("electronics")).ReturnsAsync((int?)null);
            _mockCategoryRepository.Setup(x => x.GetCategoryIdByExternalId("books")).ReturnsAsync((int?)null);

            // Act - Should complete without throwing since validation errors don't count as processing errors
            await _categoryService.SyncCategories(categories);

            // Assert
            _mockCategoryRepository.Verify(x => x.InsertCategory(It.IsAny<Category>()), Times.Exactly(2));
            _mockValidationService.Verify(x => x.ValidateCategory(It.IsAny<CategoryFromApiDto>()), Times.Exactly(3));
        }

        [Fact]
        public async Task SyncCategories_WithEmptyCollection_ShouldCompleteSuccessfully()
        {
            // Arrange
            var categories = new List<CategoryFromApiDto>();

            // Act
            await _categoryService.SyncCategories(categories);

            // Assert
            _mockCategoryRepository.Verify(x => x.InsertCategory(It.IsAny<Category>()), Times.Never);
            _mockCategoryRepository.Verify(x => x.UpdateCategory(It.IsAny<Category>(), It.IsAny<int>()), Times.Never);
            _mockValidationService.Verify(x => x.ValidateCategory(It.IsAny<CategoryFromApiDto>()), Times.Never);
        }

        #endregion

        #region Helper Methods

        private void SetupMocks(CategoryFromApiDto categoryDto, Category categoryModel, ValidationResult validationResult, bool isValid)
        {
            _mockValidationService.Setup(x => x.ValidateCategory(categoryDto)).Returns(validationResult);
            
            var errorList = new List<string>();
            _mockValidationService.Setup(x => x.IsValid<CategoryFromApiDto>(validationResult, out errorList))
                .Returns(isValid);
            
            _mockCategoryMapper.Setup(x => x.MapToCategory(categoryDto)).Returns(categoryModel);
        }

        #endregion
    }
}