using Newtonsoft.Json;

namespace Finals;

public class Transactions
{
    [JsonProperty("TransactionDate")]
    private DateTime TransactionDate { get; set; }
    [JsonProperty("TransactionType")]
    private string TransactionType { get; set; }
    [JsonProperty("AmountGEL")]
    private decimal AmountGEL { get; set; }
    [JsonProperty("AmountUSD")]
    private decimal AmountUSD { get; set; }
    [JsonProperty("AmountEUR")]
    private decimal AmountEUR { get; set; }

    public Transactions(DateTime transactionDate, string transactionType, decimal amountGel, decimal amountUsd, decimal amountEur)
    {
        TransactionDate = transactionDate;
        TransactionType = transactionType;
        AmountGEL = amountGel;
        AmountUSD = amountUsd;
        AmountEUR = amountEur;
    }

    public DateTime GetTransactionDate()
    {
        return this.TransactionDate;
    }
    public decimal GetAmountGEL()
    {
        return this.AmountGEL;
    }
    public decimal GetAmountUSD()
    {
        return this.AmountUSD;
    }
    
    public decimal GetAmountEUR()
    {
        return this.AmountEUR;
    }

    public string GetTransactionType()
    {
        return this.TransactionType;
    }
}