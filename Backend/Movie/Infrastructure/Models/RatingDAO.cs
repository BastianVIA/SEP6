using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Movie.Infrastructure.Models;

public class RatingDAO
{
    [Key]
    [ForeignKey("Movie")]
    public string MovieId  { get; set; }
    [Required]
    public double Rating  { get; set; }
    [Required]
    public int Votes { get; set; }
}