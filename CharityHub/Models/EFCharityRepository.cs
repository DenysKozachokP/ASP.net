namespace CharityHub.Models
{
    public class EFCharityRepository : ICharityRepository
    {
        private CharityDbContext context;

        public EFCharityRepository(CharityDbContext ctx)
        {
            context = ctx;
        }

        public IQueryable<Event> Events => context.Events;
    }
}
