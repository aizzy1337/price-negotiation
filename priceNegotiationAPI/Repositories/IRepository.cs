using System.Linq.Expressions;

namespace priceNegotiationAPI.Repositories
{
    public interface IRepository<TEnity> where TEnity : class
    {
        Task<TEnity> GetById(int id);
        Task<IEnumerable<TEnity>> GetAll();
        Task<IEnumerable<TEnity>> Find(Expression<Func<TEnity, bool>> predicate);

        Task<bool> Add(TEnity enity);
        Task<bool> Update(TEnity enity);
        Task<bool> Remove(int id);
    }
}
