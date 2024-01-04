namespace Finals;

public class Transactions
{
    public DateTime TransactionDate { get; set; }
    public string TransactionType { get; set; }
    public decimal AmountGEL { get; set; }
    public decimal AmountUSD { get; set; }
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