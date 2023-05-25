using System.ComponentModel.DataAnnotations;

namespace Backend.Social.Infrastructure.Models;

public class SocialUserDAO
{
   [Key]
    public string Id { get; set; }
    public List<SocialUserDAO>? Following { get; set; }
}