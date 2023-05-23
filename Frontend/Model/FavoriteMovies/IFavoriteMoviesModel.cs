﻿using Frontend.Entities;

namespace Frontend.Model.FavoriteMovies;

public interface IFavoriteMoviesModel
{
    Task<IList<Movie>> GetFavoriteMovies(string userToken, string UID);
  Task AddToFavoriteMovies(string bearerToken, string movieId);

  Task RemoveFromFavoriteMovies(string tokenValue, string movieId);
}