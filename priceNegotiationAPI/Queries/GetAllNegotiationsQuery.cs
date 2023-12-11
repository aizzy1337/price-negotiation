using MediatR;
using priceNegotiationAPI.Models;

namespace priceNegotiationAPI.Queries
{
    public class GetAllNegotiationsQuery : IRequest<IEnumerable<Negotiation>>
    {

    }
}
