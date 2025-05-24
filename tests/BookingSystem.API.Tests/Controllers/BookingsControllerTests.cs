using BookingSystem.API.Controllers;
using BookingSystem.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace BookingSystem.API.Tests.Controllers
{
    public class BookingsControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly BookingsController _controller;

        public BookingsControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new BookingsController(_mediatorMock.Object);
        }

        [Fact]
        public async Task Book_SuccessfulBooking_ShouldReturnOkWithReference()
        {
            // Arrange
            var request = new BookRequest
            {
                MemberEmail = "test@example.com",
                InventoryItemName = "Test Item"
            };

            var response = new BookingResponse
            {
                Success = true,
                BookingReference = "BK-12345"
            };

            _mediatorMock.Setup(x => x.Send(It.IsAny<CreateBookingCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            var result = await _controller.Book(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = okResult.Value;
            Assert.NotNull(returnValue);
        }

        [Fact]
        public async Task Book_FailedBooking_ShouldReturnBadRequestWithError()
        {
            // Arrange
            var request = new BookRequest
            {
                MemberEmail = "test@example.com",
                InventoryItemName = "Test Item"
            };

            var response = new BookingResponse
            {
                Success = false,
                ErrorMessage = "Member not found"
            };

            _mediatorMock.Setup(x => x.Send(It.IsAny<CreateBookingCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            var result = await _controller.Book(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.NotNull(badRequestResult.Value);
        }

        [Fact]
        public async Task Cancel_SuccessfulCancellation_ShouldReturnOk()
        {
            // Arrange
            var request = new CancelRequest { ReferenceNumber = "BK-12345" };
            var response = new CancelBookingResponse { Success = true };

            _mediatorMock.Setup(x => x.Send(It.IsAny<CancelBookingCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            var result = await _controller.Cancel(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
        }

        [Fact]
        public async Task Cancel_FailedCancellation_ShouldReturnBadRequest()
        {
            // Arrange
            var request = new CancelRequest { ReferenceNumber = "INVALID" };
            var response = new CancelBookingResponse
            {
                Success = false,
                ErrorMessage = "Booking not found"
            };

            _mediatorMock.Setup(x => x.Send(It.IsAny<CancelBookingCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            var result = await _controller.Cancel(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.NotNull(badRequestResult.Value);
        }
    }
}
