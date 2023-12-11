using Microsoft.EntityFrameworkCore;
using priceNegotiationAPI.Data;
using priceNegotiationAPI.Models;

namespace priceNegotiationAPI.Repositories
{
    public class ProductsRepository : Repository<Product>, IProductsRepository
    {
        public ProductsRepository(ApplicationDbContext context, ILogger logger)
            : base(context, logger) { }

        public override async Task<Product?> GetById(int id)
        {
            try
            {
                return await ApplicationDbContext.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
            }
            catch(Exception e)
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
