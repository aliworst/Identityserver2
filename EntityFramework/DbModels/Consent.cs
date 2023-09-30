using EntityFramework.Enumrations;
using Microsoft.EntityFrameworkCore;

namespace EntityFramework.DbModels;

public class Consent
{
    public string Id { get; set; }
    public string ClientId { get; set; }
    public long SubjectId { get; set; }
    public long AccountNumber { get; set; }
    public DateTimeOffset CreationTime { get; set; }
    public DateTime Expiration { get; set; }
    public DateTime ConsumedTime { get; set; }
    public string? Scope { get; set; }
    public string? Type { get; set; } 
    public DateTime LastUpdateTime { get; set; }
    public string Parameters { get; set; }
    public Status Status { get; set; }
}

// [Keyless]
// public class Parameters
// {
//     public long MaximumDailyTransferAmount { get; set; }
//     public long MaximumDailyTransactionCount { get; set; }
//     public long MaximumAmountPerTransfer { get; set; }
//     public List<string> Creditors { get; set; }
// }