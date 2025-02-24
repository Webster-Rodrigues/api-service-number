using api_service_number.Models;
using api_service_number.Models.Models.Enum;
using api_service_number.Services;
using Microsoft.AspNetCore.Mvc;

namespace api_service_number.Controllers;

[Route("tickets")]
[ApiController]
public class TicketController : Controller
{
    private readonly ITicketService _ticketServiceservice;
    private readonly ILogger _logger;
    private readonly GeolocationService _geolocationService;

    public TicketController(ITicketService ticketServiceservice, ILogger<TicketController> logger, GeolocationService geolocationService)
    {
        this._ticketServiceservice = ticketServiceservice;
        _logger = logger;
        _geolocationService = geolocationService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Ticket>> GetAll()
    {
        _logger.LogInformation("[START] Executando Get-> /tickets | Retorna lista com todos os tickets");
        
        var tickets = _ticketServiceservice.GetAllSortedTickets().OrderBy(ticket => ticket.Priority);
        if (tickets == null)
        {
            _logger.LogInformation($"[NOT FOUND] Nenhum ticket encontrado");
            return NotFound();
        }
        _logger.LogInformation($"[SUCCESS] tickets encontrados!");
        return Ok(tickets);
    }
    
    [HttpGet("{id}")]
    public ActionResult<Ticket> GetId(int id)
    {
        _logger.LogInformation($"[START] Executando GetId-> /tickets/{id} | Retorna registro do ticket de ID = {id}");
        
        var ticket = _ticketServiceservice.GetById(id);
        if (ticket == null)
        {
            _logger.LogInformation($"[NOT FOUND] Nenhum ticket encontrado com o ID = {id}");
            return  NotFound() ;
        }
        _logger.LogInformation($"[SUCCESS] ticket de ID = {id} encontrado!");
        return Ok(ticket);
    }

    
    [HttpGet("status/{status}")]
    public ActionResult<IQueryable<Ticket>> GetTicketsByStatus(Status status)
    {
        _logger.LogInformation($"[START] Executando GetByStatus -> tickets/status/{status} | Retorna lista de ticket com status = {status}");
        
        var tickets = _ticketServiceservice.GetTicketsByStatus(status);
        if (tickets == null)
        {
            _logger.LogInformation($"[NOT FOUND] Nenhum ticket encontrado com prioridade = {status}");
            return NotFound();
        }
        _logger.LogInformation($"[SUCCESS] {tickets.Count()} tickets encontrados com o status = {status}");
        return Ok(tickets);
    }

    
    [HttpGet("priority/{priority}")]
    public ActionResult<IQueryable<Ticket>> GetTicketsByPriority(Priority priority)
    {
        _logger.LogInformation($"[START] Executando GetByPriority -> /tickets/priority/{priority}");

        var tickets = _ticketServiceservice.GetTicketsByPriority(priority);
    
        if (tickets == null || !tickets.Any()) // Melhor validar se a lista está vazia
        {
            _logger.LogWarning($"[NOT FOUND] Nenhum ticket encontrado com prioridade = {priority}");
            return NotFound();
        }

        _logger.LogInformation($"[SUCCESS] {tickets.Count()} tickets encontrados com prioridade = {priority}");
        return Ok(tickets);
    }

    
    [HttpPost]
    public async Task<ActionResult<Ticket>> Post([FromQuery] Priority priority)
    {
        _logger.LogInformation($"[START] Executando Post -> /tickets | Cria um ticket a partir da prioridade = {priority}");

        var ipAddress = Request.Headers["X-Forwarded-For"].FirstOrDefault() ?? HttpContext.Connection.RemoteIpAddress?.ToString();
        
        //var geolocation = await _geolocationService.GetGeolocationAsync(ipAddress);
        
        var geolocation = await _geolocationService.GetGeolocationAsync("89.248.169.10"); //Única alternativa para testes locais
        _logger.LogInformation($"IP Detectado: {ipAddress}"); //Testando a captura do IP para saber se está capturando 



        if (geolocation == null)
        {
            _logger.LogWarning("[ERROR] Falha ao obter geolocalização.");
            return StatusCode(500, "Erro ao obter geolocalização.");
        }
        
        var ticket = await _ticketServiceservice.Create(priority,geolocation);
        if (ticket == null)
        {
            _logger.LogWarning($"[BAD REQUEST] Não foi possível criar o ticket para a prioridade = {priority}. Dados inválidos ou falta de informação.");
            return BadRequest();
        }
        
        _logger.LogInformation($"[SUCCESS] Ticket criado com sucesso");
        //Corrige o 200
        return CreatedAtAction(nameof(GetAll), new { id = ticket.Id }, ticket); 

    }

    
    [HttpPut("{id:int}")]
    public ActionResult<Ticket> Put(int id, Ticket? ticket)
    {
        _logger.LogInformation($"[START] Executando Put -> /tickets/{id} | Atualiza todos os atributos do ticket");
        if (id != ticket.Id)
        {
            _logger.LogWarning($"[BAD REQUEST] Não foi possível atualizar o ticket de ID = {id}. Dados inválidos ou falta de informação.");
            return BadRequest();
        }
        
        _logger.LogInformation($"[SUCCESS] Ticket atualizado com sucesso");
        _ticketServiceservice.Update(ticket);
        return Ok(ticket);
    }
    

    [HttpDelete("{id:int}")]
    public ActionResult<Ticket> Delete(int id)
    {
        _logger.LogInformation($"[START] Executando Delete -> /tickets/{id} | Deleta a ticket de ID = {id}");
        var ticket = _ticketServiceservice.GetById(id);
        if (ticket == null)
        {
            _logger.LogWarning($"[BAD REQUEST] Não foi possível deletar o ticket de ID = {id}. Dados inválidos ou falta de informação.");
            return BadRequest();
        }
        _logger.LogInformation($"[SUCCESS] Ticket de ID{id} deletado com sucesso");
        _ticketServiceservice.Delete(ticket);
        return NoContent();
    }


    [HttpPatch("{id:int}/finalize")]
    public ActionResult<Ticket> Finalize(int id)
    {
        _logger.LogInformation($"[START] Executando Patch -> /tickets/{id}/finalize | Altera o status do ticket de ID = {id} para finalizado");
        if (!_ticketServiceservice.CloseTicket(id))
        {
            _logger.LogInformation($"[NOT FOUND] Nenhum ticket encontrado com o ID = {id}");
            return NotFound();
        }
        _logger.LogInformation($"[SUCCESS] Ticket de ID{id} finalizado com sucesso");
        return NoContent();
    }
    
    
    
}