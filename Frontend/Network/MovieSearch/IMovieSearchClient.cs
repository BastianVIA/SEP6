﻿using Frontend.Entities;
using Frontend.Service;

namespace Frontend.Network.MovieSearch;

public interface IMovieSearchClient
{
    public Task<List<Movie>> SearchForMovieAsync(string title, MovieSortingKey? movieSortingKey = null,
        SortingDirection2? sortingDirection = null, int? pageNumber = null);
}