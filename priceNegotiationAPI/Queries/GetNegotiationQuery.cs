using MediatR;
using priceNegotiationAPI.Models;

namespace priceNegotiationAPI.Queries
{
    public class GetNegotiationQuery : IRequest<Negotiation>
    {
        public int NegotiationId { get; }

        public GetNegotiationQuery(int negotiationId)
        {
            NegotiationId = negotiationId;
        }
    }
}
