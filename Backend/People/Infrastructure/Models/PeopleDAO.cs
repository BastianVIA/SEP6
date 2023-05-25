using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Backend.People.Infrastructure.Models;

[Index(nameof(Id))]
public class PeopleDAO
{
    [Key]
    public string Id { get; set; }
    [Required]
    public string Name { get; set; }
    public string ImdbId { get; set; }
    public int? BirthYear { get; set; }
    public ICollection<PeopleMovieDAO>? ActedMovies { get; set; }
    public ICollection<PeopleMovieDAO>? DirectedMovies { get; set; }
}