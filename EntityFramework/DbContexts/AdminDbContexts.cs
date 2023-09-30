using Duende.IdentityServer.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;

namespace EntityFramework.DbContexts;

public class AdminDbContexts : DbContext , IAdminDbContext 
{
    public AdminDbContexts(DbContextOptions<AdminDbContexts> options)
        : base(options)
    {
        
    }

    public DbSet<Client> Clients { get; set; }
    public DbSet<ClientCorsOrigin> ClientCorsOrigins { get; set; }
    
    public DbSet<IdentityResource> IdentityResources { get; set; }
    
    public DbSet<ApiResource> ApiResources { get; set; }
    
    public DbSet<ApiScope> ApiScopes { get; set; }
    
    public DbSet<IdentityProvider> IdentityProviders { get; set; }
    
    public DbSet<ApiResourceSecret> ApiSecrets { get; set; }
    
    public DbSet<ApiScopeClaim> ApiScopeClaims { get; set; }
    
    public DbSet<IdentityResourceClaim> IdentityClaims { get; set; }
    
    public DbSet<ApiResourceClaim> ApiResourceClaims { get; set; }
    public DbSet<ClientGrantType> ClientGrantTypes { get; set; }
    public DbSet<ClientScope> ClientScopes { get; set; }
    
    public DbSet<ClientSecret> ClientSecrets { get; set; }
    public DbSet<ClientPostLogoutRedirectUri> ClientPostLogoutRedirectUris { get; set; }
    public DbSet<ClientIdPRestriction> ClientIdPRestrictions { get; set; }
    public DbSet<ClientRedirectUri> ClientRedirectUris { get; set; }
    public DbSet<ClientClaim> ClientClaims { get; set; }
    public DbSet<ClientProperty> ClientProperties { get; set; }
    
    public DbSet<IdentityResourceProperty> IdentityResourceProperties { get; set; }
    
    public DbSet<ApiResourceProperty> ApiResourceProperties { get; set; }
    
    public DbSet<ApiScopeProperty> ApiScopeProperties { get; set; }
    
    public DbSet<ApiResourceScope> ApiResourceScopes { get; set; }
}