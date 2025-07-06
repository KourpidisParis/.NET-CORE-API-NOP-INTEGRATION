using FluentValidation.TestHelper;
using ErpConnector.DTOs;
using ErpConnector.Validators;
using Xunit;

namespace ErpConnector.Tests.Validators
{
    public class CategoryFromApiDtoValidatorTests
    {
        private readonly CategoryFromApiDtoValidator _validator;

        public CategoryFromApiDtoValidatorTests()
        {
            _validator = new CategoryFromApiDtoValidator();
        }

        [Fact]
        public void Should_Have_Error_When_Name_Is_Empty()
        {
            // Arrange
            var model = new CategoryFromApiDto { Name = "", Slug = "valid-slug" };

            // Act & Assert
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Name)
                  .WithErrorMessage("Category name is required");
        }

        [Fact]
        public void Should_Have_Error_When_Name_Is_Null()
        {
            // Arrange
            var model = new CategoryFromApiDto { Name = null!, Slug = "valid-slug" };
            
            // Act & Assert
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Name)
                  .WithErrorMessage("Category name is required");
        }

        [Fact]
        public void Should_Have_Error_When_Name_Is_Too_Long()
        {
            // Arrange
            var model = new CategoryFromApiDto 
            { 
                Name = new string('A', 256), // 256 characters
                Slug = "valid-slug" 
            };

            // Act & Assert
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Name)
                  .WithErrorMessage("Category name must be between 1 and 255 characters");
        }

        [Fact]
        public void Should_Have_Error_When_Slug_Is_Empty()
        {
            // Arrange
            var model = new CategoryFromApiDto { Name = "Valid Name", Slug = "" };

            // Act & Assert
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Slug)
                  .WithErrorMessage("Category slug is required");
        }

        [Fact]
        public void Should_Have_Error_When_Slug_Is_Null()
        {
            // Arrange
            var model = new CategoryFromApiDto { Name = "Valid Name", Slug = null! };

            // Act & Assert
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Slug)
                  .WithErrorMessage("Category slug is required");
        }

        [Fact]
        public void Should_Have_Error_When_Slug_Is_Too_Long()
        {
            // Arrange
            var model = new CategoryFromApiDto 
            { 
                Name = "Valid Name", 
                Slug = new string('a', 256) // 256 characters
            };

            // Act & Assert
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Slug)
                  .WithErrorMessage("Category slug must be between 1 and 255 characters");
        }

        [Theory]
        [InlineData("INVALID-SLUG")]
        [InlineData("invalid_slug")]
        [InlineData("invalid slug")]
        [InlineData("invalid@slug")]
        [InlineData("invalid.slug")]
        [InlineData("invalid/slug")]
        [InlineData("invalid\\slug")]
        [InlineData("invalid#slug")]
        public void Should_Have_Error_When_Slug_Has_Invalid_Format(string invalidSlug)
        {
            // Arrange
            var model = new CategoryFromApiDto { Name = "Valid Name", Slug = invalidSlug };

            // Act & Assert
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Slug)
                  .WithErrorMessage("Category slug can only contain lowercase letters, numbers, and hyphens");
        }

        [Theory]
        [InlineData("valid-slug")]
        [InlineData("valid123")]
        [InlineData("valid-slug-123")]
        [InlineData("a")]
        [InlineData("1")]
        [InlineData("a-1")]
        [InlineData("test-category-name")]
        public void Should_Not_Have_Error_When_Slug_Is_Valid(string validSlug)
        {
            // Arrange
            var model = new CategoryFromApiDto { Name = "Valid Name", Slug = validSlug };

            // Act & Assert
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.Slug);
        }

        [Theory]
        [InlineData("A")]
        [InlineData("Valid Name")]
        [InlineData("Category With Numbers 123")]
        [InlineData("Very Long Category Name That Is Still Valid")]
        public void Should_Not_Have_Error_When_Name_Is_Valid(string validName)
        {
            // Arrange
            var model = new CategoryFromApiDto { Name = validName, Slug = "valid-slug" };

            // Act & Assert
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Should_Not_Have_Error_When_Model_Is_Valid()
        {
            // Arrange
            var model = new CategoryFromApiDto 
            { 
                Name = "Valid Category Name", 
                Slug = "valid-category-slug" 
            };

            // Act & Assert
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Should_Not_Have_Error_When_Name_Is_Maximum_Length()
        {
            // Arrange
            var model = new CategoryFromApiDto 
            { 
                Name = new string('A', 255), // Exactly 255 characters
                Slug = "valid-slug" 
            };

            // Act & Assert
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Should_Not_Have_Error_When_Slug_Is_Maximum_Length()
        {
            // Arrange
            var model = new CategoryFromApiDto 
            { 
                Name = "Valid Name", 
                Slug = new string('a', 255) // Exactly 255 characters
            };

            // Act & Assert
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.Slug);
        }
    }
}
