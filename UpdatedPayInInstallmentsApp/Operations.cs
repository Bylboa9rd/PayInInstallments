using static System.Console;
using static UpdatedPayInInstallmentsApp.Enums;

namespace UpdatedPayInInstallmentsApp;

public class Operations
{
    private Debtor _debtor = new Debtor();

    private IList<Debtor> _debtors = new List<Debtor>
    {
        new Debtor(1, "xoxo@gmail.com", "okpa", 1234567899876543212, 1234, 500_000, Plan.Monthly, 350_000, 50_000),
        new Debtor(2, "mainichi@gmail.com", "sushi", 1876543211234567890, 4321, 120_000, Plan.Weekly, 90_000, 10_000)
    };

    public void GetCurrentDebtor()
    {
        WriteLine("Enter email: ");
        string? email = ReadLine().Trim();

        var result = from d in _debtors
                     where d.Email.ToLower().Equals(email.ToLower())
                     select d;

        var debtor = result.FirstOrDefault();

        if (debtor is not null)
        {
            _debtor = debtor;
        }
        else{
            WriteLine("Sorry, we could not find details that match yours");
        }

    }

    /*public void DisplayCuDebtors()
    {
        var debtors = from debtor in _debtors
                      orderby debtor.Id, debtor.Email, debtor.RemainingDebt
                      select debtor;
        foreach (var debtor in debtors)
        {
            WriteLine($"Id: {debtor.Id}, Email: {debtor.Email}, Total Debt: {debtor.RemainingDebt}");
        }
    }*/


    public void Register()
    {
    register:

        try
        {
            int userID, id = _debtors.Count;
            userID = ++id;

        getemail:

            WriteLine("Enter email: ");
            string? email = ReadLine().Trim();

            if (string.IsNullOrEmpty(email) || string.IsNullOrWhiteSpace(email))
            {
                ArgumentNullException.ThrowIfNull(email, nameof(email));
                goto getemail;
            }

            foreach (Debtor de in _debtors)
            {
                if (de.Email.Equals(email))
                {
                    Console.WriteLine("Email already exists. Try again");
                    goto getemail;
                }
            }

        getpassword:

            WriteLine("Enter password: ");
            string? password = ReadLine().Trim();

            if (string.IsNullOrEmpty(password) || string.IsNullOrWhiteSpace(password))
            {
                ArgumentNullException.ThrowIfNull(password, nameof(password));
                goto getpassword;
            }

        getdebitcardnumber:

            WriteLine("Enter debit card number: ");
            decimal? debitCardNumber = decimal.Parse(ReadLine().Trim());

            if (!debitCardNumber.HasValue)
            {
                ArgumentNullException.ThrowIfNull(debitCardNumber, nameof(debitCardNumber));
                goto getdebitcardnumber;
            }
            if (!debitCardNumber.HasValue || debitCardNumber <= 0)
            {
                WriteLine("Debit card number cannot be less than or equal to zero!");
                goto getdebitcardnumber;
            }
            if (debitCardNumber?.ToString().Length != 19)
            {
                WriteLine("Digits must be exactly 19 characters long");
                goto getdebitcardnumber;
            }


        getpin:

            WriteLine("Enter debit card pin: ");
            short? pin = short.Parse(ReadLine().Trim());

            if (!pin.HasValue)
            {
                ArgumentNullException.ThrowIfNull(pin, nameof(pin));
                goto getpin;
            }
            if (pin <= 0)
            {
                WriteLine("Debit card pin cannot be less than or equal to zero!");
                goto getpin;
            }
            if (pin?.ToString().Length != 4)
            {
                WriteLine("Digits must be exactly 4 characters long");
                goto getpin;
            }

        getoriginaldebt:

            WriteLine("Enter original debt amount (not in words): ");
            decimal? originalDebt = decimal.Parse(ReadLine().Trim());


            if (!originalDebt.HasValue)
            {
                ArgumentNullException.ThrowIfNull(originalDebt, nameof(originalDebt));
                goto getoriginaldebt;
            }
            if (originalDebt <= 0)
            {
                WriteLine("Debit card pin cannot be less than or equal to zero!");
                goto getoriginaldebt;
            }


            decimal? remainingDebt = originalDebt;


        getplan:

            WriteLine("Choose your desired plan: [1] Daily [2] Weekly [3] Bi-weekly [4] Monthly [5] Six-monthly [6] Yearly\nYour choice:");
            int? plan = int.Parse(ReadLine().Trim());

            if (!plan.HasValue)
            {
                ArgumentNullException.ThrowIfNull(plan, nameof(plan));
                goto getplan;
            }
            if (plan <= 0 || plan > 6)
            {
                WriteLine("Please choose from the options 1 - 6 above!");
                goto getplan;
            }

            var choice = _debtor.GetPlan((int)plan);


        getamountperdeposit:

            WriteLine("Enter amount you can pay per installment (not in words): ");
            decimal? amountPerDeposit = decimal.Parse(ReadLine().Trim());

            if (!amountPerDeposit.HasValue)
            {
                ArgumentNullException.ThrowIfNull(amountPerDeposit, nameof(amountPerDeposit));
                goto getamountperdeposit;
            }
            if (amountPerDeposit <= 0)
            {
                throw new Exception("Debit card pin cannot be less than or equal to zero!");
                goto getamountperdeposit;
            }
            if (amountPerDeposit % 1000 != 0)
            {
                WriteLine("Sorry! Amount must be divisible by 1000. Please try again");
                goto getamountperdeposit;
            }

            Debtor debtor = new Debtor(userID, email, password, debitCardNumber, pin, originalDebt, choice, remainingDebt, amountPerDeposit);

            _debtors.Add(debtor);

            WriteLine($"Registration successful with Id: {userID}, Email:{email}");
        }
        catch (ArgumentNullException)
        {
            WriteLine("Fields cannot be null!");
            goto register;

        }
        catch (FormatException)
        {
            WriteLine("Input was in an incorrect format");
            goto register;
        }
        catch (Exception e)
        {
            WriteLine(e.Message);
            goto register;
        }
    }

    public void Pay()
    {
        WriteLine("---------------------------");
        WriteLine("Pay Off Your Remaining Debt");
        WriteLine("---------------------------");
        GetCurrentDebtor();

        if (_debtor is not null && _debtor.RemainingDebt > 0)
        {
            if (_debtor.RemainingDebt > _debtor.AmountPerDeposit)
            {
                _debtor.SetRemainingDebt();
                _debtor.SetNextExpectedPayment();
                WriteLine($"You've paid {_debtor.AmountPerDeposit} on this day {DateTime.Now}. You have an outstanding debt of {_debtor.RemainingDebt} and your next expected payment is due on {_debtor.NextExpectedPayment}");
            }
            else if (_debtor.RemainingDebt < _debtor.AmountPerDeposit)
            {
                _debtor.ClearDebt();
                WriteLine($"You've paid {_debtor.RemainingDebt} on this day {DateTime.Now}");
                WriteLine("Hurrah! You have cleared your debt! You have no more outstanding debts.");
                _debtors.Remove(_debtor);
            }
        }
        else
        {
            WriteLine("Debtor not found!");
        }


    }
}
