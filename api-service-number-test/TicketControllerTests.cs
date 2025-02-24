using api_service_number.Controllers;
using api_service_number.Models;
using api_service_number.Models.Models.Enum;
using api_service_number.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq.Protected;
using Newtonsoft.Json;

namespace api_service_number.Tests
{
    public class TicketControllerTests
    {
        private readonly TicketController _controller;
        private readonly Mock<ITicketService> _mockTicketService;
        private readonly Mock<ILogger<TicketController>> _mockLogger;
        private readonly GeolocationService _geolocationService;
        
        public TicketControllerTests()
        {
            _mockTicketService = new Mock<ITicketService>();
            _mockLogger = new Mock<ILogger<TicketController>>();

            // Configura o HttpClient mockado
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(new GeoLocationDTO
                    {
                        Country = "Brazil",
                        CountryCode = "BR",
                        City = "São Paulo",
                        Region = "SP",
                        RegionName = "Southeast",
                        TimeZone = "BRT",
                        Status = "Success"
                    })),
                });

            var httpClient = new HttpClient(mockHttpMessageHandler.Object)
            {
                BaseAddress = new System.Uri("http://ip-api.com/")
            };

            _geolocationService = new GeolocationService(httpClient);
            _controller = new TicketController(_mockTicketService.Object, _mockLogger.Object, _geolocationService);
        }


        [Fact]
        public void GetAll_ReturnsOkResult_WithListOfTickets()
        {
            // Arrange
            var tickets = new List<Ticket> { new Ticket(), new Ticket() };
            _mockTicketService.Setup(service => service.GetAll()).Returns(tickets);

            // Act
            var result = _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedTickets = Assert.IsType<List<Ticket>>(okResult.Value);
            Assert.Equal(tickets.Count, returnedTickets.Count);
        }

        [Fact]
        public void GetId_ReturnsOkResult_WithTicket()
        {
            // Arrange
            int testId = 1;
            var ticket = new Ticket { Id = testId };
            _mockTicketService.Setup(service => service.GetById(It.Is<int>(id => id == testId))).Returns(ticket);

            // Act
            var result = _controller.GetId(testId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedTicket = Assert.IsType<Ticket>(okResult.Value);
            Assert.Equal(testId, returnedTicket.Id);
        }

        [Fact]
        public void GetTicketsByStatus_ReturnsOkResult_WithTickets()
        {
            // Arrange
            var status = Status.Active;
            var tickets = new List<Ticket> { new Ticket(), new Ticket() }.AsQueryable();
            _mockTicketService.Setup(service => service.GetTicketsByStatus(It.Is<Status>(s => s == status))).Returns(tickets);

            // Act
            var result = _controller.GetTicketsByStatus(status);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedTickets = Assert.IsAssignableFrom<IQueryable<Ticket>>(okResult.Value);
            Assert.Equal(tickets.Count(), returnedTickets.Count());
        }

        [Fact]
        public void GetTicketsByPriority_ReturnsOkResult_WithTickets()
        {
            // Arrange
            var priority = Priority.Elderly;
            var tickets = new List<Ticket> { new Ticket(), new Ticket() }.AsQueryable();
            _mockTicketService.Setup(service => service.GetTicketsByPriority(It.Is<Priority>(p => p == priority))).Returns(tickets);

            // Act
            var result = _controller.GetTicketsByPriority(priority);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedTickets = Assert.IsAssignableFrom<IQueryable<Ticket>>(okResult.Value);
            Assert.Equal(tickets.Count(), returnedTickets.Count());
        }

        [Fact]
        public async Task Post_ReturnsCreatedResult_WithTicket()
        {
            // Arrange
            var priority = Priority.PregnantWoman;
            var geolocation = new GeoLocationDTO
            {
                Country = "Brazil",
                CountryCode = "BR",
                City = "São Paulo",
                Region = "SP",
                RegionName = "Southeast",
                TimeZone = "BRT",
                Status = "Success"
            };
            var ticket = new Ticket();
            _mockTicketService.Setup(service => service.Create(It.Is<Priority>(p => p == priority), It.IsAny<GeoLocationDTO>())).ReturnsAsync(ticket);

            // Act
            var result = await _controller.Post(priority);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnedTicket = Assert.IsType<Ticket>(createdAtActionResult.Value);
            Assert.Equal(ticket, returnedTicket);
        }

        [Fact]
        public void Put_ReturnsOkResult_WithTicket()
        {
            // Arrange
            int testId = 1;
            var ticket = new Ticket { Id = testId };
            _mockTicketService.Setup(service => service.Update(It.Is<Ticket>(t => t.Id == testId)));

            // Act
            var result = _controller.Put(testId, ticket);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedTicket = Assert.IsType<Ticket>(okResult.Value);
            Assert.Equal(testId, returnedTicket.Id);
        }

        [Fact]
        public void Delete_ReturnsNoContentResult()
        {
            // Arrange
            int testId = 1;
            var ticket = new Ticket { Id = testId };
            _mockTicketService.Setup(service => service.GetById(It.Is<int>(id => id == testId))).Returns(ticket);
            _mockTicketService.Setup(service => service.Delete(It.Is<Ticket>(t => t.Id == testId)));

            // Act
            var result = _controller.Delete(testId);

            // Assert
            var noContentResult = result.Result as NoContentResult;
            Assert.NotNull(noContentResult);
        }

        [Fact]
        public void Finalize_ReturnsNoContentResult()
        {
            // Arrange
            int testId = 1;
            _mockTicketService.Setup(service => service.CloseTicket(It.Is<int>(id => id == testId))).Returns(true);

            // Act
            var result = _controller.Finalize(testId);

            // Assert
            var noContentResult = result.Result as NoContentResult;
            Assert.NotNull(noContentResult);
        }
    }
}
