using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using priceNegotiationAPI.Models.Dto;

namespace priceNegotiationAPI.Commands
{
    public class ResponseNegotiationRequest : IRequest<bool>
    {
        public int NegotiationId { get; }
        public JsonPatchDocument<NegotiationDTO> PatchDTO { get; }

        public ResponseNegotiationRequest(int negotiationId, JsonPatchDocument<NegotiationDTO> patchDTO)
        {
            NegotiationId = negotiationId;
            PatchDTO = patchDTO;
        }
    }
}
