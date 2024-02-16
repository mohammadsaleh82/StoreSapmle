using FluentValidation;

namespace Application.Dtos.Auth;

public class RegisterValidator : AbstractValidator<RegisterDto>
{
    public RegisterValidator()
    {
        RuleFor(x => x.UserName).NotEmpty().WithMessage("نام کاربری را وارد کنید").Length(1, 50)
            .WithMessage("ایمیل نباید بیشتر از 50 کاراکتر باشد");
        RuleFor(x => x.Email).NotEmpty().WithMessage("ایمیل را وارد کنید")
            .Length(1, 50).WithMessage("ایمیل نباید بیشتر از 50 کاراکتر باشد").EmailAddress()
            .WithMessage("ایمیل معتبر وارد کنید");
        RuleFor(x => x.Password).NotEmpty().WithMessage("رمز عبور را وارد کنید").Length(8, 50)
            .WithMessage("رمز عبور نباید کمتر از 8 و بیشتر از 50 کاراکتر باشد");
        RuleFor(x => x.ConfirmPassword).NotEmpty().WithMessage("تکرار رمز عبور را وارد کنید").Equal(x => x.Password)
            .WithMessage("رمز عبور و تکرار آن برابر نیست");
    }
}