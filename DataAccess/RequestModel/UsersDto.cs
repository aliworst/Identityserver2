using System.ComponentModel.DataAnnotations;

namespace DataAccess.RequestModel;

public class UsersDto 
{
    [Required]
    [RegularExpression(@"^[a-zA-Z0-9_@\-\.\+]+$")]
    public string UserName { get; set; }
    [Required]
    [EmailAddress]
    public long NationalIdentifier { get; set; }
    public string Forename { get; set; }
    public string Surname { get; set; }
    public string Password { get; set; }
    public bool LockoutEnabled { get; set; }
    public bool TwoFactorEnabled { get; set; }
    public int AccessFailedCount { get; set; }
    public DateTimeOffset? LockoutEnd { get; set; }
}