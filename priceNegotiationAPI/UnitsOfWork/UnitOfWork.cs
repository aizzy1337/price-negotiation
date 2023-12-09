using priceNegotiationAPI.Data;
using priceNegotiationAPI.Repositories;

namespace priceNegotiationAPI.UnitsOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext context;

        public UnitOfWork(ApplicationDbContext context)
        {
            this.context = context;
            Products = new ProductsRepository(context);
            Negotiations = new NegotiationsRepository(context);
        }

        public IProductsRepository Products { get; set; }
        public INegotiationsRepository Negotiations { get; set; }

        public int Complete()
        {
            return context.SaveChanges();
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
