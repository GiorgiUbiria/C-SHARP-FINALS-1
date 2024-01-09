using Finals;
using Newtonsoft.Json; // Nuget Package

public class Program
{
    public static void Main()
    {

        if (File.Exists(Globals.jsonFilePath))
        {
            string json = File.ReadAllText(Globals.jsonFilePath);
            User user = JsonConvert.DeserializeObject<User>(json);

            StartBankingApplication(user);
        }
        else
        {
            Prompt();
        }
    }

    public static void StartBankingApplication(User user)
    {
        Console.WriteLine("Please enter your card number (16 digits):");
        string cardNumber = Console.ReadLine();
        Console.WriteLine("Please enter your card expiration date (MM/YY):");
        string expirationDate = Console.ReadLine();

        if (user.CheckCardGeneralInformation(cardNumber, expirationDate))
        {
            Console.WriteLine("Please enter your pin code (4 digits):");
            string pinCode = Console.ReadLine();

            if (user.CheckCardPinCode(pinCode))
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
                Console.WriteLine($"Your balance is {user.GetGELBalance()} GEL, {user.GetUSDBalance()} USD, {user.GetEURBalance()} EUR.");
                ShowMenu(user);
                break;
            case "2":
                user.CashOut();
                ShowMenu(user);
                break;
            case "3":
                user.GetLastFiveTransactions();
                ShowMenu(user);
                break;
            case "4":
                user.CashIn();
                ShowMenu(user);
                break;
            case "5":
                user.ChangePinCode();
                ShowMenu(user);
                break;
            case "6":
                user.Convert();
                ShowMenu(user);
                break;
            default:
                Console.WriteLine("Invalid option. Please try again.");
                ShowMenu(user);
                break;
        }
    }

    public static void Prompt()
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

            CardDetails cardDetails = new CardDetails(cardNumber, expirationDate, cvc, pinCode);
            Console.WriteLine(cardDetails);

            User user = new User(firstName, lastName, cardDetails);
            
            string userJson = JsonConvert.SerializeObject(user);
            Console.WriteLine(userJson);

            File.WriteAllText(Globals.jsonFilePath, userJson);

            StartBankingApplication(user);
    }
}