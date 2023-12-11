using Castle.Core.Logging;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using priceNegotiationAPI.Commands;
using priceNegotiationAPI.Handlers;
using priceNegotiationAPI.Models;
using priceNegotiationAPI.Models.Dto;
using priceNegotiationAPI.UnitsOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Members.Commands
{
    public class CreateNegotiationHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;

        public CreateNegotiationHandlerTests()
        {
            _unitOfWorkMock = new();
        }

        [Fact]
        public async void Handle_Should_ReturnNull_WhenRecivedObjectIsNull()
        {
            // Arrange
            NegotiationDTO negotiationDTO = null;
            var command = new CreateNegotiationRequest(negotiationDTO);
            var handler = new CreateNegotiationHandler(_unitOfWorkMock.Object, NullLoggerFactory.Instance);

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async void Handle_Should_ReturnNull_WhenRecivedObjectHasIdNotEqualToZero()
        {
            // Arrange
            NegotiationDTO negotiationDTO = new NegotiationDTO();
            negotiationDTO.Id = 1;
            negotiationDTO.ProductId = 1;
            negotiationDTO.Accepted = true;
            negotiationDTO.ProposedPrice = 12.00;
            negotiationDTO.WasHandled = true;
            var command = new CreateNegotiationRequest(negotiationDTO);
            var handler = new CreateNegotiationHandler(_unitOfWorkMock.Object, NullLoggerFactory.Instance);

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            Assert.Null(result);
        }
    }
}
