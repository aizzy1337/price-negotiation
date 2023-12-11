using MediatR;
using priceNegotiationAPI.Models.Dto;

namespace priceNegotiationAPI.Commands
{
    public class DeleteNegotiationRequest : IRequest<bool>
    {
        public int NegotiationId { get; }

        public DeleteNegotiationRequest(int negotiationId)
        {
            NegotiationId = negotiationId;
        }
    }
}
