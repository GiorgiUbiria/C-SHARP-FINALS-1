using System;
using System.Collections.Generic;
using System.Linq;
using Finals;
using Newtonsoft.Json; // Nuget Package

public class Program
{
    public static void Main()
    {
        string jsonFilePath = "user.json"; // NOTE: creates the user.json file in bin/Debug/net8.0

        if (File.Exists(jsonFilePath))
        {
            string json = File.ReadAllText(jsonFilePath);
            User user = JsonConvert.DeserializeObject<User>(json);

            StartBankingApplication(user);
        }
        else
        {
            Console.WriteLine("Welcome to the banking application. Please create an account.");

            Console.WriteLine("Enter your first name:");
            string firstName = Console.ReadLine();

            Console.WriteLine("Enter your last name:");
            string lastName = Console.ReadLine();

            Console.WriteLine("Enter your card number (16 digits):");
            string cardNumber = Console.ReadLine();

            Console.WriteLine("Enter your card expiration date (MM/YY):");
            string expirationDate = Console.ReadLine();

            Console.WriteLine("Enter your CVC (3 digits):");
            string cvc = Console.ReadLine();

            Console.WriteLine("Enter your pin code (4 digits):");
            string pinCode = Console.ReadLine();

            User user = new User
            {
                FirstName = firstName,
                LastName = lastName,
                Balance = 0,
                CardDetails = new CardDetails
                {
                    CardNumber = cardNumber,
                    ExpirationDate = expirationDate,
                    CVC = cvc,
                    PinCode = pinCode
                },
                TransactionHistory = new List<Transactions>()
            };

            string userJson = JsonConvert.SerializeObject(user);

            File.WriteAllText(jsonFilePath, userJson);

            StartBankingApplication(user);
        }
    }

    public static void StartBankingApplication(User user)
    {
        Console.WriteLine("Please enter your card number (16 digits):");
        string cardNumber = Console.ReadLine();
        Console.WriteLine("Please enter your card expiration date (MM/YY):");
        string expirationDate = Console.ReadLine();

        if (cardNumber == user.CardDetails.CardNumber && expirationDate == user.CardDetails.ExpirationDate)
        {
            Console.WriteLine("Please enter your pin code (4 digits):");
            string pinCode = Console.ReadLine();

            if (pinCode == user.CardDetails.PinCode)
            {
                ShowMenu(user);
            }
            else
            {
                Console.WriteLine("Incorrect pin code. The application will exit.\n");
                Environment.Exit(0);
            }
        }
        else
        {
            Console.WriteLine("Incorrect card number or expiration date. The application will exit.\n");
            Environment.Exit(0);
        }
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
                Console.WriteLine($"Your balance is {user.Balance} GEL.");
                ShowMenu(user);
                break;
            case "2":
                // TODO: Implement the cash out logic here
                break;
            case "3":
                user.getLastFiveTransactions();
                ShowMenu(user);
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