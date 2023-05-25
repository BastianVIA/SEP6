using Microsoft.EntityFrameworkCore;

namespace Backend.Movie.Infrastructure.Models;

[Index(nameof(Id))]
public class PersonDAO
{
    public string Id { get; set; }
    public ICollection<MovieDAO> ActedMovies { get; set; }
    public ICollection<MovieDAO> DirectedMovies { get; set; }
}