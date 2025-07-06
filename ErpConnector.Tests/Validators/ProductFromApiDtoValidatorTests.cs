using FluentValidation.TestHelper;
using ErpConnector.DTOs;
using ErpConnector.Validators;
using Xunit;

namespace ErpConnector.Tests.Validators
{
    public class ProductFromApiDtoValidatorTests
    {
        private readonly ProductFromApiDtoValidator _validator;

        public ProductFromApiDtoValidatorTests()
        {
            _validator = new ProductFromApiDtoValidator();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-100)]
        [InlineData(-999)]
        public void Should_Have_Error_When_Id_Is_Not_Greater_Than_Zero(int invalidId)
        {
            // Arrange
            var model = new ProductFromApiDto 
            { 
                Id = invalidId,
                Title = "Valid Title",
                Price = 10.50m,
                Category = "Valid Category"
            };

            // Act & Assert
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Id)
                  .WithErrorMessage("Product ID must be greater than 0");
        }

        [Theory]
        [InlineData(1)]
        [InlineData(100)]
        [InlineData(999999)]
        public void Should_Not_Have_Error_When_Id_Is_Greater_Than_Zero(int validId)
        {
            // Arrange
            var model = new ProductFromApiDto 
            { 
                Id = validId,
                Title = "Valid Title",
                Price = 10.50m,
                Category = "Valid Category"
            };

            // Act & Assert
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.Id);
        }

        [Fact]
        public void Should_Have_Error_When_Title_Is_Empty()
        {
            // Arrange
            var model = new ProductFromApiDto 
            { 
                Id = 1,
                Title = "",
                Price = 10.50m,
                Category = "Valid Category"
            };

            // Act & Assert
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Title)
                  .WithErrorMessage("Product title is required");
        }

        [Fact]
        public void Should_Have_Error_When_Title_Is_Null()
        {
            // Arrange
            var model = new ProductFromApiDto 
            { 
                Id = 1,
                Title = null!,
                Price = 10.50m,
                Category = "Valid Category"
            };

            // Act & Assert
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Title)
                  .WithErrorMessage("Product title is required");
        }

        [Fact]
        public void Should_Have_Error_When_Title_Is_Too_Long()
        {
            // Arrange
            var model = new ProductFromApiDto 
            { 
                Id = 1,
                Title = new string('A', 256), // 256 characters
                Price = 10.50m,
                Category = "Valid Category"
            };

            // Act & Assert
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Title)
                  .WithErrorMessage("Product title must be between 1 and 255 characters");
        }

        [Theory]
        [InlineData("A")]
        [InlineData("Valid Product Title")]
        [InlineData("Product With Numbers 123")]
        [InlineData("Very Long Product Title That Is Still Valid And Contains Many Words")]
        public void Should_Not_Have_Error_When_Title_Is_Valid(string validTitle)
        {
            // Arrange
            var model = new ProductFromApiDto 
            { 
                Id = 1,
                Title = validTitle,
                Price = 10.50m,
                Category = "Valid Category"
            };

            // Act & Assert
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.Title);
        }

        [Fact]
        public void Should_Not_Have_Error_When_Title_Is_Maximum_Length()
        {
            // Arrange
            var model = new ProductFromApiDto 
            { 
                Id = 1,
                Title = new string('A', 255), // Exactly 255 characters
                Price = 10.50m,
                Category = "Valid Category"
            };

            // Act & Assert
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.Title);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(-0.01)]
        [InlineData(-100.50)]
        [InlineData(-999.99)]
        public void Should_Have_Error_When_Price_Is_Negative(decimal invalidPrice)
        {
            // Arrange
            var model = new ProductFromApiDto 
            { 
                Id = 1,
                Title = "Valid Title",
                Price = invalidPrice,
                Category = "Valid Category"
            };

            // Act & Assert
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Price)
                  .WithErrorMessage("Product price must be greater than or equal to 0");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(0.01)]
        [InlineData(10.50)]
        [InlineData(999.99)]
        [InlineData(9999.99)]
        public void Should_Not_Have_Error_When_Price_Is_Valid(decimal validPrice)
        {
            // Arrange
            var model = new ProductFromApiDto 
            { 
                Id = 1,
                Title = "Valid Title",
                Price = validPrice,
                Category = "Valid Category"
            };

            // Act & Assert
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.Price);
        }

        [Fact]
        public void Should_Have_Error_When_Description_Is_Too_Long()
        {
            // Arrange
            var model = new ProductFromApiDto 
            { 
                Id = 1,
                Title = "Valid Title",
                Price = 10.50m,
                Description = new string('A', 1001), // 1001 characters
                Category = "Valid Category"
            };

            // Act & Assert
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Description)
                  .WithErrorMessage("Product description must not exceed 1000 characters");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("Short description")]
        [InlineData("A very detailed product description that contains multiple sentences and provides comprehensive information about the product.")]
        public void Should_Not_Have_Error_When_Description_Is_Valid(string? validDescription)
        {
            // Arrange
            var model = new ProductFromApiDto 
            { 
                Id = 1,
                Title = "Valid Title",
                Price = 10.50m,
                Description = validDescription,
                Category = "Valid Category"
            };

            // Act & Assert
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.Description);
        }

        [Fact]
        public void Should_Not_Have_Error_When_Description_Is_Maximum_Length()
        {
            // Arrange
            var model = new ProductFromApiDto 
            { 
                Id = 1,
                Title = "Valid Title",
                Price = 10.50m,
                Description = new string('A', 1000), // Exactly 1000 characters
                Category = "Valid Category"
            };

            // Act & Assert
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.Description);
        }

        [Fact]
        public void Should_Have_Error_When_Category_Is_Empty()
        {
            // Arrange
            var model = new ProductFromApiDto 
            { 
                Id = 1,
                Title = "Valid Title",
                Price = 10.50m,
                Category = ""
            };

            // Act & Assert
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Category)
                  .WithErrorMessage("Product category is required");
        }

        [Fact]
        public void Should_Have_Error_When_Category_Is_Null()
        {
            // Arrange
            var model = new ProductFromApiDto 
            { 
                Id = 1,
                Title = "Valid Title",
                Price = 10.50m,
                Category = null!
            };

            // Act & Assert
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Category)
                  .WithErrorMessage("Product category is required");
        }

        [Fact]
        public void Should_Have_Error_When_Category_Is_Too_Long()
        {
            // Arrange
            var model = new ProductFromApiDto 
            { 
                Id = 1,
                Title = "Valid Title",
                Price = 10.50m,
                Category = new string('A', 101) // 101 characters
            };

            // Act & Assert
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Category)
                  .WithErrorMessage("Product category must be between 1 and 100 characters");
        }

        [Theory]
        [InlineData("A")]
        [InlineData("Electronics")]
        [InlineData("Books & Literature")]
        [InlineData("Home & Garden")]
        [InlineData("Sports & Outdoors")]
        public void Should_Not_Have_Error_When_Category_Is_Valid(string validCategory)
        {
            // Arrange
            var model = new ProductFromApiDto 
            { 
                Id = 1,
                Title = "Valid Title",
                Price = 10.50m,
                Category = validCategory
            };

            // Act & Assert
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.Category);
        }

        [Fact]
        public void Should_Not_Have_Error_When_Category_Is_Maximum_Length()
        {
            // Arrange
            var model = new ProductFromApiDto 
            { 
                Id = 1,
                Title = "Valid Title",
                Price = 10.50m,
                Category = new string('A', 100) // Exactly 100 characters
            };

            // Act & Assert
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.Category);
        }

        [Fact]
        public void Should_Not_Have_Error_When_Model_Is_Valid()
        {
            // Arrange
            var model = new ProductFromApiDto 
            { 
                Id = 1,
                Title = "Valid Product Title",
                Price = 99.99m,
                Description = "Valid product description",
                Category = "Valid Category"
            };

            // Act & Assert
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Should_Not_Have_Error_When_Model_Is_Valid_With_Null_Description()
        {
            // Arrange
            var model = new ProductFromApiDto 
            { 
                Id = 1,
                Title = "Valid Product Title",
                Price = 0m, // Zero price should be valid
                Description = null,
                Category = "Valid Category"
            };

            // Act & Assert
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Should_Not_Have_Error_When_Model_Is_Valid_With_Empty_Description()
        {
            // Arrange
            var model = new ProductFromApiDto 
            { 
                Id = 999,
                Title = "Another Valid Product Title",
                Price = 1234.56m,
                Description = "",
                Category = "Another Valid Category"
            };

            // Act & Assert
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Should_Have_Multiple_Errors_When_Multiple_Fields_Are_Invalid()
        {
            // Arrange
            var model = new ProductFromApiDto 
            { 
                Id = 0, // Invalid
                Title = "", // Invalid
                Price = -10.50m, // Invalid
                Description = new string('A', 1001), // Invalid
                Category = "" // Invalid
            };

            // Act & Assert
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Id);
            result.ShouldHaveValidationErrorFor(x => x.Title);
            result.ShouldHaveValidationErrorFor(x => x.Price);
            result.ShouldHaveValidationErrorFor(x => x.Description);
            result.ShouldHaveValidationErrorFor(x => x.Category);
        }
    }
}
