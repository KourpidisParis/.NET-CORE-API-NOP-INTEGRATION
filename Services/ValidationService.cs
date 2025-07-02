using FluentValidation;
using FluentValidation.Results;
using ErpConnector.DTOs;
using ErpConnector.Services.IServices;
using Microsoft.Extensions.Logging;

namespace ErpConnector.Services
{
    public class ValidationService : IValidationService
    {
        private readonly IValidator<CategoryFromApiDto> _categoryValidator;
        private readonly IValidator<ProductFromApiDto> _productValidator;
        private readonly ILogger<ValidationService> _logger;

        public ValidationService(
            IValidator<CategoryFromApiDto> categoryValidator,
            IValidator<ProductFromApiDto> productValidator,
            ILogger<ValidationService> logger)
        {
            _categoryValidator = categoryValidator;
            _productValidator = productValidator;
            _logger = logger;
        }

        public ValidationResult ValidateCategory(CategoryFromApiDto category)
        {
            if (category == null)
            {
                _logger.LogWarning("Category validation failed: Category is null");
                var result = new ValidationResult();
                result.Errors.Add(new ValidationFailure("Category", "Category cannot be null"));
                return result;
            }

            var validationResult = _categoryValidator.Validate(category);
            
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Category validation failed for category: {CategoryName}. Errors: {Errors}", 
                    category.Name, string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
            }

            return validationResult;
        }

        public ValidationResult ValidateProduct(ProductFromApiDto product)
        {
            if (product == null)
            {
                _logger.LogWarning("Product validation failed: Product is null");
                var result = new ValidationResult();
                result.Errors.Add(new ValidationFailure("Product", "Product cannot be null"));
                return result;
            }

            var validationResult = _productValidator.Validate(product);
            
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Product validation failed for product: {ProductTitle} (ID: {ProductId}). Errors: {Errors}", 
                    product.Title, product.Id, string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
            }

            return validationResult;
        }

        public bool IsValid<T>(ValidationResult result, out List<string> errors)
        {
            errors = new List<string>();
            
            if (result.IsValid)
            {
                return true;
            }

            errors = result.Errors.Select(e => e.ErrorMessage).ToList();
            return false;
        }
    }
}
