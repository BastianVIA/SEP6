using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Backend.Movie.Infrastructure;

public class RatingDAO
{
    [Key]
    [ForeignKey("Movie")]
    public string MovieId  { get; set; }
    [Required]
    public float Rating  { get; set; }
    [Required]
    public int Votes { get; set; }
}