using api_service_number.Models;
using api_service_number.Models.Models.Enum;
using api_service_number.Repositories;
using Newtonsoft.Json;


namespace api_service_number.Services;

public class TicketService 
{
    private readonly ITicketRepository _repository;

    public TicketService(ITicketRepository repository)
    {
        _repository = repository;
    }


    public IEnumerable<Ticket> GetAll()
    {
        return _repository.GetAll();
    }

    public Ticket? GetById(int id)
    {
        return _repository.GetById(id);
    }

    public IQueryable<Ticket> GetTicketsByPriority(Priority priority)
    {
        return _repository.GetTicketsByPriority(priority);
    }

    public IQueryable<Ticket>? GetTicketsByStatus(Status status)
    {
        return _repository.GetTicketsByStatus(status);
    }

    public async Task<Ticket> Create(Priority priority, GeoLocationDTO geolocation)
    {
        var prefix = priority.ToString().Substring(0, 3).ToUpper();
        
        var lastTicket = _repository.GetAll().Where(t => t.Priority == priority)
            .OrderByDescending(t => t.TicketNumber).FirstOrDefault();

        int numberLastTicket = 1;
        if (lastTicket != null)
        {
            string lastTicketNumber = lastTicket.TicketNumber.Substring(3);

            if (int.TryParse(lastTicketNumber, out int lastNumber)) //TryParse tenta converter. Corrige o bug de conversão 
            {
                numberLastTicket = lastNumber + 1;
            }
        }
        var serialNumber = $"{prefix}{numberLastTicket:D3}";
        
        
        var ticket = new Ticket(serialNumber , priority);
        ticket.GeoLocation = JsonConvert.SerializeObject(geolocation);
        await _repository.CreateAsync(ticket);
        return ticket;
    }
    
    /*public Ticket Create(Priority priority)
    {
        var prefix = priority.ToString().Substring(0, 3).ToUpper();
        
        var lastTicket = _repository.GetAll().Where(t => t.Priority == priority)
            .OrderByDescending(t => t.TicketNumber).FirstOrDefault();

        int numberLastTicket = 1;
        if (lastTicket != null)
        {
            string lastTicketNumber = lastTicket.TicketNumber.Substring(3);

            if (int.TryParse(lastTicketNumber, out int lastNumber)) //TryParse tenta converter. Corrige o bug de conversão 
            {
                numberLastTicket = lastNumber + 1;
            }
        }
        
        var serialNumber = $"{prefix}{numberLastTicket:D3}";
        var ticket = new Ticket(serialNumber , priority);
        return _repository.Create(ticket);
    }*/

    public Ticket Update(Ticket ticket)
    {
        return _repository.Update(ticket);
    }

    public Ticket Delete(Ticket ticket)
    {
        return _repository.Delete(ticket);
    }


    public bool CloseTicket(int id)
    {
        var ticket = _repository.GetById(id);
        if (ticket == null) return false;
        
        ticket.Status = Status.Finished;
        ticket.EndDate = DateTime.Now;
        _repository.Update(ticket);
        return true;
    }

    public async Task CancelTicket()
    {
        var expiredTickets = await _repository.GetExpiredTickets();

        foreach (var ticket in expiredTickets)
        {
            ticket.Status = Status.Canceled;
            ticket.EndDate = DateTime.Now;
             await _repository.UpdateAsync(ticket);
        }
        
    }
    
    
    
    
    
    
}