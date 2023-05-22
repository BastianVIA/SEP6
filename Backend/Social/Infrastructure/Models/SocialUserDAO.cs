using System.ComponentModel.DataAnnotations;

namespace Backend.SocialFeed.Infrastructure;

public class SocialUserDAO
{
   [Key]
    public string Id { get; set; }
    public List<SocialUserDAO>? Following { get; set; }
}