namespace Frontend.Entities;

[Serializable]
public class Actor : Person
{
    public List<Movie> ActedInList { get; set; }
}