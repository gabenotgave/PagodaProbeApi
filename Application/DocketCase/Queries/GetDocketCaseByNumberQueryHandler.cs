using Application.ApiClients.StateDocketSearches;
using Domain.Entities;
using MediatR;

namespace Application.DocketCase.Queries;

public class GetDocketCaseByNumberQueryHandler : IRequestHandler<GetDocketCaseByNumberQuery, Domain.Entities.DocketCase>
{
    private readonly IPennsylvaniaDocketCaseSearchApiClient _pennsylvaniaDocketCaseSearchApiClient;

    public GetDocketCaseByNumberQueryHandler(
        IPennsylvaniaDocketCaseSearchApiClient pennsylvaniaDocketCaseSearchApiClient
        )
    {
        _pennsylvaniaDocketCaseSearchApiClient = pennsylvaniaDocketCaseSearchApiClient;
    }

    public async Task<Domain.Entities.DocketCase> Handle(GetDocketCaseByNumberQuery request, CancellationToken cancellationToken)
    {
        var docketCase = await _pennsylvaniaDocketCaseSearchApiClient.GetDocketCaseByNumber(request.DocketNumber);

        return docketCase;
    }
}