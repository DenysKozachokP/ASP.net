using Microsoft.EntityFrameworkCore;
using CharityHub.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddDbContext<CharityDbContext>(opts =>
{
    opts.UseSqlServer(builder.Configuration["ConnectionStrings:CharityHubConnection"]);
});

builder.Services.AddScoped<ICharityRepository, EFCharityRepository>();

var app = builder.Build();

app.UseStaticFiles();

app.UseSession();

app.MapDefaultControllerRoute();

// Заповнюємо базу початковими даними
SeedData.EnsurePopulated(app);

app.Run();