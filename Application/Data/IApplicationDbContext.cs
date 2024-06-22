using Domain.DataModels;
using Microsoft.EntityFrameworkCore;

namespace Application.Data;

public interface IApplicationDbContext
{
    public DbSet<Domain.DataModels.Search> Searches { get; set; }
    public DbSet<Domain.DataModels.ContactInquiry> ContactInquiries { get; set; }
    public DbSet<Domain.DataModels.DocketCaseSearch> DocketCaseSearches { get; set; }
    public DbSet<DesktopAppRelease> DesktopAppReleases { get; set; }

    public Task<int> SaveChangesAsync();
}