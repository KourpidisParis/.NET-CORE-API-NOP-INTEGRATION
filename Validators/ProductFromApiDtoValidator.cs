using FluentValidation;
using ErpConnector.DTOs;

namespace ErpConnector.Validators
{
    public class ProductFromApiDtoValidator : AbstractValidator<ProductFromApiDto>
    {
        public ProductFromApiDtoValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Product ID must be greater than 0");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Product title is required")
                .Length(1, 255).WithMessage("Product title must be between 1 and 255 characters");

            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0).WithMessage("Product price must be greater than or equal to 0");

            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("Product description must not exceed 1000 characters");

            RuleFor(x => x.Category)
                .NotEmpty().WithMessage("Product category is required")
                .Length(1, 100).WithMessage("Product category must be between 1 and 100 characters");
        }
    }
}
