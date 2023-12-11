using MediatR;
using priceNegotiationAPI.Models.Dto;

namespace priceNegotiationAPI.Commands
{
    public class UpdateProductRequest : IRequest<bool>
    {
        public ProductDTO ProductDTO { get; }

        public UpdateProductRequest(ProductDTO productDTO)
        {
            ProductDTO = productDTO;
        }
    }
}
