using Application.Person.Queries;
using Domain.Entities;

namespace Application.ApiClients.CountyParcelSearches;

public interface IBerksParcelSearchApiClient : IParcelSearchApiClient
{
    public new Task<List<Parcel>> GetParcelsByPersonName(GetPersonByNameQuery person);
}