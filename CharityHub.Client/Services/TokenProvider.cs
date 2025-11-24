using Microsoft.JSInterop;

namespace CharityHub.Client.Services
{
    public class TokenProvider
    {
        private const string StorageKey = "charityhub-auth-token";
        private readonly IJSRuntime _jsRuntime;
        private string? _token;
        private bool _isInitialized;

        public TokenProvider(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public string? Token => _token;

        public event Action? TokenChanged;

        public async Task EnsureInitializedAsync()
        {
            if (_isInitialized)
            {
                return;
            }

            _token = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", StorageKey);
            _isInitialized = true;
        }

        public async Task SetTokenAsync(string? token)
        {
            await EnsureInitializedAsync();

            _token = token;
            if (string.IsNullOrWhiteSpace(token))
            {
                await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", StorageKey);
            }
            else
            {
                await _jsRuntime.InvokeVoidAsync("localStorage.setItem", StorageKey, token);
            }

            TokenChanged?.Invoke();
        }

        public bool HasToken => !string.IsNullOrWhiteSpace(_token);
    }
}

