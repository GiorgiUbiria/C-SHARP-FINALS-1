using System.Linq;
using System.Transactions;

namespace Finals;

public class User
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int Balance { get; set; }
    public CardDetails CardDetails { get; set; }
    public List<Transactions> TransactionHistory { get; set; }

    public void getLastFiveTransactions()
    {
        var lastFiveTransactions = this.TransactionHistory.TakeLast(5);
        if (!lastFiveTransactions.Any())
        {
            Console.WriteLine("No valid transactions yet!\n");    
        }
        else
        {
            Console.WriteLine($"{"Date",-15} {"Type",-10} {"Amount (GEL)",-15} {"Amount (USD)",-15} {"Amount (EUR)",-15}");
            foreach (var transaction in lastFiveTransactions)
            {
                Console.WriteLine(String.Format("{0,-15} {1,-10} {2,-15} {3,-15} {4,-15}", transaction.TransactionDate.ToShortDateString(), transaction.TransactionType, transaction.AmountGEL, transaction.AmountUSD, transaction.AmountEUR));
            }
            Console.WriteLine();
        }
    }
}