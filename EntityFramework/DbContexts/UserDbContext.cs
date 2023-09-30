using EntityFramework.DbModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EntityFramework.DbContexts;

public class UserDbContext : DbContext
{ 
    public UserDbContext(DbContextOptions<UserDbContext> options)
        : base(options)
    { }
    public DbSet<Users> IdentityUsers { get; set; }
}