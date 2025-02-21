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
    [StringLength(20)]
    public Priority Priority { get; set; }
    
    [Required]
    [StringLength(20)]
    public Status Status { get; set; }
    
    public DateTime StartDate { get; set; }
    public DateTime EndDate{ get; set; }
}