﻿using Backend.Database.TransactionManager;
using Backend.Social.Domain;
using Backend.Social.Infrastructure;
using MediatR;

namespace Backend.Social.Application.ReactToPost;

public record Command(string PostId, string UserId, TypeOfReaction Reaction) : IRequest;



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
            var post = await _repository.ReadPostFromIdAsync(request.PostId, transaction, includeReactions:true);
            post.PutReaction(request.UserId, request.Reaction);
            await _repository.UpdateAsync(post, transaction);
        }
        catch (Exception e)
        {
            await transaction.RollbackTransactionAsync();
            throw;
        }
    }
}