using BusinessRule.Services;
using EntityFramework.DbContexts;
using EntityFramework.Repositories;
using Identityserver.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Identityserver.Program;

public static partial class ServiceInitializer
{
    

 

    public static IServiceCollection RegisterApplicationServices(this IServiceCollection services)
    {
        RegisterSwagger(services);
        services.AddControllers();
        RegisterIdentityServer(services);
        RegisterCustomDependencies(services);
        return services;
    }
    private static void RegisterCustomDependencies(IServiceCollection services)
    {
        services.AddScoped<IClientService, ClientService>();
        services.AddTransient<IUsersServices, UsersServices>();
        services.AddScoped<IConsentServices , ConsentServices>();
        services.AddScoped<IClientsRepository, ClientsRepository>();
        services.AddTransient<IUserRepository, UserRepository>();
        services.AddScoped<IConsentRepository, ConsentRepository>();
    }
    private static void RegisterHttpClientDependencies(IServiceCollection services)
    {
        
    }
    private static void RegisterSwagger(IServiceCollection services)
    {
        
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }

    private static void RegisterIdentityServer(IServiceCollection services)
    {

        var assembly = typeof(global::Program).Assembly.GetName().Name;
        
        services.AddDbContext<IAdminDbContext, AdminDbContexts>(options =>
            options.UseSqlite("Data Source=C:/Users/a.norozi/RiderProjects/Identityserver/Identityserver/IdentityServer.db",
                b => b.MigrationsAssembly(assembly)));
        
        services.AddDbContext<UserDbContext>(options =>
            options.UseSqlite("Data Source=C:/Users/a.norozi/RiderProjects/Identityserver/Identityserver/IdentityServer.db",
                b => b.MigrationsAssembly(assembly)));

        services.AddDbContext<ConsentDbContext>(options =>
            options.UseSqlite(
                "Data Source=C:/Users/a.norozi/RiderProjects/Identityserver/Identityserver/IdentityServer.db",
                b => b.MigrationsAssembly(assembly)));
        services.AddIdentityServer(options =>
            {
                options.UserInteraction.ConsentUrl = "/Authorize";
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseSuccessEvents = true;
                options.EmitStaticAudienceClaim = true;
            })
            .AddConfigurationStore(options =>
            {
                options.ConfigureDbContext = b =>
                    b.UseSqlite(
                        "Data Source=C:/Users/a.norozi/RiderProjects/Identityserver/Identityserver/IdentityServer.db",
                        opt
                            => opt.MigrationsAssembly(assembly));
            })
            .AddOperationalStore(options =>
            {
                options.ConfigureDbContext = b =>
                    b.UseSqlite(
                        "Data Source=C:/Users/a.norozi/RiderProjects/Identityserver/Identityserver/IdentityServer.db",
                        opt
                            => opt.MigrationsAssembly(assembly));
                options.EnableTokenCleanup = true;
                options.TokenCleanupInterval = 3600; // e.g. cleanup once an hour
            })
            .AddInMemoryPersistedGrants();
        
    }
}