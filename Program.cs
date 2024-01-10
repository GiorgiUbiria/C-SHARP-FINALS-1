using Finals;
using Newtonsoft.Json;
using NLog;

public class Program
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    public static void Main()
    {
        LogManager.LoadConfiguration("nlog.config");

        Logger.Info("Application started");

        Console.WriteLine("Welcome to the banking application.");

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
        Logger.Info("Starting banking application");

        Console.WriteLine("Please enter your card number (16 digits):");
        string cardNumber = Console.ReadLine();

        Console.WriteLine("Please enter your card expiration date (MM/YY):");
        string expirationDate = Console.ReadLine();

        Console.WriteLine("Please enter your card cvc (3 digits on the back):");
        string cvc = Console.ReadLine();

        if (user.CheckCardGeneralInformation(cardNumber, expirationDate, cvc))
        {
            Console.WriteLine("Please enter your pin code (4 digits):");
            string pinCode = Console.ReadLine();

            if (user.CheckCardPinCode(pinCode))
            {
                Logger.Info("User authentication successful");
                ShowMenu(user);
            }
            else
            {
                Logger.Warn("Incorrect pin code entered. Exiting application.");
                Console.WriteLine("Incorrect pin code. The application will exit.\n");
                Environment.Exit(0);
            }
        }
        else
        {
            Logger.Warn("Incorrect card information entered. Exiting application.");
            Console.WriteLine("Incorrect card number, expiration date, or cvc. The application will exit.\n");
            Environment.Exit(0);
        }
    }

    public static void Prompt()
    {
        Logger.Info("Prompting user to create an account");

        Console.WriteLine("Please create an account.");

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

    public static void ShowMenu(User user)
    {
        Logger.Info("Displaying main menu");

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
                Logger.Info("User checking balance");
                user.ShowBalance();
                ShowMenu(user);
                break;
            case "2":
                Logger.Info("User performing cash out");
                user.CashOut();
                ShowMenu(user);
                break;
            case "3":
                Logger.Info("User viewing last 5 transactions");
                user.GetLastFiveTransactions();
                ShowMenu(user);
                break;
            case "4":
                Logger.Info("User performing cash in");
                user.CashIn();
                ShowMenu(user);
                break;
            case "5":
                Logger.Info("User changing pin code");
                user.ChangePinCode();
                ShowMenu(user);
                break;
            case "6":
                Logger.Info("User converting currency");
                user.Convert();
                ShowMenu(user);
                break;
            default:
                Logger.Warn("Invalid option entered by the user");
                Console.WriteLine("Invalid option. Please try again.");
                ShowMenu(user);
                break;
        }
    }
}