namespace Frontend.Entities;

[Serializable]
public class Actor
{
    public string ID { get; set; }
    public string Name { get; set; }
    public int? BirthYear { get; set; }
}