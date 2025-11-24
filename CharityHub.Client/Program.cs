using CharityHub.Client;
using CharityHub.Client.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var apiBaseAddress = builder.Configuration["ApiBaseUrl"] ?? "https://localhost:7154/";

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(apiBaseAddress) });
builder.Services.AddOptions();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<TokenProvider>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<EventService>();
builder.Services.AddScoped<AuthenticationStateProvider, JwtAuthenticationStateProvider>();

await builder.Build().RunAsync();
