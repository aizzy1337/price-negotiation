using MediatR;
using priceNegotiationAPI.Commands;
using priceNegotiationAPI.Models;
using priceNegotiationAPI.Models.Dto;
using priceNegotiationAPI.UnitsOfWork;

namespace priceNegotiationAPI.Handlers
{
    public class CreateProductHandler : IRequestHandler<CreateProductRequest, Product>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;

        public CreateProductHandler(IUnitOfWork unitOfWork, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger("productsLogger");
            _unitOfWork = unitOfWork;
        }

        public async Task<Product> Handle(CreateProductRequest request, CancellationToken cancellationToken)
        {
            var productDTO = request.ProductDTO;

            if (productDTO == null)
            {
                _logger.LogError("Recievied object is null");
                return null;
            }

            if (productDTO.Id != 0)
            {
                _logger.LogError("Object has ID diffrent then 0");
                return null;
            }

            if (productDTO.Name == "" || productDTO.Price <= 0.0)
            {
                _logger.LogError("Object has no name or price is lower then 0");
                return null;
            }

            Product model = new Product()
            {
                Name = productDTO.Name,
                Description = productDTO.Description,
                Price = productDTO.Price,
                CreatedDate = DateTime.Now
            };

            await _unitOfWork.Products.Add(model);
            await _unitOfWork.CompleteAsync();
            return model;
        }
    }
}
