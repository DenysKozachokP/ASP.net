using System.Net.Http.Headers;
using System.Net.Http.Json;
using CharityHub.Client.Models;

namespace CharityHub.Client.Services
{
    public class EventService
    {
        private readonly HttpClient _httpClient;
        private readonly TokenProvider _tokenProvider;

        public EventService(HttpClient httpClient, TokenProvider tokenProvider)
        {
            _httpClient = httpClient;
            _tokenProvider = tokenProvider;
        }

        public async Task<List<EventDto>> GetEventsAsync()
        {
            await EnsureAuthorizedAsync();
            return await _httpClient.GetFromJsonAsync<List<EventDto>>("api/events") ?? new List<EventDto>();
        }

        public async Task<EventDto?> GetEventAsync(long id)
        {
            await EnsureAuthorizedAsync();
            return await _httpClient.GetFromJsonAsync<EventDto>($"api/events/{id}");
        }

        public async Task<(bool Success, string? Error)> CreateEventAsync(EventDto dto)
        {
            await EnsureAuthorizedAsync();
            var response = await _httpClient.PostAsJsonAsync("api/events", dto);
            return await BuildResult(response, "Не вдалося створити подію");
        }

        public async Task<(bool Success, string? Error)> UpdateEventAsync(long id, EventDto dto)
        {
            await EnsureAuthorizedAsync();
            var response = await _httpClient.PutAsJsonAsync($"api/events/{id}", dto);
            return await BuildResult(response, "Не вдалося оновити подію");
        }

        public async Task<(bool Success, string? Error)> DeleteEventAsync(long id)
        {
            await EnsureAuthorizedAsync();
            var response = await _httpClient.DeleteAsync($"api/events/{id}");
            return await BuildResult(response, "Не вдалося видалити подію");
        }

        private async Task EnsureAuthorizedAsync()
        {
            await _tokenProvider.EnsureInitializedAsync();

            if (_tokenProvider.HasToken)
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", _tokenProvider.Token);
                return;
            }

            _httpClient.DefaultRequestHeaders.Authorization = null;
            throw new InvalidOperationException("Користувача не авторизовано. Увійдіть, щоб продовжити.");
        }

        private static async Task<(bool Success, string? Error)> BuildResult(HttpResponseMessage response, string fallbackError)
        {
            if (response.IsSuccessStatusCode)
            {
                return (true, null);
            }

            string? apiError = null;
            try
            {
                apiError = await response.Content.ReadAsStringAsync();
            }
            catch
            {
                // ignore parsing errors
            }

            return (false, string.IsNullOrWhiteSpace(apiError) ? fallbackError : apiError);
        }
    }
}

