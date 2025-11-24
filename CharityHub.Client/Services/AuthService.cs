using System.Net.Http.Json;
using CharityHub.Client.Models;

namespace CharityHub.Client.Services
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;
        private readonly TokenProvider _tokenProvider;

        public AuthService(HttpClient httpClient, TokenProvider tokenProvider)
        {
            _httpClient = httpClient;
            _tokenProvider = tokenProvider;
        }

        public async Task<(bool Success, string? Error)> LoginAsync(LoginRequest request)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/auth/login", request);
                if (!response.IsSuccessStatusCode)
                {
                    return (false, await BuildErrorMessage(response, "Помилка входу"));
                }

                var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
                if (result is null || string.IsNullOrWhiteSpace(result.Token))
                {
                    return (false, "Не вдалося отримати токен аутентифікації");
                }

                await _tokenProvider.SetTokenAsync(result.Token);
                return (true, null);
            }
            catch (HttpRequestException)
            {
                return (false, "Сервіс аутентифікації недоступний");
            }
        }

        public async Task<(bool Success, string? Error)> RegisterAsync(RegisterRequest request)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/auth/register", request);
                if (!response.IsSuccessStatusCode)
                {
                    return (false, await BuildErrorMessage(response, "Помилка реєстрації"));
                }

                var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
                if (result is null || string.IsNullOrWhiteSpace(result.Token))
                {
                    return (false, "Не вдалося отримати токен аутентифікації");
                }

                await _tokenProvider.SetTokenAsync(result.Token);
                return (true, null);
            }
            catch (HttpRequestException)
            {
                return (false, "Сервіс реєстрації недоступний");
            }
        }

        public async Task LogoutAsync()
        {
            await _tokenProvider.SetTokenAsync(null);
        }

        private static async Task<string> BuildErrorMessage(HttpResponseMessage response, string fallback)
        {
            string? body = null;
            try
            {
                body = await response.Content.ReadAsStringAsync();
            }
            catch
            {
                // ignore
            }

            var statusInfo = $"{fallback} (HTTP {(int)response.StatusCode})";
            if (string.IsNullOrWhiteSpace(body))
            {
                return statusInfo;
            }

            return $"{statusInfo}: {body}";
        }
    }
}

