using MediatR;
using priceNegotiationAPI.Models;
using priceNegotiationAPI.Models.Dto;

namespace priceNegotiationAPI.Commands
{
    public class CreateProductRequest : IRequest<Product>
    {
        public ProductDTO ProductDTO { get; }

        public CreateProductRequest(ProductDTO productDTO)
        {
            ProductDTO = productDTO;
        }
    }
}
