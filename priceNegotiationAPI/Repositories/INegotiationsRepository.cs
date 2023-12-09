using priceNegotiationAPI.Models;

namespace priceNegotiationAPI.Repositories
{
    public interface INegotiationsRepository : IRepository<Negotiation>
    {
        void HandleNegotiation(int id, bool accepted);
    }
}
