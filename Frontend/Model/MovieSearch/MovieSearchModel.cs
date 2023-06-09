﻿using Frontend.Entities;
using Frontend.Network;
using Frontend.Network.MovieSearch;
using Frontend.Service;

namespace Frontend.Model.MovieSearch
{
    public class MovieSearchModel : IMovieSearchModel
    {
            private IMovieSearchClient _client;
            
            public MovieSearchModel(IMovieSearchClient client)
            {
                _client = client;
            }

            public async Task<List<Movie>> SearchForMovieAsync(string title, MovieSortingKey? movieSortingKey = null, SortingDirection? sortingDirection = null, int? pageNumber = null)
            {
                return await _client.SearchForMovieAsync(title, movieSortingKey, sortingDirection, pageNumber);
            }
    }
}



