using MediatR;
using priceNegotiationAPI.Commands;
using priceNegotiationAPI.Models;
using priceNegotiationAPI.Models.Dto;
using priceNegotiationAPI.UnitsOfWork;

namespace priceNegotiationAPI.Handlers
{
    public class UpdateNegotiationHandler : IRequestHandler<UpdateNegotiationRequest, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;

        public UpdateNegotiationHandler(IUnitOfWork unitOfWork, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger("negotiationsLogger");
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(UpdateNegotiationRequest request, CancellationToken cancellationToken)
        {
            var negotiationDTO = request.NegotiationDTO;

            if (negotiationDTO == null)
            {
                _logger.LogError("Recievied object is null");
                return false;
            }

            var negotiation = await _unitOfWork.Negotiations.GetById(negotiationDTO.Id);
            if (negotiation == null)
            {
                _logger.LogError("Object of given index was not found");
                return false;
            }

            var product = await _unitOfWork.Products.GetById(negotiationDTO.ProductId);
            if (product == null)
            {
                _logger.LogError("Related object was not found");
                return false;
            }
            else if (2 * product.Price < negotiationDTO.ProposedPrice)
            {
                _logger.LogError("Proposed prize can't be two times larger then base prize");
                return false;
            }
            else if (negotiationDTO.ProposedPrice <= 0)
            {
                _logger.LogError("Proposed prize can't be 0 or negative");
                return false;
            }

            Negotiation model = new Negotiation()
            {
                Id = negotiationDTO.Id,
                ProposedPrice = negotiationDTO.ProposedPrice,
                Accepted = negotiationDTO.Accepted,
                WasHandled = negotiationDTO.WasHandled,
                ProductId = product.Id,
                CreatedDate = DateTime.Now
            };

            await _unitOfWork.Negotiations.Update(model);
            await _unitOfWork.CompleteAsync();

            return true;
        }
    }
}
