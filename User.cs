using System.Linq;
using System.Transactions;
using Newtonsoft.Json;

namespace Finals;

public class User
{
    [JsonProperty("FirstName")]
    private string FirstName { get; set; }
    [JsonProperty("LastName")]
    private string LastName { get; set; }
    [JsonProperty("GELBalance")]
    private decimal GELBalance { get; set; }
    [JsonProperty("USDBalance")]
    private decimal USDBalance { get; set; }
    [JsonProperty("EURBalance")]
    private decimal EURBalance { get; set; }
    [JsonProperty("CardDetails")]
    private CardDetails CardDetails { get; set; }
    [JsonProperty("TransactionHistory")]
    
    private List<Transactions> TransactionHistory { get; set; }

    public User(string firstName, string lastName, CardDetails cardDetails)
    {
        FirstName = firstName;
        LastName = lastName;
        GELBalance = 0;
        USDBalance = 0;
        EURBalance = 0;
        CardDetails = cardDetails;
        TransactionHistory = new List<Transactions>();
    }

    public void ChangePinCode()
    {
        Console.WriteLine("Enter the new pin code: ");
        string newPinCode = Console.ReadLine();
        string oldPinCode = this.CardDetails.getPinCode();
        this.CardDetails.setPinCode(newPinCode);
        string userJson = JsonConvert.SerializeObject(this);
        File.WriteAllText(Globals.jsonFilePath, userJson);
        Console.WriteLine($"Your pin code has changed from {oldPinCode} to {this.CardDetails.getPinCode()}");
    }
    public void GetLastFiveTransactions()
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
                Console.WriteLine(String.Format("{0,-15} {1,-10} {2,-15} {3,-15} {4,-15}", transaction.GetTransactionDate().ToShortDateString(), transaction.GetTransactionType(), transaction.GetAmountGEL(), transaction.GetAmountUSD(), transaction.GetAmountEUR()));
            }
            Console.WriteLine();
        }
    }
    public decimal GetGELBalance()
    {
        return this.GELBalance;
    }
    public decimal GetUSDBalance()
    {
        return this.USDBalance;
    }
    public decimal GetEURBalance()
    {
        return this.EURBalance;
    }
    public bool CheckCardGeneralInformation(string cardNumber, string expirationDate)
    {
        if (cardNumber == this.CardDetails.getCardNumber() && expirationDate == this.CardDetails.getExpirationDate())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CheckCardPinCode(string pinCode)
    {
        if (pinCode == this.CardDetails.getPinCode())
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void CashOut()
    {
        Console.WriteLine("Enter the amount of money you want to cash out:");
        string input = Console.ReadLine();

        Console.WriteLine("Enter the currency type you want to cash in, such as GEL, USD, or EUR:");
        string currencyType = Console.ReadLine().ToUpper();
        
        decimal amountGEL = 0;
        decimal amountUSD = 0;
        decimal amountEUR = 0;
        decimal amount;
        if (decimal.TryParse(input, out amount) && amount > 0)
        {
            if (currencyType == "GEL" && amount <= this.GELBalance)
            {
                amountGEL = amount;
                this.GELBalance -= amountGEL;
                Console.WriteLine($"You have successfully cashed out {amountGEL} GEL.");
            } else if (currencyType == "USD" && amount <= this.USDBalance)
            {
                amountUSD = amount;
                this.USDBalance -= amountUSD;
                Console.WriteLine($"You have successfully cashed out {amountUSD} USD.");
            } else if (currencyType == "EUR" && amount <= this.EURBalance)
            {
                amountEUR = amount;
                this.EURBalance -= amountEUR;
                Console.WriteLine($"You have successfully cashed out {amountEUR} EUR.");
            }

            Transactions transaction = new Transactions(DateTime.Now, "Cash out", amountGEL, amountUSD, amountEUR);

            this.TransactionHistory.Add(transaction);
            string userJson = JsonConvert.SerializeObject(this);

            File.WriteAllText("user.json", userJson);

            Console.WriteLine($"Your new balance is {this.GELBalance} GEL, {this.USDBalance} USD, {this.EURBalance} EUR.");
        }
        else
        {
            Console.WriteLine("Invalid input. Please enter a positive decimal number that is less than or equal to your balance.");
        }
    }
    public void CashIn()
    {
        Console.WriteLine("Enter the currency type you want to cash in, such as GEL, USD, or EUR:");
        string currencyType = Console.ReadLine().ToUpper();

        if (currencyType == "GEL" || currencyType == "USD" || currencyType == "EUR")
        {
            Console.WriteLine($"Enter the amount of money you want to cash in, in {currencyType}:");
            string input = Console.ReadLine();

            decimal amount;
            decimal amountGEL = 0;
            decimal amountUSD = 0;
            decimal amountEUR = 0;
            if (decimal.TryParse(input, out amount) && amount > 0)
            {
                if (currencyType == "GEL")
                {
                    amountGEL = amount;
                    this.GELBalance += amountGEL;
                } else if (currencyType == "USD")
                {
                    amountUSD = amount;
                    this.USDBalance += amountUSD;
                }
                else
                {
                    amountEUR = amount;
                    this.EURBalance += amountEUR;
                }

                Transactions transaction = new Transactions(DateTime.Now, "Cash in", amountGEL, amountUSD, amountEUR);

                this.TransactionHistory.Add(transaction);
                string userJson = JsonConvert.SerializeObject(this);
                File.WriteAllText(Globals.jsonFilePath, userJson);
                Console.WriteLine($"Your new balance is {this.GELBalance} GEL, {this.USDBalance} USD, {this.EURBalance} EUR.");
            }
            else
            {
                Console.WriteLine("Invalid number. Please enter the positive decimal!");
            }
        }
        else
        {
            Console.WriteLine("Ivalind Currency!");
        }
    }
    
    public void Convert()
    {
        Console.WriteLine($"Your current balance is {this.GELBalance} GEL, {this.USDBalance} USD, {this.EURBalance} EUR.");
        Console.WriteLine("Choose from what currency you want convert (GEL/USD/EUR):");
        string original = Console.ReadLine().ToUpper();
        Console.WriteLine("Choose desired currency (GEL/USD/EUR):");
        string desired = Console.ReadLine().ToUpper();
        
        Console.WriteLine("Enter the amount: ");
        string amount = Console.ReadLine();
        
        decimal money;
        
        decimal.TryParse(amount, out money);
        
        decimal convertedAmount = 0m;

        try
        {
            switch (original + desired)
            {
                case "GELUSD":
                    convertedAmount = money * 0.30m;
                    if (GELBalance < money)
                    {
                        throw new Exception("Insufficient funds.");
                    }
                    GELBalance -= money;
                    USDBalance += convertedAmount;
                    Transactions transaction = new Transactions(DateTime.Now, "Convert", money, convertedAmount, 0);
                    TransactionHistory.Add(transaction);
                    break;
                case "GELEUR":
                    convertedAmount = money * 0.25m;
                    if (GELBalance < money)
                    {
                        throw new Exception("Insufficient funds.");
                    }
                    GELBalance -= money;
                    EURBalance += convertedAmount;
                    transaction = new Transactions(DateTime.Now, "Convert", money, convertedAmount, 0);
                    TransactionHistory.Add(transaction);
                    break;
                case "USDGEL":
                    convertedAmount = money * 3.33m;
                    if (USDBalance < money)
                    {
                        throw new Exception("Insufficient funds.");
                    }
                    USDBalance -= money;
                    GELBalance += convertedAmount;
                    transaction = new Transactions(DateTime.Now, "Convert", convertedAmount, money, 0);
                    TransactionHistory.Add(transaction);
                    break;
                case "USDEUR":
                    convertedAmount = money  * 0.85m;
                    if (USDBalance < money)
                    {
                        throw new Exception("Insufficient funds.");
                    }
                    USDBalance -= money;
                    EURBalance += convertedAmount;
                    transaction = new Transactions(DateTime.Now, "Convert", 0, money, convertedAmount);
                    TransactionHistory.Add(transaction);
                    break;
                case "EURGEL":
                    convertedAmount = money * 4.00m;
                    if (EURBalance < money)
                    {
                        throw new Exception("Insufficient funds.");
                    }
                    EURBalance -= money;
                    GELBalance += convertedAmount;
                    transaction = new Transactions(DateTime.Now, "Convert", convertedAmount, 0, money);
                    TransactionHistory.Add(transaction);
                    break;
                case "EURUSD":
                    convertedAmount = money * 1.18m;
                    if (EURBalance < money)
                    {
                        throw new Exception("Insufficient funds.");
                    }
                    EURBalance -= money;
                    USDBalance += convertedAmount;
                    transaction = new Transactions(DateTime.Now, "Convert", 0, convertedAmount, money);
                    TransactionHistory.Add(transaction);
                    break;
                default:
                    throw new Exception("Invalid currency types.");
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        
        string userJson = JsonConvert.SerializeObject(this);
        File.WriteAllText(Globals.jsonFilePath, userJson);
        Console.WriteLine("Successful convertion of funds!");
        Console.WriteLine($"Your current balance is {this.GELBalance} GEL, {this.USDBalance} USD, {this.EURBalance} EUR.");
    }
}