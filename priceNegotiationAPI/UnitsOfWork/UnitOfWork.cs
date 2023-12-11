using priceNegotiationAPI.Data;
using priceNegotiationAPI.Repositories;

namespace priceNegotiationAPI.UnitsOfWork
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;
        public IProductsRepository Products { get; private set; }
        public INegotiationsRepository Negotiations { get; private set; }

        public UnitOfWork(ApplicationDbContext context, ILoggerFactory loggerFactory)
        {
            _context = context;
            _logger = loggerFactory.CreateLogger("log/repositoryLog");
            Products = new ProductsRepository(context, _logger);
            Negotiations = new NegotiationsRepository(context, _logger);
        }

        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
