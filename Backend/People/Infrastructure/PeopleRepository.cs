using System.Diagnostics;
using Backend.Database.Transaction;
using Backend.People.Domain;
using Backend.Service;
using Microsoft.EntityFrameworkCore;

namespace Backend.People.Infrastructure;

public class PeopleRepository : IPeopleRepository
{
    private const int NumberOfResultsPerPage = 20;

    public async Task<Person> ReadPersonFromIdAsync(string id, DbReadOnlyTransaction tx, bool includeActed = false,
        bool includeDirected = false)
    {
        var result = await tx.DataContext.People
            .Where(p => p.Id == id)
            .FirstOrDefaultAsync();
        
        if (result == null)
        {
            throw new KeyNotFoundException($"Could not find person with id: {id}");
        }
        
        if (includeActed)
        {
            await tx.DataContext.Entry(result)
                .Collection(p => p.ActedMovies)
                .LoadAsync();
        }

        if (includeDirected)
        {
            await tx.DataContext.Entry(result)
                .Collection(p => p.DirectedMovies)
                .LoadAsync();
        }

        return ToDomain(result);
    }

    public async Task<List<Person>> SearchForPersonAsync(string name, int requestPageNumber, DbReadOnlyTransaction tx)
    {
        var query = tx.DataContext.People.Where(p => EF.Functions.Like(p.Name, $"%{name}%"));

        Task<List<PeopleDAO>> foundPersons = query
            .Skip(NumberOfResultsPerPage * (requestPageNumber - 1))
            .Take(NumberOfResultsPerPage)
            .ToListAsync();

        return ToDomain(await foundPersons);
    }

    public async Task<List<Person>> FindPersonsAsync(List<string> personIds, DbReadOnlyTransaction tx)
    {
        var query = tx.DataContext.People.Where(p => personIds.Contains(p.Id)).ToList();

        return ToDomain(query);
    }

    private List<Domain.Person> ToDomain(List<PeopleDAO> personDaos)
    {
        var listOfDomainPersons = new List<Domain.Person>();
        foreach (var personDao in personDaos)
        {
            listOfDomainPersons.Add(ToDomain(personDao));
        }

        return listOfDomainPersons;
    }

    private Person ToDomain(PeopleDAO peopleDao)
    {
        return new Person
        {
            BirthYear = peopleDao.BirthYear,
            Id = peopleDao.Id,
            ImdbId = peopleDao.ImdbId,
            Name = peopleDao.Name,
            ActedMoviesId = ToDomain(peopleDao.ActedMovies),
            DirectedMoviesId = ToDomain(peopleDao.DirectedMovies)
        };
    }

    private ICollection<string>? ToDomain(ICollection<PeopleMovieDAO>? personDaoActedMovies)
    {
        if (personDaoActedMovies == null || personDaoActedMovies.Count == 0)
        {
            return null;
        }

        var listOfMovieIds = new List<string>();
        foreach (var movie in personDaoActedMovies)
        {
            listOfMovieIds.Add(movie.MovieId);
        }

        return listOfMovieIds;
    }
}