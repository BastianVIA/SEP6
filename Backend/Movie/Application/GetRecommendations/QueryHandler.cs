﻿using Backend.Database.TransactionManager;
using Backend.Movie.Infrastructure;
using Backend.Service;
using MediatR;

namespace Backend.Movie.Application.GetRecommendations;

public record Query() : IRequest<MovieRecommendationsResponse>;

public record MovieRecommendationsResponse(List<MovieRecommendationDto> MovieRecommendations);

public class MovieRecommendationDto
{
    public string Title { get; set; }
    public string Id { get; set; }
    public int ReleaseYear { get; set; }
    public Uri? PathToPoster { get; set; }
    public MovieRecommendationRatingDto? Rating { get; set; }
}

public record MovieRecommendationRatingDto(double AverageRating, int Votes);

public class QueryHandler : IRequestHandler<Query, MovieRecommendationsResponse>
{
    private int MinVotesBeforeRecommending;
    private float MinRatingBeforeRecommending;
    
    private readonly IMovieRepository _repository;
    private readonly IImageService _imageService;
    private readonly IDatabaseTransactionFactory _databaseTransactionFactory;

    public QueryHandler(IConfiguration configuration, IMovieRepository repository, IImageService imageService, IDatabaseTransactionFactory databaseTransactionFactory)
    {
        _repository = repository;
        _imageService = imageService;
        _databaseTransactionFactory = databaseTransactionFactory;
        MinRatingBeforeRecommending = configuration.GetSection("QueryConstants").GetValue<float>("MinRating");
        MinVotesBeforeRecommending = configuration.GetSection("QueryConstants").GetValue<int>("MinVotes");
    }

    public async Task<MovieRecommendationsResponse> Handle(Query request, CancellationToken cancellationToken)
    {
        var transaction = _databaseTransactionFactory.BeginReadOnlyTransaction();
        var movies = await _repository.GetRecommendedMoviesAsync(
            MinVotesBeforeRecommending, 
            MinRatingBeforeRecommending, 
            transaction);
        var dtoMovies = new List<MovieRecommendationDto>();
        foreach (var domainMovie in movies)
        {
            var posterForMovie = _imageService.GetPathForPosterAsync(domainMovie.Id);
            var dtoMovie = new MovieRecommendationDto
            {
                Id = domainMovie.Id,
                Title = domainMovie.Title,
                ReleaseYear = domainMovie.ReleaseYear,
                PathToPoster = await posterForMovie
            };
            if (domainMovie.Rating != null)
            {
                dtoMovie.Rating =
                    new MovieRecommendationRatingDto(domainMovie.Rating.AverageRating, domainMovie.Rating.Votes);
            }
            dtoMovies.Add(dtoMovie);
        }

        return new MovieRecommendationsResponse(dtoMovies);
    }
}