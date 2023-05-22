using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.User.Infrastructure;

public class ReviewDAO
{
    [Key]
    [ForeignKey("User")]
    public string UserId { get; set; }
    
    [Required]
    public string Body { get; set; }
}