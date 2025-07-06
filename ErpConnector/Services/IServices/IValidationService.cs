using FluentValidation.Results;

namespace ErpConnector.Services.IServices
{
    public interface IValidationService
    {
        ValidationResult ValidateCategory(DTOs.CategoryFromApiDto category);
        ValidationResult ValidateProduct(DTOs.ProductFromApiDto product);
        bool IsValid<T>(ValidationResult result, out List<string> errors);
    }
}
