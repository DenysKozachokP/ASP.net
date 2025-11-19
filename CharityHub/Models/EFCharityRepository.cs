using Microsoft.EntityFrameworkCore;

namespace CharityHub.Models
{
    public class EFCharityRepository : ICharityRepository
    {
        private CharityDbContext context;

        public EFCharityRepository(CharityDbContext ctx)
        {
            context = ctx;
        }

        public IQueryable<Event> Events => context.Events.Include(e => e.Donations);
        public IQueryable<Donation> Donations => context.Donations.Include(d => d.Event);

        // CRUD операції для Event
        public void CreateEvent(Event e)
        {
            context.Add(e);
            context.SaveChanges();
        }

        public void UpdateEvent(Event e)
        {
            context.Update(e);
            context.SaveChanges();
        }

        public void DeleteEvent(Event e)
        {
            context.Remove(e);
            context.SaveChanges();
        }

        public Event? GetEventById(long? id)
        {
            return context.Events.Include(e => e.Donations).FirstOrDefault(e => e.EventId == id);
        }

        // CRUD операції для Donation
        public void CreateDonation(Donation d)
        {
            context.Add(d);
            context.SaveChanges();
        }

        public void UpdateDonation(Donation d)
        {
            context.Update(d);
            context.SaveChanges();
        }

        public void DeleteDonation(Donation d)
        {
            context.Remove(d);
            context.SaveChanges();
        }

        public Donation? GetDonationById(long? id)
        {
            return context.Donations.Include(d => d.Event).FirstOrDefault(d => d.DonationId == id);
        }
    }
}
