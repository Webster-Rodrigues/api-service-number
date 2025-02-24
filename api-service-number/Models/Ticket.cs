using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using api_service_number.Models.Models.Enum;

namespace api_service_number.Models;

[Table("Tickets")]
public class Ticket
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [StringLength(20)]
    public string? TicketNumber { get; set; }
    
    [Required]
    public Priority Priority { get; set; }
    
    [Required]
    public Status Status { get; set; } = Status.Active;

    public DateTime StartDate { get; set; } = DateTime.UtcNow;
    public DateTime? EndDate { get; set; }
    
   public string? GeoLocation { get; set; }
    
    public Ticket()
    {
        
    }

    public Ticket(string ticketNumber ,Priority priority)
    {
        TicketNumber = ticketNumber;
        Priority = priority;
    }
}