using Application.Data;
using Domain.DataModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }

        public DbSet<Search> Searches { get; set; }
        public DbSet<ContactInquiry> ContactInquiries { get; set; }
        public DbSet<DocketCaseSearch> DocketCaseSearches { get; set; }
        public DbSet<DesktopAppRelease> DesktopAppReleases { get; set; }
        
        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }
    }
}
