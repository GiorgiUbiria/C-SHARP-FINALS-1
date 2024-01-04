using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using Finals;
using Newtonsoft.Json; // Nuget Package

public class Program
{
    public static void Main()
    {
        // Initialize a new user
        User user = new User
        {
            FirstName = "John",
            LastName = "Doe",
            CardDetails = new CardDetails
            {
                CardNumber = "1234-5678-9012-3456",
                ExpirationDate = "12/25",
                CVC = "123",
                PinCode = "1234"
            },
            TransactionHistory = new List<Transaction>()
        };

        // Start the banking application
        StartBankingApplication(user);
    }

    public static void StartBankingApplication(User user)
    {
        // TODO: Implement the login and verification logic here

        // If verification is successful, show the menu to the user
        ShowMenu(user);
    }

    public static void ShowMenu(User user)
    {
        Console.WriteLine("Choose an action:");
        Console.WriteLine("1. Check balance");
        Console.WriteLine("2. Cash out");
        Console.WriteLine("3. View last 5 transactions");
        Console.WriteLine("4. Cash in");
        Console.WriteLine("5. Change pin code");
        Console.WriteLine("6. Convert currency");

        string option = Console.ReadLine();

        switch (option)
        {
            case "1":
                // TODO: Implement the check balance logic here
                break;
            case "2":
                // TODO: Implement the cash out logic here
                break;
            case "3":
                // TODO: Implement the view transactions logic here
                break;
            case "4":
                // TODO: Implement the cash in logic here
                break;
            case "5":
                // TODO: Implement the change pin code logic here
                break;
            case "6":
                // TODO: Implement the convert currency logic here
                break;
            default:
                Console.WriteLine("Invalid option. Please try again.");
                ShowMenu(user);
                break;
        }
    }
}