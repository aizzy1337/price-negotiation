using System.Linq.Expressions;

namespace priceNegotiationAPI.Repositories
{
    public interface IRepository<TEnity> where TEnity : class
    {
        Task<TEnity?> GetById(int id);
        Task<IEnumerable<TEnity>> GetAll();

        Task<bool> Add(TEnity enity);
        Task<bool> Update(TEnity enity);
        Task<bool> Remove(TEnity enity);
    }
}
