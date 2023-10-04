using System.Text;
using BusinessRule.Services;
using EntityFramework.DbContexts;
using EntityFramework.Repositories;
using Identityserver.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Identityserver.Program;

public static partial class ServiceInitializer
{
    

 

    public static IServiceCollection RegisterApplicationServices(this IServiceCollection services)
    {
        RegisterSwagger(services);
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder.WithOrigins("http://localhost:63342") // replace with your client's URL
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });
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
        services.AddScoped<IAuthorizeService , AuthorizeService>();
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
                options.UserInteraction.ConsentUrl = "/Consent";
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
        
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "AlirezaBanking",
                    ValidAudience = "AlirezaBanking",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("-----BEGIN OPENSSH PRIVATE KEY-----\\nb3BlbnNzaC1rZXktdjEAAAAABG5vbmUAAAAEbm9uZQAAAAAAAAABAAABFwAAAAdzc2gtcn\\nNhAAAAAwEAAQAAAQEAx5Z4ZlnHkPC63o6qtj8hsQl5B/18S7WswxM56q99puaYjl4cbd4L\\n0f4h36vVwArY6RaYoWIHAQQwbZsDMDtiZU0j0aKQKV+3OTvNJ2JpqY0cala35JNPeI7Es0\\nqFfSiLp2d2ZvMfV+nJpJqGzE5ckgRh9eqJO9gqbBhaWprGrX61gF9c55h6HnR4mC462Wqg\\n/vIlnXkWTi/jZTe9gcNu1kQOgtAToJZr4eFnHnGleIg6XA+8wF2KI3usYYTsKH4tiOa3EE\\n2FQLx0c70c6626XzpLTo9nxbLL15GQbU2Nc0YjHULQ9cvlDwdz6EZsZIWI+B0Bjp9BKtGc\\nL/aK/+mqGQAAA8gupOSvLqTkrwAAAAdzc2gtcnNhAAABAQDHlnhmWceQ8Lrejqq2PyGxCX\\nkH/XxLtazDEznqr32m5piOXhxt3gvR/iHfq9XACtjpFpihYgcBBDBtmwMwO2JlTSPRopAp\\nX7c5O80nYmmpjRxqVrfkk094jsSzSoV9KIunZ3Zm8x9X6cmkmobMTlySBGH16ok72CpsGF\\npamsatfrWAX1znmHoedHiYLjrZaqD+8iWdeRZOL+NlN72Bw27WRA6C0BOglmvh4WcecaV4\\niDpcD7zAXYoje6xhhOwofi2I5rcQTYVAvHRzvRzrrbpfOktOj2fFssvXkZBtTY1zRiMdQt\\nD1y+UPB3PoRmxkhYj4HQGOn0Eq0Zwv9or/6aoZAAAAAwEAAQAAAQAEuXZxex/oOb/n/hnj\\nD9eCU1EUHqyki3u4Gmpn4xjnAdUL+a+pQh+m1uHXuOo/Di1YRyVRx0+XIebrkUcgCyfRx2\\nOuVgTHlfEbtQ4EJ/uMUeohnO/kGetAVVDUAFLsmtsYxAWKJlMlnO5zAQgT1Kmy+HuWe04d\\npoj1NMEJcq8lSCx8b5bAj+byULuGxfODtokVaHHnqvmNVQZp6EHd8NIOhtL/0y50K7u6OD\\noFMqxA0Xi1+JxUTq4N2yeLgbrXthhV0xqCJ85JFTF2DH8tnKONMfQ74jFNxHu5txtheuzL\\niqX7/Df532GXnKPeuq+mx96CnqVP9JD2Cbd0C/s0kTPJAAAAgQDclz8rkkTu2PE9MFsCXx\\nQGv+/CDiHBDVEHZLIEcgAKokFiwO18Gd9WzwCro5PrWOuVm3F9zAOPzBUNHsr2hPiF9ADZ\\nCjyq+ho6imomAUvKZ2d+Q3XuyYdqT50Y6/YB0cYrdh+5ITh+IZNsnStvMMtWNRz/XTMAMj\\nSbUyxILRGfCAAAAIEA9pTVQBTHrRtSvrFt7QoGheJajpCXjl7q/Au4Fn6RHwuiwmhl1G2T\\n4WsKRejsC2F89T4K8e6RmZyEtefhWX0+mCpBxBVgZgwVUuIfa9KMdqfxpgqyqLX+3hYCfg\\n+H3GeB4h6enNLYmGHCO2DoynPCDhNAozUGjIM5FPTXtyMe8R8AAACBAM82Hanx84k1Wb+0\\nHxZrRWQPP5VFgKDbXyDz2JR/ZcxS5pLYKE2bme92vOg1E9RCw7qz998FOeSOr6nMDFuI4S\\n9KfpwVDraL+qazERzkku+VXxKI4UfsiKrV4kfYfwSUKhgTSlRa8WhYodPAQroC4oK79xzn\\nNACvEyqjna+T5WXHAAAAEnJvb3RAc3J2NzQ2NzgzNTYzMg==\\n-----END OPENSSH PRIVATE KEY-----\\n"))
                };
            });
        
    }
}