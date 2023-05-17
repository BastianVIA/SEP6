﻿using System.ComponentModel.DataAnnotations;
using Backend.Database.TransactionManager;
using Backend.User.Infrastructure;
using MediatR;
using NLog;

namespace Backend.User.Application.SetFavoriteMovie;

public record Command(string userId, string movieId) : IRequest;

public class CommandHandler : IRequestHandler<Command>
{
    private readonly IUserRepository _repository;
    private readonly IMediator _mediator;
    private readonly IDatabaseTransactionFactory _databaseTransactionFactory;

    public CommandHandler(IUserRepository userRepository, IMediator mediator, IDatabaseTransactionFactory databaseTransactionFactory)
    {
        _repository = userRepository;
        _mediator = mediator;
        _databaseTransactionFactory = databaseTransactionFactory;
    }

    public async Task Handle(Command request, CancellationToken cancellationToken)
    {
        await using var transaction = await _databaseTransactionFactory.BeginTransactionAsync();
        try
        {
            var user = await _repository.ReadUserFromIdAsync(request.userId, transaction, includeFavoriteMovies:true);
            if (user.HasAlreadyFavoritedMovie(request.movieId))
            {
                LogManager.GetCurrentClassLogger()
                    .Error(
                        $"User with id: {request.userId}, tired to add: {request.movieId} to favorite list but it is already in the favorite list");
                throw new InvalidDataException($"Movie with Id: {request.movieId} Already In Favorite list");
            }
            user.FavoriteMovies.Add(request.movieId);
            await _repository.Update(user, transaction);
        }
        catch (Exception e)
        {
            await transaction.RollbackTransactionAsync();
            throw;
        }
       
    }
}