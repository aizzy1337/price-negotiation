using MediatR;
using priceNegotiationAPI.Commands;
using priceNegotiationAPI.Models.Dto;
using priceNegotiationAPI.Models;
using priceNegotiationAPI.UnitsOfWork;

namespace priceNegotiationAPI.Handlers
{
    public class UpdateProductHandler : IRequestHandler<UpdateProductRequest, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;

        public UpdateProductHandler(IUnitOfWork unitOfWork, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger("productsLogger");
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(UpdateProductRequest request, CancellationToken cancellationToken)
        {
            var productDTO = request.ProductDTO;

            if (productDTO == null)
            {
                _logger.LogError("Recievied object is null");
                return false;
            }

            var product = await _unitOfWork.Products.GetById(productDTO.Id);
            if (product == null)
            {
                _logger.LogError("Object of given index was not found");
                return false;
            }

            if (productDTO.Name == "" || productDTO.Price <= 0.0)
            {
                _logger.LogError("Object has no name or price is lower then 0");
                return false;
            }

            Product model = new Product()
            {
                Id = productDTO.Id,
                Name = productDTO.Name,
                Description = productDTO.Description,
                Price = productDTO.Price,
                CreatedDate = DateTime.Now
            };

            await _unitOfWork.Products.Update(model);
            await _unitOfWork.CompleteAsync();
            return true;
        }
    }
}
