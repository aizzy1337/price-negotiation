using MediatR;
using priceNegotiationAPI.Commands;
using priceNegotiationAPI.Models;
using priceNegotiationAPI.Models.Dto;
using priceNegotiationAPI.UnitsOfWork;

namespace priceNegotiationAPI.Handlers
{
    public class CreateNegotiationHandler : IRequestHandler<CreateNegotiationRequest, Negotiation>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;

        public CreateNegotiationHandler(IUnitOfWork unitOfWork, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger("negotiationsLogger");
            _unitOfWork = unitOfWork;
        }

        public async Task<Negotiation> Handle(CreateNegotiationRequest request, CancellationToken cancellationToken)
        {
            var negotiationDTO = request.NegotiationDTO;

            if (negotiationDTO == null)
            {
                _logger.LogError("Recievied object is null");
                return null;
            }
            else if (negotiationDTO.Id != 0)
            {
                _logger.LogError("Object has ID diffrent then 0");
                return null;
            }
            else if (negotiationDTO.Accepted == true || negotiationDTO.WasHandled == true)
            {
                _logger.LogError("Object can't be accepted or handled on creation");
                return null;
            }

            var product = await _unitOfWork.Products.GetById(negotiationDTO.ProductId);

            if (product == null)
            {
                _logger.LogError("Related object was not found");
                return null;
            }
            else if (2 * product.Price < negotiationDTO.ProposedPrice)
            {
                _logger.LogError("Proposed prize can't be two times larger then base prize");
                return null;
            }
            else if (negotiationDTO.ProposedPrice <= 0)
            {
                _logger.LogError("Proposed prize can't be 0 or negative");
                return null;
            }
            else if (product.Negotiations.FirstOrDefault(x => x.WasHandled == false) != null)
            {
                _logger.LogError("Can't create new negotiation when previous is unhandled");
                return null;
            }
            else if (product.Negotiations.Count == 4)
            {
                _logger.LogError("Only four negotiations for one product are allowed");
                return null;
            }

            Negotiation model = new Negotiation()
            {
                ProposedPrice = negotiationDTO.ProposedPrice,
                Accepted = negotiationDTO.Accepted,
                WasHandled = negotiationDTO.WasHandled,
                ProductId = product.Id,
                CreatedDate = DateTime.Now
            };

            await _unitOfWork.Negotiations.Add(model);
            await _unitOfWork.CompleteAsync();

            return model;   
        }
    }
}
