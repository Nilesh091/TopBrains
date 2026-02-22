using System;

namespace Smart_Banking_System
{
    public class CurrentAccount : BankAccount
    {
        private const double OverdraftLimit = 20000;

        public CurrentAccount(int accNo, string name, int balance)
            : base(accNo, name, balance) { }

        public override void Withdraw(int amount)
        {
            if (amount > Balance + OverdraftLimit)
                throw new InsufficientBalanceException("Overdraft limit exceeded.");

            Balance -= amount;
        }

        public override int Interest()
        {
            return 0; // Usually no interest
        }
    }
}
