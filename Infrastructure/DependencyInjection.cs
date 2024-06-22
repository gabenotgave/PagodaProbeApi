using Application.ApiClients.CountyParcelSearches;
using Application.ApiClients.RecaptchaValidator;
using Infrastructure.ApiClients.CountyParcelSearches;
using Infrastructure.ApiClients.StateDocketSearches;
using Application.ApiClients.StateDocketSearches;
using Application.Data;
using Domain.DataModels;
using Infrastructure.ApiClients.RecaptchaValidation;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, WebApplicationBuilder builder)
    {
        services.AddDbContext<ApplicationDbContext>(optionsAction:options
            => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
        
        // Add Scoped for ApplicationDbContext interface as well (when the interface is called in a
        // constructor, the ApplicationDbContext service created above is injected)
        services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());
        
        services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Lockout.MaxFailedAccessAttempts = 10;
            })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddSignInManager()
            .AddDefaultTokenProviders();
        
        services.AddTransient<IBerksParcelSearchApiClient, BerksParcelSearchApiClient>();
        services.AddTransient<IPennsylvaniaDocketCaseSearchApiClient, PennsylvaniaDocketCaseSearchApiClient>();
        services.AddTransient<IGoogleRecaptchaValidatorApiClient, GoogleRecaptchaValidatorApiClient>();
        
        return services;
    }
}