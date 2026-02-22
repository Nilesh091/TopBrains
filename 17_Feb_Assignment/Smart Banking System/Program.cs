using System;
using System.Collections.Generic;
using System.Linq;
using Smart_Banking_System;
class Program
{
  static void Main()
  {
    List<BankAccount> accounts = new List<BankAccount>
        {
            new SavingsAccount(12212345, "Rohan", 80000),
            new SavingsAccount(12212234, "Amit", 40000),
            new CurrentAccount(12212545, "Ritika", 120000),
            new CurrentAccount(12212232, "Suresh", 30000),
            new LoanAccount(12212232, "Raj", 200000),
            new SavingsAccount(12212324, "Ramesh", 60000)
        };

    // LINQ Queries

    var tc1 = accounts.Where(a => a.Balance > 50000).ToList();
    var tc2 = accounts.Sum(a => a.Balance);
    var tc3 = accounts.OrderByDescending(a => a.Balance).Take(3).ToList();
    var tc4 = accounts.GroupBy(a => a.GetType().Name);
    var tc5 = accounts.Where(a => a.CustomerName.StartsWith("R")).ToList();

  }
}