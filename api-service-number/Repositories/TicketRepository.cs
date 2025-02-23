using api_service_number.Context;
using api_service_number.Models;
using api_service_number.Models.Models.Enum;
using Microsoft.EntityFrameworkCore;

namespace api_service_number.Repositories;

public class TicketRepository : Repository<Ticket>, ITicketRepository
{
    public TicketRepository(AppDbContext context) : base(context)
    {
        
    }
    
    public IQueryable<Ticket>? GetTicketsByStatus(Status status)
    {
        return context.Tickets?.AsQueryable().Where(t => t.Status == status);
    }

    public IQueryable<Ticket> GetTicketsByPriority(Priority priority)
    {
        return context.Tickets.AsQueryable().Where(t => t.Priority == priority);
    }

    public async Task<IEnumerable<Ticket>> GetExpiredTickets()
    {
        var limite = DateTime.UtcNow.AddMinutes(-2); 

        return await context.Tickets
            .Where(t => t.Status != Status.Finished && t.StartDate <= limite)
            .ToListAsync();
    }

}