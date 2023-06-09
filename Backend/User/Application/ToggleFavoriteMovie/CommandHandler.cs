﻿using Backend.Database.TransactionManager;
using Backend.User.Infrastructure;
using MediatR;

namespace Backend.User.Application.ToggleFavoriteMovie;

public record Command(string userId, string movieId) : IRequest;

public class CommandHandler : IRequestHandler<Command>
{
    private readonly IUserRepository _repository;
    private readonly IDatabaseTransactionFactory _databaseTransactionFactory;

    public CommandHandler(IUserRepository userRepository, IDatabaseTransactionFactory databaseTransactionFactory)
    {
        _repository = userRepository;
        _databaseTransactionFactory = databaseTransactionFactory;
    }

    public async Task Handle(Command request, CancellationToken cancellationToken)
    {
        await using var transaction = await _databaseTransactionFactory.BeginTransactionAsync();
        try
        {
            
            var user = await _repository.ReadUserFromIdAsync(request.userId, transaction,includeFavoriteMovies:true);
            if (user.HasAlreadyFavoritedMovie(request.movieId))
            {
              user.RemoveFavorite(request.movieId);
            }
            else
            {
                user.AddFavoriteMovie(request.movieId);
            }
            await _repository.UpdateAsync(user, transaction);
        }
        catch (Exception e)
        {
            await transaction.RollbackTransactionAsync();
            throw;
        }
       
    }
}