using Microsoft.EntityFrameworkCore;
using priceNegotiationAPI.Data;
using priceNegotiationAPI.Models;

namespace priceNegotiationAPI.Repositories
{
    public class NegotiationsRepository : Repository<Negotiation>, INegotiationsRepository
    {
        public NegotiationsRepository(ApplicationDbContext context, ILogger logger)
            : base(context, logger) { }

        public async Task<bool> HandleNegotiation(Negotiation negotiation, bool accepted)
        {
            try
            {
                negotiation.Accepted = accepted;
                negotiation.WasHandled = true;
                ApplicationDbContext.Negotiations.Update(negotiation);
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return false;
            }
        }

        public override async Task<Negotiation?> GetById(int id)
        {
            try
            {
                return await ApplicationDbContext.Negotiations.AsNoTracking().FirstOrDefaultAsync(n => n.Id == id);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public ApplicationDbContext ApplicationDbContext
        {
            get { return _context as ApplicationDbContext; }
        }
    }
}
