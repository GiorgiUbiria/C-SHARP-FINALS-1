using System.Transactions;

namespace Finals;

public class User
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public CardDetails CardDetails { get; set; }
    public List<Transaction> TransactionHistory { get; set; }
}