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

            context.Database.EnsureCreated();

            if (!context.Events.Any())
            {
                var event1 = new Event
                {
                    Title = "Збір на дрони для фронту",
                    Description = "Волонтерська ініціатива для підтримки військових.",
                    Date = DateTime.Now.AddDays(7),
                    Location = "Київ",
                    TargetAmount = 250000
                };

                var event2 = new Event
                {
                    Title = "Благодійний концерт",
                    Description = "Усі кошти підуть на допомогу переселенцям.",
                    Date = DateTime.Now.AddDays(14),
                    Location = "Львів",
                    TargetAmount = 50000
                };

                var event3 = new Event
                {
                    Title = "Збір на генератори",
                    Description = "Закупівля генераторів для лікарень.",
                    Date = DateTime.Now.AddDays(21),
                    Location = "Харків",
                    TargetAmount = 100000
                };

                context.Events.AddRange(event1, event2, event3);
                context.SaveChanges();

                // Додаємо тестові пожертви для демонстрації зв'язку один-до-багатьох
                if (!context.Donations.Any())
                {
                    context.Donations.AddRange(
                        new Donation
                        {
                            DonorName = "Іван Петренко",
                            Amount = 5000,
                            DonationDate = DateTime.Now.AddDays(-2),
                            Comment = "Підтримую наших захисників!",
                            EventId = event1.EventId
                        },
                        new Donation
                        {
                            DonorName = "Марія Коваленко",
                            Amount = 10000,
                            DonationDate = DateTime.Now.AddDays(-1),
                            EventId = event1.EventId
                        },
                        new Donation
                        {
                            DonorName = "Олександр Шевченко",
                            Amount = 2500,
                            DonationDate = DateTime.Now,
                            Comment = "Невеликий внесок, але від щирого серця",
                            EventId = event1.EventId
                        },
                        new Donation
                        {
                            DonorName = "Анна Мельник",
                            Amount = 3000,
                            DonationDate = DateTime.Now.AddDays(-3),
                            EventId = event2.EventId
                        },
                        new Donation
                        {
                            DonorName = "Петро Бондаренко",
                            Amount = 5000,
                            DonationDate = DateTime.Now.AddDays(-1),
                            EventId = event2.EventId
                        },
                        new Donation
                        {
                            DonorName = "Тетяна Лисенко",
                            Amount = 15000,
                            DonationDate = DateTime.Now.AddDays(-5),
                            Comment = "Допомога переселенцям - важлива справа",
                            EventId = event2.EventId
                        },
                        new Donation
                        {
                            DonorName = "Володимир Гриценко",
                            Amount = 8000,
                            DonationDate = DateTime.Now.AddDays(-4),
                            EventId = event3.EventId
                        },
                        new Donation
                        {
                            DonorName = "Наталія Романенко",
                            Amount = 12000,
                            DonationDate = DateTime.Now.AddDays(-2),
                            Comment = "Лікарні потребують генераторів",
                            EventId = event3.EventId
                        }
                    );
                    context.SaveChanges();
                }
            }
        }
    }
}
