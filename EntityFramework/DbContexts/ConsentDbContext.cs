using EntityFramework.DbModels;
using Microsoft.EntityFrameworkCore;

namespace EntityFramework.DbContexts;

public class ConsentDbContext : DbContext
{
    public ConsentDbContext(DbContextOptions<ConsentDbContext> options)
        : base(options)
    { }
    public DbSet<Consent> Consent { get; set; }
}