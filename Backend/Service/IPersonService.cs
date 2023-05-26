namespace Backend.Service;

public interface IPersonService
{
    Task<PersonServiceDto?> GetPersonAsync(string id);
}

public class PersonServiceDto
{
    public Uri? PathToPersonPic { get; set; }
    public string? Bio { get; set; }
    public string? KnownFor { get; set; }
}