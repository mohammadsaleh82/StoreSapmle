using FluentValidation;
namespace Application.Dtos.Auth;

public class LoginValidator:AbstractValidator<LoginDto>
{
    public LoginValidator()
    {
        RuleFor(x=>x.Username).NotEmpty().WithMessage("نام کاربری را وارد کنید").Length(1,50).WithMessage("نام کاربری نباید بیشتر از 50 کاراکتر باشد");
        RuleFor(x=>x.Password).NotEmpty().WithMessage("رمز عبور را وارد کنید").Length(1,50).WithMessage("رمز عبور نباید بیشتر از 50 کاراکتر باشد");
    }
}