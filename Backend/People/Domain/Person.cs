namespace Backend.People.Domain;

public class Person
{
    public string Id { get; set; }
    public string Name { get; set; }
    public int? BirthYear { get; set; }
    public ICollection<string>? ActedMoviesId { get; set; }
    public ICollection<string>? DirectedMoviesId { get; set; }
}