using MediatR;
using priceNegotiationAPI.Models;

namespace priceNegotiationAPI.Queries
{
    public class GetAllProductsQuery : IRequest<IEnumerable<Product>>
    {

    }
}
