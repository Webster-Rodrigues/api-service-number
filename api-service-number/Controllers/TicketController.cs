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

    public TicketController(TicketService ticketServiceservice)
    {
        _ticketServiceservice = ticketServiceservice;
    }


    [HttpGet]
    public ActionResult<IEnumerable<Ticket>> Get()
    {
        var tickets = _ticketServiceservice.GetAll();
        if (tickets == null)return NotFound();
        
        return Ok(tickets);
    }

    
    [HttpGet("{id}")]
    public ActionResult<Ticket> Get(int id)
    {
        var ticket = _ticketServiceservice.GetById(id);
        if (ticket == null)return NotFound();

        return Ok(ticket);
    }

    
    [HttpGet("status/{status}")]
    public ActionResult<IQueryable<Ticket>> GetTicketsByStatus(Status status)
    {
        var tickets = _ticketServiceservice.GetTicketsByStatus(status);
        if (tickets == null) return NotFound();
        
        return Ok(tickets);
    }

    
    [HttpGet("priority/{priority}")]
    public ActionResult<IQueryable<Ticket>> GetTicketsByPriority(Priority priority)
    {
        var tickets = _ticketServiceservice.GetTicketsByPriority(priority);
        if (tickets == null) return NotFound();
  
        return Ok(tickets);
    }

    
    [HttpPost]
    public ActionResult<Ticket> Post([FromQuery] Priority priority)
    {
        var ticket = _ticketServiceservice.Create(priority);
        if (ticket == null) return BadRequest();
        
        //Corrige o 200
        return CreatedAtAction(nameof(Get), new { id = ticket.Id }, ticket); 

    }

    
    [HttpPut("{id:int}")]
    public ActionResult<Ticket> Put(int id, Ticket? ticket)
    {
        if (id != ticket.Id)return BadRequest();
        
        _ticketServiceservice.Update(ticket);
        return Ok(ticket);
    }
    

    [HttpDelete("{id:int}")]
    public ActionResult<Ticket> Delete(int id)
    {
        var ticket = _ticketServiceservice.GetById(id);
        if (ticket == null) return BadRequest();
        
        _ticketServiceservice.Delete(ticket);
        return NoContent();
    }


    [HttpPatch("{id:int}/finalize")]
    public ActionResult<Ticket> Finalize(int id)
    {
        if(!_ticketServiceservice.CloseTicket(id)) return NotFound();
        return NoContent();

    }
}