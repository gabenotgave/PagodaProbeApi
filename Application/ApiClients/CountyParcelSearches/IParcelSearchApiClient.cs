using Application.Person.Queries;
using Domain.Entities;

namespace Application.ApiClients.CountyParcelSearches;

public interface IParcelSearchApiClient
{
    public Task<List<Parcel>> GetParcelsByPersonName(GetPersonByNameQuery person);
}