using Backend.Database.TransactionManager;
using Backend.SocialFeed.Domain;
using Backend.SocialFeed.Infrastructure;
using MediatR;

namespace Backend.SocialFeed.EventHandler;

public class SocialUserCreatedEventHandler : INotificationHandler<SocialUserCreatedEvent>
{
    private readonly IDatabaseTransactionFactory _transactionFactory;
    private readonly IPostRepository _postRepository;

    public SocialUserCreatedEventHandler(IDatabaseTransactionFactory transactionFactory, IPostRepository postRepository)
    {
        _transactionFactory = transactionFactory;
        _postRepository = postRepository;
    }

    public async Task Handle(SocialUserCreatedEvent notification, CancellationToken cancellationToken)
    {
        var transaction = _transactionFactory.GetCurrentTransaction();
        try
        {
            var newPost = new Post(notification.Id, Activity.NewUser);
            await _postRepository.CreatePostAsync(newPost, transaction);
        }
        catch (Exception e)
        {
            await transaction.RollbackTransactionAsync();
            throw;
        }    
    }
}