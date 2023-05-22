namespace Backend.Service;

public interface IPersonService
{
    Task<PersonDto?> GetPersonAsync(string id);
}

public class PersonDto
{
    public Uri? PathToProfilePic { get; set; }
    public string? Bio { get; set; }
    public string? KnownFor { get; set; }
}