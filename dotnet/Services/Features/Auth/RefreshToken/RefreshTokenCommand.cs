using MediatR;

namespace Services.Features.Auth.RefreshToken
{
    public class RefreshTokenCommand : IRequest<AuthDto>
    {
        public string RefreshToken { get; }

        public RefreshTokenCommand(string refreshToken)
        {
            RefreshToken = refreshToken;
        }
    }
}
