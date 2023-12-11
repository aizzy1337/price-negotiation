using MediatR;
using priceNegotiationAPI.Models.Dto;
using priceNegotiationAPI.Models;

namespace priceNegotiationAPI.Commands
{
    public class DeleteProductRequest : IRequest<bool>
    {
        public int ProductId { get; }

        public DeleteProductRequest(int productId)
        {
            ProductId = productId;
        }
    }
}
