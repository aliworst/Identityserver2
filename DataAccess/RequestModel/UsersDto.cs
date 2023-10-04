
namespace DataAccess.RequestModel;

public class UsersDto 
{
    public string? Username { get; set; }
    public string Forename { get; set; }
    public string Surname { get; set; }
    public long AccountNumber { get; set; }
    public bool LockoutEnabled { get; set; }
    public bool TwoFactorEnabled { get; set; }
    public int AccessFailedCount { get; set; }
    public DateTimeOffset? LockoutEnd { get; set; }
}