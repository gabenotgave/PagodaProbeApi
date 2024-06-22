namespace Application.ApiClients.StateDocketSearches;

public interface IPennsylvaniaDocketCaseSearchApiClient : IDocketCaseSearchApiClient
{
    public new Task<List<Domain.Entities.DocketCase>> GetDocketCasesByPerson(Domain.Entities.Person person);
    public new Task<Domain.Entities.DocketCase> GetDocketCaseByNumber(string docketNumber);
}