using api_service_number.Context;
using Microsoft.AspNetCore.Mvc;

namespace api_service_number.Controllers;

public class TicketController : Controller
{
    private readonly AppDbContext _context;

    public TicketController(AppDbContext context)
    {
        _context = context;
    }

   
    
}