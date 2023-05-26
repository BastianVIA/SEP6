using System.ComponentModel.DataAnnotations;

namespace Backend.User.Infrastructure.Models;

public class UserProfileDAO
{
    [Key]
    public string Id { get; set; }
    public byte[] ProfilePicture { get; set; }
}