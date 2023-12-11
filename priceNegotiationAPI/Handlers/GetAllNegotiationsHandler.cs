using MediatR;
using priceNegotiationAPI.Models;
using priceNegotiationAPI.Queries;
using priceNegotiationAPI.UnitsOfWork;

namespace priceNegotiationAPI.Handlers
{
    public class GetAllNegotiationsHandler : IRequestHandler<GetAllNegotiationsQuery, IEnumerable<Negotiation>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;

        public GetAllNegotiationsHandler(IUnitOfWork unitOfWork, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger("negotiationsLogger");
            _unitOfWork = unitOfWork;
        }


        public async Task<IEnumerable<Negotiation>> Handle(GetAllNegotiationsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Getting all negotiations");
            return await _unitOfWork.Negotiations.GetAll();
        }
    }
}
