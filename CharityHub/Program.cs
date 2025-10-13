using Microsoft.EntityFrameworkCore;
using CharityHub.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<CharityDbContext>(opts =>
{
    opts.UseSqlServer(builder.Configuration["ConnectionStrings:CharityHubConnection"]);
});

builder.Services.AddScoped<ICharityRepository, EFCharityRepository>();

var app = builder.Build();

app.UseStaticFiles();

app.MapDefaultControllerRoute();

// Заповнюємо базу початковими даними
SeedData.EnsurePopulated(app);

app.Run();