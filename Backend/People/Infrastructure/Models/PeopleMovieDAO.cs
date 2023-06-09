﻿namespace Backend.People.Infrastructure.Models;

public class PeopleMovieDAO
{
    public string MovieId { get; set; }
    public ICollection<PeopleDAO>? Actors { get; set; }
    public ICollection<PeopleDAO>? Directors { get; set; }
}