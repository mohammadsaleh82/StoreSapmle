namespace Application.Dtos.Auth;

public record RegisterDto(
    string UserName,string Email, string Password, string ConfirmPassword);
