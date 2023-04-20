using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Backend.Movie.Infrastructure;

[Index(nameof(Id))]
[Index(nameof(Title))]
public class MovieDAO
{
    [Key]
    public string Id { get; set; }
    [Required]
    public string Title { get; set; }
    [Required]
    public int Year { get; set; }
}