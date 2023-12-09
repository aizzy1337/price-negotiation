using priceNegotiationAPI.Data;
using priceNegotiationAPI.Models;

namespace priceNegotiationAPI.Repositories
{
    public class NegotiationsRepository : Repository<Negotiation>, INegotiationsRepository
    {
        public NegotiationsRepository(ApplicationDbContext context)
            : base(context) { }

        public void HandleNegotiation(int id, bool accepted)
        {
            Negotiation negotiationToHandle = context.Set<Negotiation>().Find(id);
            if (negotiationToHandle != null)
            {
                negotiationToHandle.Accepted = accepted;
                negotiationToHandle.WasHandled = true;
                context.Set<Negotiation>().Update(negotiationToHandle);
            }
        }

        public ApplicationDbContext ApplicationDbContext
        {
            get { return context as ApplicationDbContext; }
        }
    }
}
