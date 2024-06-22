using Application.ApiClients.CountyParcelSearches;
using Application.ApiClients.StateDocketSearches;
using Application.Extensions;
using Application.Helpers;
using Domain.Entities;
using MediatR;

namespace Application.Person.Queries;

public class GetPersonByNameQueryHandler : IRequestHandler<GetPersonByNameQuery, Domain.Entities.Person>
{
    private readonly IBerksParcelSearchApiClient _berksParcelSearchApiClient;
    private readonly IPennsylvaniaDocketCaseSearchApiClient _pennsylvaniaDocketCaseSearchApiClient;
        
    public GetPersonByNameQueryHandler(
        IBerksParcelSearchApiClient berksParcelSearchApiClient,
        IPennsylvaniaDocketCaseSearchApiClient pennsylvaniaDocketCaseSearchApiClient
        )
    {
        _berksParcelSearchApiClient = berksParcelSearchApiClient;
        _pennsylvaniaDocketCaseSearchApiClient = pennsylvaniaDocketCaseSearchApiClient;
    }

    public async Task<Domain.Entities.Person> Handle(GetPersonByNameQuery request, CancellationToken cancellationToken)
    {
        if (!AreaHelper.IsSupportedArea(request.State, request.County))
        {
            throw new ArgumentException("This area is not supported.");
        }
        
        // Sanitize first and last names (remove commas and semicolons because this breaks the Berks parcel search API
        // for whatever reason)
        request.FirstName = request.FirstName.RemoveCommasAndSemicolons();
        request.LastName = request.LastName.RemoveCommasAndSemicolons();
            
        var person = new Domain.Entities.Person()
        {
            FirstName = request.FirstName.Trim().CapitalizeFirstLetter(),
            MiddleInitial = request.MiddleInitial?.Trim().Substring(0, 1)?.CapitalizeFirstLetter(),
            LastName = request.LastName.Trim().CapitalizeFirstLetter(),
            Birthdate = request.BirthDate ?? DateTime.MinValue,
            County = request.County,
            SupportedAreas = AreaHelper.GetSupportedAreas()
        };

        switch (person.County)
        {
            case "Berks":
                person.Parcels = await _berksParcelSearchApiClient.GetParcelsByPersonName(request);
                break;
            default:
                person.Parcels = new List<Parcel>();
                break;
        }

        // Exclude 314 Mountain Bl, Wernersville from parcels
        person.Parcels.RemoveAll(p =>
            p.StreetAddress == "314 MOUNTAIN BL" &&
            p.Municipality == "WERNERSVILLE"
        );

        person.DocketCases = await _pennsylvaniaDocketCaseSearchApiClient.GetDocketCasesByPerson(person);
        
        // Limit results
        person.Parcels = person.Parcels.Take(10).ToList();
        person.DocketCases = person.DocketCases.Take(20).ToList();
        
        return person;
    }
}