

namespace Application.Dtos.Auth;

public record LoginDto(string Username, string Password, bool RememberMe);