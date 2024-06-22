using MediatR;

namespace Application.Search.Commands;

public class AddSearchCommand : IRequest<Unit>
{
    public required Domain.Entities.Person Person { get; set; }
    
    public required string IpAddress { get; set; }
}