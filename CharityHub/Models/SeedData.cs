using Microsoft.EntityFrameworkCore;

namespace CharityHub.Models
{
    public static class SeedData
    {
        public static void EnsurePopulated(IApplicationBuilder app)
        {
            CharityDbContext context = app.ApplicationServices
                .CreateScope().ServiceProvider
                .GetRequiredService<CharityDbContext>();

            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }

            if (!context.Events.Any())
            {
                context.Events.AddRange(
                    new Event
                    {
                        Title = "Збір на дрони для фронту",
                        Description = "Волонтерська ініціатива для підтримки військових.",
                        Date = DateTime.Now.AddDays(7),
                        Location = "Київ",
                        TargetAmount = 250000
                    },
                    new Event
                    {
                        Title = "Благодійний концерт",
                        Description = "Усі кошти підуть на допомогу переселенцям.",
                        Date = DateTime.Now.AddDays(14),
                        Location = "Львів",
                        TargetAmount = 50000
                    },
                    new Event
                    {
                        Title = "Збір на генератори",
                        Description = "Закупівля генераторів для лікарень.",
                        Date = DateTime.Now.AddDays(21),
                        Location = "Харків",
                        TargetAmount = 100000
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
