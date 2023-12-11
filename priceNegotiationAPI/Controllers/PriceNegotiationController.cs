using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using priceNegotiationAPI.Data;
using priceNegotiationAPI.Models;
using priceNegotiationAPI.Models.Dto;
using priceNegotiationAPI.UnitsOfWork;

namespace priceNegotiationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PriceNegotiationController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;

        public PriceNegotiationController(IUnitOfWork unitOfWork, ILoggerFactory loggerFactory)
        {
            _unitOfWork = unitOfWork;
            _logger = loggerFactory.CreateLogger("/log/priceNegotiationControllerLog");
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<NegotiationDTO>>> GetNegotiations()
        {
            _logger.LogInformation("Getting all negotiations");
            return Ok(await _unitOfWork.Negotiations.GetAll());
        }

        [HttpGet("{id:int}", Name = "GetNegotiation")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<NegotiationDTO>> GetNegotiation(int id)
        {
            if (id < 1)
            {
                _logger.LogError("Index 0 and negative is not acceptable");
                return BadRequest();
            }
            
            var negotiation = await _unitOfWork.Negotiations.GetById(id);
            if (negotiation == null)
            {
                _logger.LogError("Object of given index was not found");
                return NotFound();
            }

            return Ok(negotiation);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<NegotiationDTO>> CreateNegotiation([FromBody] NegotiationDTO negotiationDTO)
        {
            if (negotiationDTO == null)
            {
                _logger.LogError("Recievied object is null");
                return BadRequest(negotiationDTO);
            }

            if (negotiationDTO.Id != 0)
            {
                _logger.LogError("Object has ID diffrent then 0");
                return BadRequest(negotiationDTO);
            }

            if (negotiationDTO.Accepted == true || negotiationDTO.WasHandled == true)
            {
                _logger.LogError("Object can't be accepted or handled on creation");
                return BadRequest(negotiationDTO);
            }

            var product = await _unitOfWork.Products.GetById(negotiationDTO.ProductId);
            if (product == null)
            {
                _logger.LogError("Related object was not found");
                return BadRequest(negotiationDTO);
            }
            else if (2 * product.Price < negotiationDTO.ProposedPrice)
            {
                _logger.LogError("Proposed prize can't be two times larger then base prize");
                return BadRequest(negotiationDTO);
            }
            else if (negotiationDTO.ProposedPrice <= 0)
            {
                _logger.LogError("Proposed prize can't be 0 or negative");
                return BadRequest(negotiationDTO);
            }
            else if (product.Negotiations.FirstOrDefault(x => x.WasHandled == false) != null)
            {
                _logger.LogError("Can't create new negotiation when previous is unhandled");
                return BadRequest(negotiationDTO);
            }
            else if(product.Negotiations.Count == 4)
            {
                _logger.LogError("Only four negotiations for one product are allowed");
                return BadRequest(negotiationDTO);
            }

            Negotiation model = new Negotiation()
            {
                ProposedPrice = negotiationDTO.ProposedPrice,
                Accepted = negotiationDTO.Accepted,
                WasHandled = negotiationDTO.WasHandled,
                ProductId = product.Id,
                Product = product,
                CreatedDate = DateTime.Now,
            };

            await _unitOfWork.Negotiations.Add(model);
            await _unitOfWork.CompleteAsync();

            return CreatedAtRoute("GetNegotiation", new { id = negotiationDTO.Id }, negotiationDTO);
        }

        [HttpDelete("{id:int}", Name = "DeleteNegotiation")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteNegotiation(int id)
        {
            if (id < 1)
            {
                _logger.LogError("Index 0 and negative is not acceptable");
                return BadRequest();
            }

            var negotiation = await _unitOfWork.Negotiations.GetById(id);
            if (negotiation == null)
            {
                _logger.LogError("Object of given index was not found");
                return NotFound();
            }

            await _unitOfWork.Negotiations.Remove(negotiation);
            await _unitOfWork.CompleteAsync();
            return NoContent();
        }

        [HttpPut("{id:int}", Name = "UpdateNegotiation")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateNegotiation(int id, [FromBody] NegotiationDTO negotiationDTO)
        {
            if (negotiationDTO == null)
            {
                _logger.LogError("Recievied object is null");
                return BadRequest();
            }

            var negotiation = await _unitOfWork.Negotiations.GetById(id);
            if (negotiation == null)
            {
                _logger.LogError("Object of given index was not found");
                return NotFound();
            }

            var product = await _unitOfWork.Products.GetById(negotiationDTO.ProductId);
            if (product == null)
            {
                _logger.LogError("Related object was not found");
                return BadRequest(negotiationDTO);
            }
            else if (2 * product.Price < negotiationDTO.ProposedPrice)
            {
                _logger.LogError("Proposed prize can't be two times larger then base prize");
                return BadRequest(negotiationDTO);
            }
            else if (negotiationDTO.ProposedPrice <= 0)
            {
                _logger.LogError("Proposed prize can't be 0 or negative");
                return BadRequest(negotiationDTO);
            }

            Negotiation model = new Negotiation()
            {
                Id = negotiationDTO.Id,
                ProposedPrice = negotiationDTO.ProposedPrice,
                Accepted = negotiationDTO.Accepted,
                WasHandled = negotiationDTO.WasHandled,
                ProductId = product.Id,
                Product = product,
                CreatedDate = DateTime.Now,
            };

            await _unitOfWork.Negotiations.Update(model);
            await _unitOfWork.CompleteAsync();

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
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> HandleNegotiation(int id, JsonPatchDocument<NegotiationDTO> patchDTO)
        {
            if (patchDTO == null)
            {
                _logger.LogError("Recievied object is null");
                return BadRequest();
            }

            var negotiation = await _unitOfWork.Negotiations.GetById(id);
            if (negotiation == null)
            {
                _logger.LogError("Object of given index was not found");
                return NotFound();
            }

            NegotiationDTO modelDTO = new()
            {
                Id = negotiation.Id,
                ProductId = (int)negotiation.ProductId,
                ProposedPrice = negotiation.ProposedPrice,
                Accepted = negotiation.Accepted,
                WasHandled = true
            };

            patchDTO.ApplyTo(modelDTO, ModelState);

            if (modelDTO.Id != negotiation.Id || modelDTO.ProductId != (int)negotiation.ProductId ||
                modelDTO.ProposedPrice != negotiation.ProposedPrice)
            {
                _logger.LogError("Other fields then Accepted were changed");
                return BadRequest(ModelState);
            }
            else if (negotiation.Accepted == true && modelDTO.Accepted == false)
            {
                _logger.LogError("The negotiation was accepted, can't change it's status");
                return BadRequest(ModelState);
            }
            else if (negotiation.WasHandled == true && modelDTO.WasHandled == false)
            {
                _logger.LogError("The negotiation was handled, can't change it's status");
                return BadRequest(ModelState);
            }
            else
            {
                await _unitOfWork.Negotiations.HandleNegotiation(negotiation, modelDTO.Accepted);
                await _unitOfWork.CompleteAsync();
            }

            return NoContent();
        }
    }
}
