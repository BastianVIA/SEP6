﻿using Backend.Database.TransactionManager;
using Backend.Movie.Application.GetInfoFromMovies;
using Backend.User.Infrastructure;
using MediatR;

namespace Backend.User.Application.GetFavoriteMovies;

public record Query(string userId) : IRequest<FavoriteMovesResponse>;

public record FavoriteMovesResponse(List<FavoriteMovieDto> movies);

public class FavoriteMovieDto
{
    public string Title { get; set; }
    public string Id { get; set; }
    public int ReleaseYear { get; set; }
    public Uri? PathToPoster { get; set; }
    public FavoriteMovieRatingDto? Rating { get; set; }
}

public record FavoriteMovieRatingDto(double AverageRating, int Votes);

public class QueryHandler : IRequestHandler<Query, FavoriteMovesResponse>
{
    private readonly IUserRepository _repository;
    private readonly IMediator _mediator;
    private readonly IDatabaseTransactionFactory _databaseTransactionFactory;

    public QueryHandler(IUserRepository repository, IMediator mediator, IDatabaseTransactionFactory databaseTransactionFactory)
    {
        _repository = repository;
        _mediator = mediator;
        _databaseTransactionFactory = databaseTransactionFactory;
    }

    public async Task<FavoriteMovesResponse> Handle(Query request, CancellationToken cancellationToken)
    {
        var transaction = _databaseTransactionFactory.BeginReadOnlyTransaction();
        var userRequested = await _repository.ReadUserFromIdAsync(request.userId,transaction, includeFavoriteMovies:true);
        var moviesInfoResponse =await _mediator.Send(new Movie.Application.GetInfoFromMovies.Query(userRequested.FavoriteMovies));
        var movieDtos = new List<FavoriteMovieDto>();
        foreach (var movieInfo in  moviesInfoResponse.MovieInfoDtos)
        {
            movieDtos.Add(toDto(movieInfo));
        }

        return new FavoriteMovesResponse(movieDtos);
    }

    private FavoriteMovieDto toDto(MovieInfoDto movieInfoDto)
    {
        var favMovie = new FavoriteMovieDto
        {
            Id = movieInfoDto.Id,
            Title = movieInfoDto.Title,
            ReleaseYear = movieInfoDto.ReleaseYear,
            PathToPoster = movieInfoDto.PathToPoser
        };
        if (movieInfoDto.Rating != null)
        {
            favMovie.Rating = new FavoriteMovieRatingDto(movieInfoDto.Rating.AverageRating, movieInfoDto.Rating.Votes);
        }
        
        return favMovie;
    }
   
}