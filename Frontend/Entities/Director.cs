namespace Frontend.Entities;

[Serializable]
public class Director : Person
{
    public List<Movie> DirectedList { get; set; }
}