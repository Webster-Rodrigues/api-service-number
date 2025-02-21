using api_service_number.Context;
using api_service_number.Models;
using api_service_number.Models.Models.Enum;

namespace api_service_number.Repositories;

public class TicketRepository : Repository<Ticket>, ITicketRepository
{
    public TicketRepository(AppDbContext context) : base(context)
    {
        
    }
    
    public IQueryable<Ticket>? GetTicketsByStatus(Status status)
    {
        return base.context.Tickets?.AsQueryable().Where(t => t.Status == status);
    }

    public IQueryable<Ticket>? GetTicketsByPriority(Priority priority)
    {
        return base.context.Tickets?.AsQueryable().Where(t => t.Priority == priority);
    }
}