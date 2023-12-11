using MediatR;
using priceNegotiationAPI.Models.Dto;

namespace priceNegotiationAPI.Commands
{
    public class UpdateNegotiationRequest : IRequest<bool>
    {
        public NegotiationDTO NegotiationDTO { get; }

        public UpdateNegotiationRequest(NegotiationDTO negotiation)
        {
            NegotiationDTO = negotiation;
        }
    }
}

