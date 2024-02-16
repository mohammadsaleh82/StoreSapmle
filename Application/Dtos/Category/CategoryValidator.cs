using FluentValidation;

namespace Application.Dtos.Category;

public class CategoryValidator:AbstractValidator<CategoryDto>
{
    public CategoryValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}