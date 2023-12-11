using MediatR;
using priceNegotiationAPI.Commands;
using priceNegotiationAPI.UnitsOfWork;

namespace priceNegotiationAPI.Handlers
{
    public class DeleteNegotiationHandler : IRequestHandler<DeleteNegotiationRequest, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;

        public DeleteNegotiationHandler(IUnitOfWork unitOfWork, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger("negotiationsLogger");
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteNegotiationRequest request, CancellationToken cancellationToken)
        {
            if (request.NegotiationId < 1)
            {
                _logger.LogError("Index 0 and negative is not acceptable");
                return false;
            }

            var negotiation = await _unitOfWork.Negotiations.GetById(request.NegotiationId);
            if (negotiation == null)
            {
                _logger.LogError("Object of given index was not found");
                return false;
            }

            await _unitOfWork.Negotiations.Remove(negotiation);
            await _unitOfWork.CompleteAsync();

            return true;
        }
    }
}
