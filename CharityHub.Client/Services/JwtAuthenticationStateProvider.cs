using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace CharityHub.Client.Services
{
    public class JwtAuthenticationStateProvider : AuthenticationStateProvider, IDisposable
    {
        private readonly TokenProvider _tokenProvider;

        public JwtAuthenticationStateProvider(TokenProvider tokenProvider)
        {
            _tokenProvider = tokenProvider;
            _tokenProvider.TokenChanged += OnTokenChanged;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            await _tokenProvider.EnsureInitializedAsync();

            if (!_tokenProvider.HasToken || string.IsNullOrWhiteSpace(_tokenProvider.Token))
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            var identity = new ClaimsIdentity(ParseClaims(_tokenProvider.Token), "jwt");
            return new AuthenticationState(new ClaimsPrincipal(identity));
        }

        private static IEnumerable<Claim> ParseClaims(string jwt)
        {
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwt);
            return token.Claims;
        }

        private void OnTokenChanged()
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public void Dispose()
        {
            _tokenProvider.TokenChanged -= OnTokenChanged;
        }
    }
}

