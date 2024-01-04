﻿using System.Linq;
using System.Transactions;
using Newtonsoft.Json;

namespace Finals;

public class User
{
    [JsonProperty("FirstName")]
    private string FirstName { get; set; }
    [JsonProperty("LastName")]
    private string LastName { get; set; }
    [JsonProperty("Balance")]
    private decimal Balance { get; set; }
    [JsonProperty("CardDetails")]
    private CardDetails CardDetails { get; set; }
    [JsonProperty("TransactionHistory")]
    private List<Transactions> TransactionHistory { get; set; }

    public User(string firstName, string lastName, CardDetails cardDetails)
    {
        FirstName = firstName;
        LastName = lastName;
        Balance = 0;
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
                Console.WriteLine(String.Format("{0,-15} {1,-10} {2,-15} {3,-15} {4,-15}", transaction.TransactionDate.ToShortDateString(), transaction.TransactionType, transaction.AmountGEL, transaction.AmountUSD, transaction.AmountEUR));
            }
            Console.WriteLine();
        }
    }
    public decimal GetBalance()
    {
        return this.Balance;
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
                    this.Balance += amountGEL;
                } else if (currencyType == "USD")
                {
                    amountUSD = amount;
                    this.Balance += amount * 3;
                }
                else
                {
                    amountEUR = amount;
                    this.Balance += amount * 4;
                }

                Transactions transaction = new Transactions(DateTime.Now, "Cash in", amountGEL, amountUSD, amountEUR);

                this.TransactionHistory.Add(transaction);
                string userJson = JsonConvert.SerializeObject(this);
                File.WriteAllText(Globals.jsonFilePath, userJson);
                Console.WriteLine($"Your new balance is {this.Balance} GEL.");
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
}