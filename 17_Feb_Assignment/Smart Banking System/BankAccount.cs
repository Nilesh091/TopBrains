using System;
using System.ComponentModel;

namespace Smart_Banking_System
{
    public abstract class BankAccount
    {
        public int AccountNumber { get; set; }
        public string CustomerName { get; set; }
        public int Balance { get; set; }

        public BankAccount(int accno, string name, int balance)
        {
            this.AccountNumber = accno;
            this.CustomerName = name;
            this.Balance = balance;
        }

        public virtual void Deposit(int n)
        {
        }
        public virtual void Withdraw(int n)
        {

        }
        public virtual int Interest()
        {
            return 0;
        }
    }
}
