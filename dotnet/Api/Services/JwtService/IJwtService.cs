namespace Api.Services.JwtService
{
    public interface IJwtService
    {
        string GenerateToken(string secretKey);
        bool ValidateToken(string token, string secretKey);
    }
}
