using System;
using Services;
using Domain;
using Exceptions;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            AccountUtility service = new AccountUtility();

            while (true)
            {
                Console.WriteLine("\n0. Create Account");
                Console.WriteLine("1. Display Accounts");
                Console.WriteLine("2. Deposit");
                Console.WriteLine("3. Withdraw");
                Console.WriteLine("4. Exit");

                Console.Write("Enter choice: ");
                int choice = Convert.ToInt32(Console.ReadLine());

                try
                {
                    switch (choice)
                    {
                        case 0:
                            Console.Write("Enter Account Number: ");
                            string accNo = Console.ReadLine();

                            Console.Write("Enter Holder Name: ");
                            string name = Console.ReadLine();

                            Console.Write("Enter Initial Balance: ");
                            decimal balance = Convert.ToDecimal(Console.ReadLine());

                            Account newAccount = new Account
                            {
                                AccountNumber = accNo,
                                HolderName = name,
                                Balance = balance
                            };

                            service.AddEntity(newAccount);
                            Console.WriteLine("Account Created Successfully.");
                            break;

                        case 1:
                            var accounts = service.GetAll();
                            foreach (var acc in accounts)
                            {
                                Console.WriteLine($"Account No: {acc.AccountNumber}, Name: {acc.HolderName}, Balance: {acc.Balance}");
                            }
                            break;

                        case 2:
                            Console.Write("Enter Account Number: ");
                            string depositAcc = Console.ReadLine();

                            Console.Write("Enter Amount to Deposit: ");
                            decimal depositAmount = Convert.ToDecimal(Console.ReadLine());

                            service.Deposit(depositAcc, depositAmount);
                            Console.WriteLine("Deposit Successful.");
                            break;

                        case 3:
                            Console.Write("Enter Account Number: ");
                            string withdrawAcc = Console.ReadLine();

                            Console.Write("Enter Amount to Withdraw: ");
                            decimal withdrawAmount = Convert.ToDecimal(Console.ReadLine());

                            service.Withdraw(withdrawAcc, withdrawAmount);
                            Console.WriteLine("Withdraw Successful.");
                            break;

                        case 4:
                            Console.WriteLine("Thank You");
                            return;

                        default:
                            Console.WriteLine("Invalid choice");
                            break;
                    }
                }
                catch (NegativeBalanceException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (InsufficientFundsException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (AccountNotFoundException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (Exception)
                {
                    Console.WriteLine("Invalid Input. Please try again.");
                }
            }
        }
    }
}
