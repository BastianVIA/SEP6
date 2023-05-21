﻿using Backend.Database.TransactionManager;
using Backend.SocialFeed.Domain;
using Backend.SocialFeed.Infrastructure;
using Backend.User.Domain;
using MediatR;

namespace Backend.SocialFeed.EventHandler.IntegrationEvents.UserEvents;

public class CreatedReviewEventHandler : INotificationHandler<CreateReviewEvent>
{
    private readonly IDatabaseTransactionFactory _transactionFactory;
    private readonly IPostRepository _repository;

    public CreatedReviewEventHandler(IDatabaseTransactionFactory transactionFactory, IPostRepository repository)
    {
        _transactionFactory = transactionFactory;
        _repository = repository;
    }
    
    public async Task Handle(CreateReviewEvent notification, CancellationToken cancellationToken)
    {

        var transaction = _transactionFactory.GetCurrentTransaction();

        try
        {
            var activityData = new ActivityData
            {
                MovieId = notification.Review.MovieId,
                ReviewBody = notification.Review.ReviewBody
            };
            var newPost = new Post(notification.UserId, Activity.CreatedReview, activityData);
            await _repository.CreatePostAsync(newPost, transaction);
        }
        catch (Exception e)
        {
            await transaction.RollbackTransactionAsync();
            throw;
        }
        
    }
}