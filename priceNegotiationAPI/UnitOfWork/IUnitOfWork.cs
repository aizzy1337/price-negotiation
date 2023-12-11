using priceNegotiationAPI.Repositories;

namespace priceNegotiationAPI.UnitsOfWork
{
    public interface IUnitOfWork
    {
        IProductsRepository Products { get; }
        INegotiationsRepository Negotiations { get; }
        Task CompleteAsync();
    }
}
