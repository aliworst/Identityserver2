using Microsoft.AspNetCore.Identity;

namespace EntityFramework.DbModels;

public class Users 
{
    public string Id { get; set; }
    public string UserName { get; set; }
    public bool Enabled { get; set; }
    public long NationalIdentifier { get; set; }
    public string Forename { get; set; }
    public string Surname { get; set; }
    public string PasswordHash { get; set; }
    public bool LockoutEnabled { get; set; }
    public bool TwoFactorEnabled { get; set; }
    public string ConcurrencyStamp { get; set; }
    public string SecurityStamp { get; set; }
    public int AccessFailedCount { get; set; }
    public DateTimeOffset? LockoutEnd { get; set; }
}