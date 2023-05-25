using Backend.Database.TransactionManager;
using Backend.Social.Infrastructure;
using MediatR;

namespace Backend.Social.Application.CommentOnPost;

public record Command(string PostId, string UserId, string Comment) : IRequest;

public class CommandHandler : IRequestHandler<Command>
{
    private readonly IPostRepository _repository;
    private readonly IDatabaseTransactionFactory _transactionFactory;

    public CommandHandler(IPostRepository repository, IDatabaseTransactionFactory transactionFactory)
    {
        _repository = repository;
        _transactionFactory = transactionFactory;
    }
    
    public async Task Handle(Command request, CancellationToken cancellationToken)
    {
        await using var transaction = await _transactionFactory.BeginTransactionAsync();
        try
        {
            var post = await _repository.ReadPostFromIdAsync(request.PostId, transaction, includeComments:true);
            post.AddComment(request.UserId, request.Comment);
            await _repository.UpdateAsync(post, transaction);
        }
        catch (Exception e)
        {
            await transaction.RollbackTransactionAsync();
            throw;
        }
    }
}