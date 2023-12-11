using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using priceNegotiationAPI.Commands;
using priceNegotiationAPI.Data;
using priceNegotiationAPI.Models;
using priceNegotiationAPI.Models.Dto;
using priceNegotiationAPI.Queries;
using priceNegotiationAPI.UnitsOfWork;

namespace priceNegotiationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PriceNegotiationController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly IMediator _mediator;

        public PriceNegotiationController(IUnitOfWork unitOfWork, ILoggerFactory loggerFactory, IMediator mediator)
        {
            _mediator = mediator;
            _unitOfWork = unitOfWork;
            _logger = loggerFactory.CreateLogger("priceNegotiationControllerLogger");
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<NegotiationDTO>>> GetNegotiations()
        {
            var query = new GetAllNegotiationsQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id:int}", Name = "GetNegotiation")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<NegotiationDTO>> GetNegotiation(int id)
        {
            var query = new GetNegotiationQuery(id);
            var result = await _mediator.Send(query);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<NegotiationDTO>> CreateNegotiation([FromBody] NegotiationDTO negotiationDTO)
        {
            var command = new CreateNegotiationRequest(negotiationDTO);
            var result = await _mediator.Send(command);

            if(result == null)
            {
                return BadRequest();
            }

            return CreatedAtRoute("GetNegotiation", new { id = negotiationDTO.Id }, negotiationDTO);
        }

        [HttpDelete("{id:int}", Name = "DeleteNegotiation")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteNegotiation(int id)
        {
            var command = new DeleteNegotiationRequest(id);
            var result = await _mediator.Send(command);

            if (result == false)
            {
                return BadRequest();
            }

            return NoContent();
        }

        [HttpPut("", Name = "UpdateNegotiation")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateNegotiation([FromBody] NegotiationDTO negotiationDTO)
        {
            var command = new UpdateNegotiationRequest(negotiationDTO);
            var result = await _mediator.Send(command);

            if (result == false)
            {
                return BadRequest();
            }

            return NoContent();
        }

        /*
         * Usage:
         * [
            {
               "path": "/Accepted", - field that defines if negotiation was accepted
               "op": "replace", - operation we are using
               "value": "true" - true/false
            }
           ]
        */
        [HttpPatch("{id:int}", Name = "HandleNegotiation")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> HandleNegotiation(int id, JsonPatchDocument<NegotiationDTO> patchDTO)
        {
            var command = new ResponseNegotiationRequest(id, patchDTO);
            var result = await _mediator.Send(command);

            if (result == false)
            {
                return BadRequest();
            }

            return NoContent();
        }
    }
}
