using Application.Data;
using MediatR;

namespace Application.Search.Commands;

public class AddSearchCommandHandler : IRequestHandler<AddSearchCommand, Unit>
{
    private readonly IApplicationDbContext _applicationDbContext;

    public AddSearchCommandHandler(IApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public async Task<Unit> Handle(AddSearchCommand request, CancellationToken cancellationToken)
    {
        Domain.DataModels.Search search = new Domain.DataModels.Search
        {
            FirstName = request.Person.FirstName,
            LastName = request.Person.LastName,
            Birthdate = request.Person.Birthdate,
            County = request.Person.County,
            ParcelCount = request.Person.Parcels.Count,
            DocketCaseCount = request.Person.DocketCases.Count,
            DateSearched = DateTime.Now,
            IpAddress = request.IpAddress
        };
        _applicationDbContext.Searches.Add(search);
        await _applicationDbContext.SaveChangesAsync();
        
        return Unit.Value;
    }
}