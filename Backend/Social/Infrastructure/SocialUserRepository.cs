using Backend.Database.Transaction;
using Backend.Social.Domain;
using Backend.Social.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Social.Infrastructure;

public class SocialUserRepository : ISocialUserRepository
{
    public async Task<SocialUser> ReadSocialUserAsync(string id, DbReadOnlyTransaction tx, bool includeFollowing = false)
    {
        var query = tx.DataContext.SocialUsers
            .Where(s => s.Id == id);
        
        if (includeFollowing)
        {
            query = query.Include(s => s.Following);
        }
        var user = await query.FirstOrDefaultAsync();

        if (user == null)
        {
            throw new KeyNotFoundException($"Could not find user with id: {id}");
        }
        
        return ToDomain(user);
    }

    public void CreateSocialUserAsync(SocialUser user, DbTransaction tx)
    {
        tx.AddDomainEvents(user.ReadAllDomainEvents());
        tx.DataContext.SocialUsers.Add(new SocialUserDAO
        {
            Id = user.Id
        });
    }
    
    public async Task UpdateSocialUserAsync(SocialUser socialUser, DbTransaction tx)
    {
        tx.AddDomainEvents(socialUser.ReadAllDomainEvents());
        var userDao = await tx.DataContext.SocialUsers
            .Include(s => s.Following)
            .SingleAsync(s => s.Id == socialUser.Id);

        if (socialUser.Following != null)
        {
            if (userDao.Following == null)
            {
                userDao.Following = new List<SocialUserDAO>();
            }

            await FromDomain(userDao.Following, socialUser.Following, tx);
        }

        tx.DataContext.SocialUsers.Update(userDao);
    }
    
    private async Task FromDomain(List<SocialUserDAO> following, List<string> socialUserFollowing, DbReadOnlyTransaction tx)
    {
        following.RemoveAll(s => !socialUserFollowing.Contains(s.Id));

        foreach (var userId in socialUserFollowing)
        {
            var userInList = following.Any(dao => dao.Id == userId);
            if (!userInList)
            {
                var toFollow = await tx.DataContext.SocialUsers.FindAsync(userId);
                if (toFollow != null)
                {
                    following.Add(toFollow);
                }
            }
        }
    }
    
    private Domain.SocialUser ToDomain(SocialUserDAO userDao)
    {
        return new SocialUser
        {
            Id = userDao.Id,
            Following = ToDomain(userDao.Following)
        };
    }

    private List<string>? ToDomain(List<SocialUserDAO>? daos)
    {
        if (daos == null)
        {
            return null;
        }

        var domainUsers = new List<string>();
        foreach (var socialUserDao in daos)
        {
            domainUsers.Add(socialUserDao.Id);
        }

        return domainUsers;
    }
}