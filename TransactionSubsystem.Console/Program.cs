
namespace TransactionSubsystem.Console
{
    using System;

    class Program
    {
        static void Main(string[] args)
        {
            Bootstrapper bootstrapper = new Bootstrapper();

            Console.WriteLine("Start bootstrapping services");
            bootstrapper.Initialize();
            var apiMock = bootstrapper.ApiInstance;
            Console.WriteLine("Finished bootstrapping services/n");

            while (true)
            {
                apiMock.PrepareNewTransaction();

                var isUserValid = false;
                while (!isUserValid)
                {
                    Console.WriteLine("Enter recepient for transaction");
                    var recepientName = Console.ReadLine();

                    Console.WriteLine("Start verifying recepient user");
                    isUserValid = apiMock.VerifyRecepientUser(recepientName);
                    Console.WriteLine($"Finished. Result: {isUserValid}");
                }

                var isAmountValid = false;
                while (!isAmountValid)
                {
                    Console.WriteLine("Enter amount for transaction");
                    var amountStr = Console.ReadLine();
                    decimal amount;

                    isAmountValid = decimal.TryParse(amountStr, out amount);
                    if (isAmountValid)
                    {
                        Console.WriteLine("Start verifying amount");
                        isAmountValid = apiMock.VerifyDonorBalance(amount);
                        Console.WriteLine($"Finished. Result: {isAmountValid}");
                    }
                    else
                    {
                        Console.WriteLine("Failed parsing amount");
                    }
                }

                Console.WriteLine("Enter 'send'");

                string message = string.Empty;
                while (message != "send")
                {
                    message = Console.ReadLine();
                }

                Console.WriteLine("Start commiting transaction");
                try
                {
                    apiMock.CommitTransaction();
                    Console.WriteLine("Transaction comitted/n");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Transaction failed. {ex.Message}/n");
                }
            }
        }
    }
}
