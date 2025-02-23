using api_service_number.Models;
using api_service_number.Models.Models.Enum;
using api_service_number.Services;
using Microsoft.AspNetCore.Mvc;

namespace api_service_number.Controllers;

[Route("tickets")]
[ApiController]
public class TicketController : Controller
{
    private readonly TicketService _ticketServiceservice;
    private readonly ILogger _logger;

    public TicketController(TicketService ticketServiceservice, ILogger<TicketController> logger)
    {
        _ticketServiceservice = ticketServiceservice;
        _logger = logger;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Ticket>> Get()
    {
        _logger.LogInformation("Executando Get-> /tickets | Retorna lista com todos os tickets");
        
        var tickets = _ticketServiceservice.GetAll();
        if (tickets == null)
        {
            _logger.LogInformation("Executando Get-> /tickets | ====NOT FOUND====");
            return NotFound();
        }
        
        return Ok(tickets);
    }

    
    [HttpGet("{id}")]
    public ActionResult<Ticket> GetId(int id)
    {
        _logger.LogInformation($"Executando GetId-> /tickets/{id} | Retorna registro do ticket de ID = {id}");
        
        var ticket = _ticketServiceservice.GetById(id);
        if (ticket == null)
        {
            _logger.LogInformation($"Executando GetId-> /tickets/{id} | ====NOT FOUND====");
            return  NotFound() ;
        }

        return Ok(ticket);
    }

    
    [HttpGet("status/{status}")]
    public ActionResult<IQueryable<Ticket>> GetTicketsByStatus(Status status)
    {
        _logger.LogInformation($"Executando GetByStatus -> tickets/status/{status} | Retorna lista de ticket com status = {status}");
        
        var tickets = _ticketServiceservice.GetTicketsByStatus(status);
        if (tickets == null)
        {
            _logger.LogInformation($"Executando GetByStatus -> tickets/status/{status} | ====NOT FOUND====");
            return NotFound();
        }
        
        return Ok(tickets);
    }

    
    [HttpGet("priority/{priority}")]
    public ActionResult<IQueryable<Ticket>> GetTicketsByPriority(Priority priority)
    {
        _logger.LogInformation($"Executando GetByPriority -> /tickets/priority/{priority} | Retorna lista de ticket com prioridade = {priority}");
        
        var tickets = _ticketServiceservice.GetTicketsByPriority(priority);
        if (tickets == null)
        {
            _logger.LogInformation($"Executando GetByPriority -> /tickets/priority/{priority} | ====NOT FOUND====");
            return NotFound();
        }
  
        return Ok(tickets);
    }

    
    [HttpPost]
    public ActionResult<Ticket> Post([FromQuery] Priority priority)
    {
        _logger.LogInformation($"Executando Post -> /tickets | Cria um ticket a partir da prioridade = {priority}");
        
        var ticket = _ticketServiceservice.Create(priority);
        if (ticket == null)
        {
            _logger.LogInformation("Executando Post -> /tickets |  ====BAD REQUEST====");
            return BadRequest();
        }
        
        //Corrige o 200
        return CreatedAtAction(nameof(Get), new { id = ticket.Id }, ticket); 

    }

    
    [HttpPut("{id:int}")]
    public ActionResult<Ticket> Put(int id, Ticket? ticket)
    {
        _logger.LogInformation($"Executando Put -> /tickets/{id} | Atualiza todos os atributos do ticket");
        if (id != ticket.Id)
        {
            _logger.LogInformation($"Executando Put -> /tickets/{id} | ====BAD REQUEST====");
            return BadRequest();
        }
        
        _ticketServiceservice.Update(ticket);
        return Ok(ticket);
    }
    

    [HttpDelete("{id:int}")]
    public ActionResult<Ticket> Delete(int id)
    {
        _logger.LogInformation($"Executando Delete -> /tickets/{id} | Deleta a ticket de ID = {id}");
        var ticket = _ticketServiceservice.GetById(id);
        if (ticket == null)
        {
            _logger.LogInformation($"Executando Delete -> /tickets/{id} | ====BAD REQUEST====");
            return BadRequest();
        }
        
        _ticketServiceservice.Delete(ticket);
        return NoContent();
    }


    [HttpPatch("{id:int}/finalize")]
    public ActionResult<Ticket> Finalize(int id)
    {
        _logger.LogInformation($"Executando Patch -> /tickets/{id}/finalize | Altera o status do ticket de ID = {id} para finalizado");
        if (!_ticketServiceservice.CloseTicket(id))
        {
            _logger.LogInformation($"Executando Patch -> /tickets/{id}/finalize | ====NOT FOUND====");
            return NotFound();
        }
        return NoContent();

    }
}