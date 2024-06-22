using MediatR;

namespace Application.DocketCase.Queries;

public class GetDocketCaseByNumberQuery : IRequest<Domain.Entities.DocketCase>
{
    public required string DocketNumber { get; set; }
}