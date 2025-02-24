using api_service_number.Models;
using api_service_number.Models.Models.Enum;
using api_service_number.Repositories;
using api_service_number.Services;
using Moq;

public class TicketServiceTests
{
    private readonly TicketService _ticketService;
    private readonly Mock<ITicketRepository> _mockRepository;

    public TicketServiceTests()
    {
        _mockRepository = new Mock<ITicketRepository>();
        _ticketService = new TicketService(_mockRepository.Object);
    }

    [Fact]
    public void GetAll_ShouldReturnAllTickets()
    {
        // Arrange
        var tickets = new List<Ticket> 
        {
            new Ticket("PRE001", Priority.PregnantWoman),
            new Ticket("ELD002", Priority.Elderly)
        };
        _mockRepository.Setup(repo => repo.GetAll()).Returns(tickets);

        // Act
        var result = _ticketService.GetAll();

        // Assert
        Assert.Equal(2, result.Count());
        Assert.Equal("PRE001", result.First().TicketNumber);
    }

    [Fact]
    public void GetById_Ticket_ShouldReturnTicket()
    {
        // Arrange
        var ticket = new Ticket("PRE001", Priority.PregnantWoman);
        _mockRepository.Setup(repo => repo.GetById(1)).Returns(ticket);

        // Act
        var result = _ticketService.GetById(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("PRE001", result.TicketNumber);
    }

    [Fact]
    public void GetTicketsByPriority_ShouldReturnTicketsByPriority()
    {
        // Arrange
        var tickets = new List<Ticket>
        {
            new Ticket("PRE001", Priority.PregnantWoman),
            new Ticket("ELD002", Priority.Elderly),
            new Ticket("PRE003", Priority.PregnantWoman)
        }.AsQueryable();
        _mockRepository.Setup(repo => repo.GetTicketsByPriority(Priority.PregnantWoman)).Returns(tickets.Where(t => t.Priority == Priority.PregnantWoman));

        // Act
        var result = _ticketService.GetTicketsByPriority(Priority.PregnantWoman);

        // Assert
        Assert.Equal(2, result.Count());
        Assert.All(result, t => Assert.Equal(Priority.PregnantWoman, t.Priority));
    }

    [Fact]
    public void GetTicketsByStatus_ShouldReturnTicketsByStatus()
    {
        // Arrange
        var tickets = new List<Ticket>
        {
            new Ticket("PRE001", Priority.PregnantWoman) { Status = Status.Active },
            new Ticket("ELD002", Priority.Elderly) { Status = Status.Finished },
            new Ticket("PRE003", Priority.PregnantWoman) { Status = Status.Active }
        }.AsQueryable();
        _mockRepository.Setup(repo => repo.GetTicketsByStatus(Status.Active)).Returns(tickets.Where(t => t.Status == Status.Active));

        // Act
        var result = _ticketService.GetTicketsByStatus(Status.Active);

        // Assert
        Assert.Equal(2, result.Count());
        Assert.All(result, t => Assert.Equal(Status.Active, t.Status));
    }

    [Fact]
    public async Task Create_ShouldReturnNewTicket()
    {
        // Arrange
        var priority = Priority.PregnantWoman;
        var geoLocation = new GeoLocationDTO 
        { 
            Country = "Brasil",
            CountryCode = "BR",
            City = "São Paulo",
            Region = "SP",
            RegionName = "São Paulo",
            TimeZone = "BRT",
            Status = "Active"
        };
        var lastTicket = new Ticket("PRE001", Priority.PregnantWoman);
        _mockRepository.Setup(repo => repo.GetAll()).Returns(new List<Ticket> { lastTicket }.AsQueryable());
        _mockRepository.Setup(repo => repo.CreateAsync(It.IsAny<Ticket>())).Returns(Task.FromResult(new Ticket("PRE002", priority)));

        // Act
        var result = await _ticketService.Create(priority, geoLocation);

        // Assert
        Assert.NotNull(result);
        Assert.StartsWith("PRE", result.TicketNumber);
    }


    [Fact]
    public void Update_ShouldReturnUpdatedTicket()
    {
        // Arrange
        var ticket = new Ticket("PRE001", Priority.PregnantWoman);
        _mockRepository.Setup(repo => repo.Update(ticket)).Returns(ticket);

        // Act
        var result = _ticketService.Update(ticket);

        // Assert
        Assert.Equal("PRE001", result.TicketNumber);
    }

    [Fact]
    public void Delete_ShouldReturnDeletedTicket()
    {
        // Arrange
        var ticket = new Ticket("PRE001", Priority.PregnantWoman);
        _mockRepository.Setup(repo => repo.Delete(ticket)).Returns(ticket);

        // Act
        var result = _ticketService.Delete(ticket);

        // Assert
        Assert.Equal("PRE001", result.TicketNumber);
    }

    [Fact]
    public void CloseTicket_ShouldReturnTrueWhenTicketIsClosed()
    {
        // Arrange
        var ticket = new Ticket("PRE001", Priority.PregnantWoman) { Status = Status.Active };
        _mockRepository.Setup(repo => repo.GetById(1)).Returns(ticket);
        _mockRepository.Setup(repo => repo.Update(ticket)).Returns(ticket);

        // Act
        var result = _ticketService.CloseTicket(1);

        // Assert
        Assert.True(result);
        Assert.Equal(Status.Finished, ticket.Status);
    }

    [Fact]
    public async Task CancelTicket_ShouldCancelAllExpiredTickets()
    {
        // Arrange
        var tickets = new List<Ticket>
        {
            new Ticket("PRE001", Priority.PregnantWoman) { Status = Status.Active, EndDate = DateTime.Now.AddDays(-1) },
            new Ticket("ELD002", Priority.Elderly) { Status = Status.Active, EndDate = DateTime.Now.AddDays(-2) }
        };
        _mockRepository.Setup(repo => repo.GetExpiredTickets()).ReturnsAsync(tickets);
        _mockRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Ticket>())).Returns(Task.FromResult(new Ticket("PRE001", Priority.PregnantWoman)));

        // Act
        await _ticketService.CancelTicket();

        // Assert
        _mockRepository.Verify(repo => repo.UpdateAsync(It.Is<Ticket>(t => t.Status == Status.Canceled)), Times.Exactly(2));
    }

}