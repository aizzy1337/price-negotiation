using MediatR;
using priceNegotiationAPI.Commands;
using priceNegotiationAPI.Models;
using priceNegotiationAPI.UnitsOfWork;

namespace priceNegotiationAPI.Handlers
{
    public class DeleteProductHandler : IRequestHandler<DeleteProductRequest, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;

        public DeleteProductHandler(IUnitOfWork unitOfWork, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger("productsLogger");
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteProductRequest request, CancellationToken cancellationToken)
        {
            if (request.ProductId < 1)
            {
                _logger.LogError("Index 0 and negative is not acceptable");
                return false;
            }

            var product = await _unitOfWork.Products.GetById(request.ProductId);
            if (product == null)
            {
                _logger.LogError("Object of given index was not found");
                return false;
            }

            var negotiations = await _unitOfWork.Negotiations.GetAll();
            if (negotiations.Any(n => n.ProductId == product.Id))
            {
                _logger.LogError("Cannot delete object becouse there are connected other objects");
                return false;
            }

            await _unitOfWork.Products.Remove(product);
            await _unitOfWork.CompleteAsync();
            return true;
        }
    }
}
