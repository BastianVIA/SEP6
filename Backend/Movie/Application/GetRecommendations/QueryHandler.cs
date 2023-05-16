﻿using Backend.Movie.Infrastructure;
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

public record MovieRecommendationRatingDto(float AverageRating, int Votes);

public class QueryHandler : IRequestHandler<Query, MovieRecommendationsResponse>
{
    private const int MinVotesBeforeRecommending = 100;
    private const float MinRatingBeforeRecommending = 7f;
    
    private readonly IMovieRepository _repository;
    private readonly IImageService _imageService;

    public QueryHandler(IMovieRepository repository, IImageService imageService)
    {
        _repository = repository;
        _imageService = imageService;
    }

    public async Task<MovieRecommendationsResponse> Handle(Query request, CancellationToken cancellationToken)
    {
        var movies = await _repository.GetRecommendedMovies(MinVotesBeforeRecommending, MinRatingBeforeRecommending);
        var dtoMovies = new List<MovieRecommendationDto>();
        foreach (var domainMovie in movies)
        {
            var posterForMovie = _imageService.GetPathForPoster(domainMovie.Id);
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