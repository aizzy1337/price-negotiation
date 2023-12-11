using MediatR;
using priceNegotiationAPI.Models;
using priceNegotiationAPI.Queries;
using priceNegotiationAPI.UnitsOfWork;

namespace priceNegotiationAPI.Handlers
{
    public class GetNegotiationHandler : IRequestHandler<GetNegotiationQuery, Negotiation>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;

        public GetNegotiationHandler(IUnitOfWork unitOfWork, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger("negotiationsLogger");
            _unitOfWork = unitOfWork;
        }

        public async Task<Negotiation> Handle(GetNegotiationQuery request, CancellationToken cancellationToken)
        {
            var negotiation = await _unitOfWork.Negotiations.GetById(request.NegotiationId);

            if (negotiation == null)
            {
                _logger.LogError("Object of given index was not found");
                return null;
            }

            return negotiation;
        }
    }
}
