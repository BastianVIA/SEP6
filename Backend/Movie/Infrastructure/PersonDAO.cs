using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Backend.Movie.Infrastructure;

[Index(nameof(Id))]
public class PersonDAO
{
    [Key]
    public string Id { get; set; }
    [Required]
    public string Name { get; set; }
    public int? BirthYear { get; set; }
    public ICollection<MovieDAO> ActedMovies { get; set; }
    public ICollection<MovieDAO> DirectedMovies { get; set; }
}