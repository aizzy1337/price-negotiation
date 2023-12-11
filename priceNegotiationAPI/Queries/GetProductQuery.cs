using MediatR;
using priceNegotiationAPI.Models;

namespace priceNegotiationAPI.Queries
{
    public class GetProductQuery : IRequest<Product>
    {
        public int ProductId { get; }

        public GetProductQuery(int productId)
        {
            ProductId = productId;
        }
    }
}
