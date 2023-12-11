using MediatR;
using priceNegotiationAPI.Models;
using priceNegotiationAPI.Queries;
using priceNegotiationAPI.UnitsOfWork;

namespace priceNegotiationAPI.Handlers
{
    public class GetProductHandler : IRequestHandler<GetProductQuery, Product>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;

        public GetProductHandler(IUnitOfWork unitOfWork, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger("productsLogger");
            _unitOfWork = unitOfWork;
        }

        public async Task<Product> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            var product = await _unitOfWork.Products.GetById(request.ProductId);

            if (product == null)
            {
                _logger.LogError("Object of given index was not found");
                return null;
            }

            return product;
        }
    }
}
