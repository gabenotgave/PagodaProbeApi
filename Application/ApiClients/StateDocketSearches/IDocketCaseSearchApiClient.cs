namespace Application.ApiClients.StateDocketSearches;

public interface IDocketCaseSearchApiClient
{
    public Task<List<Domain.Entities.DocketCase>> GetDocketCasesByPerson(Domain.Entities.Person person);
    public Task<Domain.Entities.DocketCase> GetDocketCaseByNumber(string docketNumber);
}