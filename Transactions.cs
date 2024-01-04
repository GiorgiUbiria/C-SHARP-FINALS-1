using Newtonsoft.Json;

namespace Finals;

public class Transactions
{
    [JsonProperty("TransactionDate")]
    public DateTime TransactionDate { get; set; }
    [JsonProperty("TransactionType")]
    public string TransactionType { get; set; }
    [JsonProperty("AmountGEL")]
    public decimal AmountGEL { get; set; }
    [JsonProperty("AmountUSD")]
    public decimal AmountUSD { get; set; }
    [JsonProperty("AmountEUR")]
    public decimal AmountEUR { get; set; }

    public Transactions(DateTime transactionDate, string transactionType, decimal amountGel, decimal amountUsd, decimal amountEur)
    {
        TransactionDate = transactionDate;
        TransactionType = transactionType;
        AmountGEL = amountGel;
        AmountUSD = amountUsd;
        AmountEUR = amountEur;
    }
}