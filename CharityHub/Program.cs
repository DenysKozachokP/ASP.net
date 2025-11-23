using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
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
    opts.UseSqlite(builder.Configuration["ConnectionStrings:CharityHubConnection"]);
});

builder.Services.AddScoped<ICharityRepository, EFCharityRepository>();

builder.Services.AddDbContext<AppIdentityDbContext>(opts => 
    opts.UseSqlite(builder.Configuration["ConnectionStrings:IdentityConnection"]));

builder.Services.AddIdentity<AppUser, IdentityRole>(opts =>
{
    // Налаштування пароля
    opts.Password.RequiredLength = 8;
    opts.Password.RequireDigit = true;
    opts.Password.RequireLowercase = true;
    opts.Password.RequireUppercase = true;
    opts.Password.RequireNonAlphanumeric = false;
    
    // Налаштування користувача
    opts.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<AppIdentityDbContext>()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(opts =>
{
    opts.LoginPath = "/Account/Login";
    opts.AccessDeniedPath = "/Account/AccessDenied";
});

var app = builder.Build();

app.UseStaticFiles();

app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapDefaultControllerRoute();

// Заповнюємо базу початковими даними
SeedData.EnsurePopulated(app);

// Ініціалізуємо ролі та адміністратора
await IdentitySeedData.EnsurePopulated(app);

await app.RunAsync();