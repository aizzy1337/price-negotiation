using MediatR;
using Microsoft.Extensions.Logging;
using priceNegotiationAPI.Commands;
using priceNegotiationAPI.Models.Dto;
using priceNegotiationAPI.UnitsOfWork;

namespace priceNegotiationAPI.Handlers
{
    public class ResponseNegotiationHandler : IRequestHandler<ResponseNegotiationRequest, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;

        public ResponseNegotiationHandler(IUnitOfWork unitOfWork, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger("negotiationsLogger");
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(ResponseNegotiationRequest request, CancellationToken cancellationToken)
        {
            if (request.PatchDTO == null)
            {
                _logger.LogError("Recievied object is null");
                return false;
            }

            var negotiation = await _unitOfWork.Negotiations.GetById(request.NegotiationId);
            if (negotiation == null)
            {
                _logger.LogError("Object of given index was not found");
                return false;
            }

            NegotiationDTO modelDTO = new()
            {
                Id = negotiation.Id,
                ProductId = (int)negotiation.ProductId,
                ProposedPrice = negotiation.ProposedPrice,
                Accepted = negotiation.Accepted,
                WasHandled = true
            };

            try
            {
                request.PatchDTO.ApplyTo(modelDTO);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return false;
            }

            if (modelDTO.Id != negotiation.Id || modelDTO.ProductId != (int)negotiation.ProductId ||
                modelDTO.ProposedPrice != negotiation.ProposedPrice)
            {
                _logger.LogError("Other fields then Accepted were changed");
                return false;
            }
            else if (negotiation.Accepted == true && modelDTO.Accepted == false)
            {
                _logger.LogError("The negotiation was accepted, can't change it's status");
                return false;
            }
            else if (negotiation.WasHandled == true && modelDTO.WasHandled == false)
            {
                _logger.LogError("The negotiation was handled, can't change it's status");
                return false;
            }

            await _unitOfWork.Negotiations.HandleNegotiation(negotiation, modelDTO.Accepted);
            await _unitOfWork.CompleteAsync();
            return true;

        }
    }
}
