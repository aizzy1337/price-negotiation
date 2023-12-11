using Microsoft.AspNetCore.Http;
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
    public class ProductsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;

        public ProductsController(IUnitOfWork unitOfWork, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger("/log/productsControllerLog");
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetNegotiations()
        {
            _logger.LogInformation("Getting all products");
            return Ok(await _unitOfWork.Products.GetAll());
        }

        [HttpGet("{id:int}", Name = "GetProduct")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductDTO>> GetProduct(int id)
        {
            if (id < 1)
            {
                _logger.LogError("Index 0 and negative is not acceptable");
                return BadRequest();
            }

            var product = await _unitOfWork.Products.GetById(id);
            if (product == null)
            {
                _logger.LogError("Object of given index was not found");
                return NotFound();
            }

            return Ok(product);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ProductDTO>> CreateProduct([FromBody] ProductDTO productDTO)
        {
            if (productDTO == null)
            {
                _logger.LogError("Recievied object is null");
                return BadRequest(productDTO);
            }

            if (productDTO.Id != 0)
            {
                _logger.LogError("Object has ID diffrent then 0");
                return BadRequest(productDTO);
            }

            if (productDTO.Name == "" || productDTO.Price <= 0.0)
            {
                _logger.LogError("Object has no name or price is lower then 0");
                return BadRequest(productDTO);
            }

            Product model = new Product()
            {
                Name = productDTO.Name,
                Description = productDTO.Description,
                Price = productDTO.Price,
                CreatedDate = DateTime.Now
            };

            await _unitOfWork.Products.Add(model);
            await _unitOfWork.CompleteAsync();

            return CreatedAtRoute("GetProduct", new { id = productDTO.Id }, productDTO);
        }

        [HttpDelete("{id:int}", Name = "DeleteProduct")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            if (id < 1)
            {
                _logger.LogError("Index 0 and negative is not acceptable");
                return BadRequest();
            }

            var product = await _unitOfWork.Products.GetById(id);
            if (product == null)
            {
                _logger.LogError("Object of given index was not found");
                return NotFound();
            }

            await _unitOfWork.Products.Remove(product);
            await _unitOfWork.CompleteAsync();
            return NoContent();
        }

        [HttpPut("{id:int}", Name = "UpdateProduct")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductDTO productDTO)
        {
            if (productDTO == null)
            {
                _logger.LogError("Recievied object is null");
                return BadRequest();
            }

            var product = await _unitOfWork.Products.GetById(id);
            if (product == null)
            {
                _logger.LogError("Object of given index was not found");
                return NotFound();
            }

            if (productDTO.Name == "" || productDTO.Price <= 0.0)
            {
                _logger.LogError("Object has no name or price is lower then 0");
                return BadRequest(productDTO);
            }

            Product model = new Product()
            {
                Id = productDTO.Id,
                Name = productDTO.Name,
                Description = productDTO.Description,
                Price = productDTO.Price,
                CreatedDate = DateTime.Now
            };

            await _unitOfWork.Products.Update(model);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }

    }
}
