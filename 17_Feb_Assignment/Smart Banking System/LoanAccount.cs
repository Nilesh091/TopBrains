using System;

namespace Smart_Banking_System
{
    public class LoanAccount : BankAccount
    {
        private const double LoanInterestRate = 0.08;

        public LoanAccount(int accNo, string name, int loanAmount)
            : base(accNo, name, loanAmount) { }

        public override void Deposit(int amount)
        {
            throw new InvalidTransactionException("Cannot deposit into a Loan Account.");
        }

        public override int Interest()
        {
            return (int)(Balance * LoanInterestRate);
        }
    }
}
