using MediatR;
using priceNegotiationAPI.Models;
using priceNegotiationAPI.Models.Dto;

namespace priceNegotiationAPI.Commands
{
    public class CreateNegotiationRequest : IRequest<Negotiation>
    {
        public NegotiationDTO NegotiationDTO { get; }

        public CreateNegotiationRequest(NegotiationDTO negotiation)
        {
            NegotiationDTO = negotiation;
        }
    }
}
