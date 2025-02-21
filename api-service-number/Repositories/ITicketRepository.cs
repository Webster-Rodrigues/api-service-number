using api_service_number.Models;
using api_service_number.Models.Models.Enum;

namespace api_service_number.Repositories;

public interface ITicketRepository : IRepository<Ticket>
{
    IQueryable<Ticket>? GetTicketsByStatus(Status status);
    IQueryable<Ticket>? GetTicketsByPriority(Priority priority);
    
}