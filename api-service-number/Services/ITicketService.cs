using api_service_number.Models;
using api_service_number.Models.Models.Enum;

namespace api_service_number.Services;

public interface ITicketService
{
    IEnumerable<Ticket> GetAll();
    Ticket GetById(int id);
    IQueryable<Ticket> GetTicketsByStatus(Status status);
    IQueryable<Ticket> GetTicketsByPriority(Priority priority);
    Task<Ticket> Create(Priority priority, GeoLocationDTO geolocation);
    Ticket Update(Ticket ticket);
    Ticket Delete(Ticket ticket);
    bool CloseTicket(int id);
    IEnumerable<Ticket> GetAllSortedTickets();

}
