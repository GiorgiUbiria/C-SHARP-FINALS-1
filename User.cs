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

    public void SaveUser()
    {
        string userJson = JsonConvert.SerializeObject(this); 
        File.WriteAllText("user.json", userJson);
    }
    
    public void ChangePinCode()
    {
        Console.WriteLine("Enter the new pin code: ");
        string newPinCode = Console.ReadLine();
        
        string oldPinCode = CardDetails.getPinCode();
        
        CardDetails.setPinCode(newPinCode);
        
        SaveUser();
        
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

    public void ShowBalance()
    {
        Console.WriteLine($"Your balance is {GetGELBalance()} GEL, {GetUSDBalance()} USD, {GetEURBalance()} EUR.");
    }
    
    public bool CheckCardGeneralInformation(string cardNumber, string expirationDate, string cvc)
    {
        if (cardNumber == this.CardDetails.getCardNumber() && expirationDate == this.CardDetails.getExpirationDate() && cvc == this.CardDetails.getCvc())
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
        decimal convertedAmount = 0m;

        try
        {
            Console.WriteLine("Enter the currency type you want to cash out, such as GEL, USD, or EUR:");
            string currencyType = Console.ReadLine().ToUpper();

            
            decimal amountGEL = 0;
            decimal amountUSD = 0;
            decimal amountEUR = 0;
            
            switch (currencyType)
            {
                case "GEL":
                    convertedAmount = GELBalance;
                    while (true)
                    {
                        Console.WriteLine("Enter the amount of money you want to cash out, in GEL:");
                        string input = Console.ReadLine();

                        if (decimal.TryParse(input, out amountGEL) && amountGEL > 0 && amountGEL <= GELBalance)
                        {
                            GELBalance -= amountGEL;

                            Transactions transaction = new Transactions(DateTime.Now, "Cash out", amountGEL, 0, 0);

                            TransactionHistory.Add(transaction);

                            SaveUser();
                            
                            Console.WriteLine($"You have successfully cashed out {amountGEL} GEL.");
                            
                            ShowBalance();

                            break;
                        }
                        else
                        {
                            Console.WriteLine("Invalid input. Please enter a positive decimal number that is less than or equal to your balance in GEL.");
                        }
                    }
                    break;
                case "USD":
                    convertedAmount = USDBalance;
                    while (true)
                    {
                        Console.WriteLine("Enter the amount of money you want to cash out, in USD:");
                        string input = Console.ReadLine();

                        if (decimal.TryParse(input, out amountUSD) && amountUSD > 0 && amountUSD <= USDBalance)
                        {
                            USDBalance -= amountUSD;

                            Transactions transaction = new Transactions(DateTime.Now, "Cash out", 0, amountUSD, 0);

                            TransactionHistory.Add(transaction);
                            
                            SaveUser();

                            Console.WriteLine($"You have successfully cashed out {amountUSD} USD.");

                            ShowBalance();
                            
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Invalid input. Please enter a positive decimal number that is less than or equal to your balance in USD.");
                        }
                    }
                    break;
                case "EUR":
                    convertedAmount = EURBalance;
                    while (true)
                    {
                        Console.WriteLine("Enter the amount of money you want to cash out, in EUR:");
                        string input = Console.ReadLine();

                        if (decimal.TryParse(input, out amountEUR) && amountEUR > 0 && amountEUR <= EURBalance)
                        {
                            EURBalance -= amountEUR;

                            Transactions transaction = new Transactions(DateTime.Now, "Cash out", 0, 0, amountEUR);

                            TransactionHistory.Add(transaction);

                            SaveUser();

                            Console.WriteLine($"You have successfully cashed out {amountEUR} EUR.");
                            
                            ShowBalance();
                            
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Invalid input. Please enter a positive decimal number that is less than or equal to your balance in EUR.");
                        }
                    }
                    break;
                default:
                    throw new Exception("Invalid currency type. Please enter GEL, USD, or EUR.");
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
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
                    GELBalance += amountGEL;
                } else if (currencyType == "USD")
                {
                    amountUSD = amount;
                    USDBalance += amountUSD;
                }
                else
                {
                    amountEUR = amount; EURBalance += amountEUR;
                }

                Transactions transaction = new Transactions(DateTime.Now, "Cash in", amountGEL, amountUSD, amountEUR);
                TransactionHistory.Add(transaction);
                
                SaveUser();
                
                ShowBalance();
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
        ShowBalance();
        
        Console.WriteLine("Choose from what currency you want convert (GEL/USD/EUR):");
        string original = GetValidCurrencyInput();
        
        Console.WriteLine("Choose desired currency (GEL/USD/EUR):");
        string desired = GetValidCurrencyInput();
        
        decimal convertedAmount = 0m;
        
        try
        {
            switch (original + desired)
            {
                case "GELUSD":
                    decimal money;

                    do
                    {
                        Console.WriteLine("Enter the amount: ");
                        string amount = Console.ReadLine();

                        decimal.TryParse(amount, out money);

                        if (GELBalance >= money)
                        {
                            convertedAmount = money * Globals.GEL_TO_USD;

                            GELBalance -= money;
                            USDBalance += convertedAmount;

                            Transactions transaction =
                                new Transactions(DateTime.Now, "Convert", money, convertedAmount, 0);
                            TransactionHistory.Add(transaction);

                            break;
                        }
                        else
                        {
                            Console.WriteLine("Insufficient funds. Try again.");
                        }
                    } while (true);
                
                    break;
                case "GELEUR":
                    do
                    {
                        Console.WriteLine("Enter the amount: ");
                        string amount = Console.ReadLine();

                        decimal.TryParse(amount, out money);

                        if (GELBalance >= money)
                        {
                            convertedAmount = money * Globals.GEL_TO_EUR;

                            GELBalance -= money;
                            EURBalance += convertedAmount;

                            Transactions transaction =
                                new Transactions(DateTime.Now, "Convert", money, 0, convertedAmount);
                            TransactionHistory.Add(transaction);

                            break;
                        }
                        else
                        {
                            Console.WriteLine("Insufficient funds. Try again.");
                        }
                    } while (true);
                
                    break;
                case "USDEUR":
                    do
                    {
                        Console.WriteLine("Enter the amount: ");
                        string amount = Console.ReadLine();

                        decimal.TryParse(amount, out money);

                        if (USDBalance >= money)
                        {
                            convertedAmount = money * Globals.USD_TO_EUR;

                            USDBalance -= money;
                            EURBalance += convertedAmount;

                            Transactions transaction =
                                new Transactions(DateTime.Now, "Convert", 0, money, convertedAmount);
                            TransactionHistory.Add(transaction);

                            break;
                        }
                        else
                        {
                            Console.WriteLine("Insufficient funds. Try again.");
                        }
                    } while (true);
                
                    break;
                case "USDGEL":
                    do
                    {
                        Console.WriteLine("Enter the amount: ");
                        string amount = Console.ReadLine();

                        decimal.TryParse(amount, out money);

                        if (USDBalance >= money)
                        {
                            convertedAmount = money * Globals.USD_TO_GEL;

                            USDBalance -= money;
                            GELBalance += convertedAmount;

                            Transactions transaction =
                                new Transactions(DateTime.Now, "Convert", convertedAmount, money, 0);
                            TransactionHistory.Add(transaction);

                            break;
                        }
                        else
                        {
                            Console.WriteLine("Insufficient funds. Try again.");
                        }
                    } while (true);
                
                    break;
                case "EURGEL":
                    do
                    {
                        Console.WriteLine("Enter the amount: ");
                        string amount = Console.ReadLine();

                        decimal.TryParse(amount, out money);

                        if (EURBalance >= money)
                        {
                            convertedAmount = money * Globals.EUR_TO_GEL;

                            EURBalance -= money;
                            GELBalance += convertedAmount;

                            Transactions transaction =
                                new Transactions(DateTime.Now, "Convert", convertedAmount, 0, money);
                            TransactionHistory.Add(transaction);

                            break;
                        }
                        else
                        {
                            Console.WriteLine("Insufficient funds. Try again.");
                        }
                    } while (true);
                
                    break;
                case "EURUSD":
                    do
                    {
                        Console.WriteLine("Enter the amount: ");
                        string amount = Console.ReadLine();

                        decimal.TryParse(amount, out money);

                        if (EURBalance >= money)
                        {
                            convertedAmount = money * Globals.EUR_TO_USD;

                            EURBalance -= money;
                            USDBalance += convertedAmount;

                            Transactions transaction =
                                new Transactions(DateTime.Now, "Convert", 0, convertedAmount, money);
                            TransactionHistory.Add(transaction);

                            break;
                        }
                        else
                        {
                            Console.WriteLine("Insufficient funds. Try again.");
                        }
                    } while (true);
                
                    break;
                default:
                    throw new Exception("Invalid currency types.");
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        
        SaveUser();
        
        ShowBalance();
    }
    
    private string GetValidCurrencyInput()
    {
        string currency;

        do
        {
            Console.WriteLine("Enter a valid currency (GEL/USD/EUR):");
            currency = Console.ReadLine().ToUpper();

        } while (currency != "GEL" && currency != "USD" && currency != "EUR");

        return currency;
    }
}