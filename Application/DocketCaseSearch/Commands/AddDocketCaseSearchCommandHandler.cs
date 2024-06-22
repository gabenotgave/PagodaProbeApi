using Application.Data;
using MediatR;

namespace Application.DocketCaseSearch.Commands;

public class AddDocketCaseSearchCommandHandler : IRequestHandler<AddDocketCaseSearchCommand, Unit>
{
    private readonly IApplicationDbContext _applicationDbContext;
    
    public AddDocketCaseSearchCommandHandler(IApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }
    
    public async Task<Unit> Handle(AddDocketCaseSearchCommand request, CancellationToken cancellationToken)
    {
        Domain.DataModels.DocketCaseSearch docketCaseSearch = new Domain.DataModels.DocketCaseSearch
        {
            DocketNumber = request.DocketCase.DocketNumber,
            DateSearched = DateTime.Now,
            IpAddress = request.IpAddress
        };

        _applicationDbContext.DocketCaseSearches.Add(docketCaseSearch);
        await _applicationDbContext.SaveChangesAsync();

        return Unit.Value;
    }
}