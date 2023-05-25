using System.ComponentModel.DataAnnotations;
using Backend.Foundation;

namespace Backend.Social.Domain;

public class SocialUser : BaseDomain
{
    public string Id { get; set; }
    public List<string>? Following { get; set; }

    public SocialUser()
    {
    }

    public SocialUser(string id)
    {
        if (id == "")
        {
            throw new ValidationException(
                $"Trying to add a SocialUser with empty userId, this is not allowed, a SocialUser has to have a userId");
        }

        Id = id;
        AddDomainEvent(new SocialUserCreatedEvent(Id));
    }

    public bool AlreadyFollows(string userToCheckIfFollows)
    {
        if (Following == null)
        {
            return false;
        }

        foreach (var user in Following)
        {
            if (user == userToCheckIfFollows)
            {
                return true;
            }
        }

        return false;
    }

    public void StartFollowing(string userToFollowId)
    {
        if (AlreadyFollows(userToFollowId))
        {
            throw new ValidationException($"Tried to follow, but user already follows {userToFollowId}");
        }

        if (Following == null)
        {
            Following = new List<string>();
        }

        Following.Add(userToFollowId);
    }

    public void UnFollow(string userToUnFollowId)
    {
        if (!AlreadyFollows(userToUnFollowId))
        {
            throw new ValidationException($"Tried to unfollow, but user does not follow {userToUnFollowId}");
        }

        Following.Remove(userToUnFollowId);
    }
}