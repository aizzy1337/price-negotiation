using priceNegotiationAPI.Models;

namespace priceNegotiationAPI.Repositories
{
    public interface INegotiationsRepository : IRepository<Negotiation>
    {
        Task<bool> HandleNegotiation(Negotiation negotiation, bool accepted);
    }
}
