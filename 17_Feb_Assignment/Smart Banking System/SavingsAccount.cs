using System;

namespace Smart_Banking_System
{
    public class SavingsAccount : BankAccount
    {
        private const double MinBalance = 10000;
        private const double InterestRate = 0.04; // 4%

        public SavingsAccount(int accNo, string name, int balance)
            : base(accNo, name, balance)
        {
            if (balance < MinBalance)
                throw new MinimumBalanceException("Savings account requires minimum balance of 10,000.");
        }

        public override void Withdraw(int amount)
        {
            if (Balance - amount < MinBalance)
                throw new MinimumBalanceException("Cannot go below minimum balance.");

            base.Withdraw(amount);
        }

        public override int Interest()
        {
            return (int)(Balance * InterestRate);
        }
    }
}
