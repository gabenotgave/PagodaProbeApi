using MediatR;

namespace Application.DocketCaseSearch.Commands;

public class AddDocketCaseSearchCommand : IRequest<Unit>
{
    public required Domain.Entities.DocketCase DocketCase { get; set; }
    
    public required string IpAddress { get; set; }
}