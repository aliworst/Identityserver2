namespace DataAccess.RequestModel;

public class ConsentDto
{
    public string ClientId { get; set; }
    public List<string> Scopes { get; set; }
    public DateTime ExpirationDate { get; set; }
    public long MaximumDailyTransferAmount { get; set; }
    public long MaximumDailyTransactionCount { get; set; }
    public long MaximumAmountPerTransfer { get; set; }
    public List<string> CreditorAccounts { get; set; }
}