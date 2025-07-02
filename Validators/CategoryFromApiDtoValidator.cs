using FluentValidation;
using ErpConnector.DTOs;

namespace ErpConnector.Validators
{
    public class CategoryFromApiDtoValidator : AbstractValidator<CategoryFromApiDto>
    {
        public CategoryFromApiDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Category name is required")
                .Length(1, 255).WithMessage("Category name must be between 1 and 255 characters");

            RuleFor(x => x.Slug)
                .NotEmpty().WithMessage("Category slug is required")
                .Length(1, 255).WithMessage("Category slug must be between 1 and 255 characters")
                .Matches("^[a-z0-9-]+$").WithMessage("Category slug can only contain lowercase letters, numbers, and hyphens");
        }
    }
}
