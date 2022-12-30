using static System.Console;

namespace UpdatedPayInInstallmentsApp
{
    internal class Program
    {
        private static Operations _operations = new Operations();

        static void Log(string message)
        {
            Console.WriteLine(message);
        }

        static void Continue()
        {
            
            Log("\nWould you like to perform another transaction (Y/N) ? ");
            string response = ReadLine().Trim().ToLower();

            switch(response)
            {
                case "y":
                    Console.Clear();
                    Run();
                    break;
                case "n":
                    Log("Thank you for using our app. You are now exiting");
                    Environment.Exit(0);
                    break;
                default:
                    Log("Enter either Y for Yes or N for No please");
                    Continue();
                    break;
            }

        }

        static void Menu()
        {

        }
        static void Run()
        {
                     
            Log("-----------------------------------------------------------------------------------");
            Log("Welcome to Buhari's Pay In Installments app. It lets you pay back you debt in bits.");
            Log("-----------------------------------------------------------------------------------");
            
            Log("[1] Register [2] Pay [3] Exit");
            string response = ReadLine().Trim();

            switch(response)
            {
                case "1":
                    _operations.Register();
                    Continue();
                    break;
                case "2":
                    _operations.Pay();
                    Continue();
                    break;
                case "3":
                    Environment.Exit(0);
                    break;
                default:
                    Log("Invalid input. Try again.");
                    Console.Clear();
                    Run();
                    break;
            }
        }
        static void Main(string[] args)
        {
            Run();
        }
    }
}