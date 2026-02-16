using System.Collections.Generic;
using Domain;
using Exceptions;

namespace Services
{
    public class AccountUtility
    {
        private SortedDictionary<decimal, List<Account>> _data
            = new SortedDictionary<decimal, List<Account>>();

        public void AddEntity(Account account)
        {
            if (account.Balance < 0)
            {
                throw new NegativeBalanceException("balance must be greater than 0");
            }
            if (!_data.ContainsKey(account.Balance))
            {
                _data[account.Balance] = new List<Account>();
            }
            _data[account.Balance].Add(account);
        }

        public void Deposit(string accountNumber, decimal amount)
        {
            if (amount <= 0)
                throw new NegativeBalanceException("Deposit amount must be positive.");

            Account account = FindAccount(accountNumber);

            _data[account.Balance].Remove(account);
            if (_data[account.Balance].Count == 0)
                _data.Remove(account.Balance);

            account.Balance += amount;

            if (!_data.ContainsKey(account.Balance))
                _data[account.Balance] = new List<Account>();

            _data[account.Balance].Add(account);
        }

        public void Withdraw(string accountid,decimal amount)
        {
            if (amount <= 0)
            {
                throw new NegativeBalanceException("withdraw amount must be positive");
            }

            Account account = FindAccount(accountid);

            if(account.Balance < amount)
            {
                throw new InsufficientFundsException("Insufficient balance");
            }


            _data[account.Balance].Remove(account);
            if (_data[account.Balance].Count == 0)
            {
                _data.Remove(account.Balance);
            }

            account.Balance -= amount;


            if (!_data.ContainsKey(account.Balance))
            {
                _data[account.Balance] = new List<Account>();
            }

            _data[account.Balance].Add(account);


        }

        public Account FindAccount(string accountid)
        {
            foreach(var item in _data)
            {
                foreach(var account in item.Value)
                {
                    if(account.AccountNumber == accountid)
                    {
                        return account;
                    }
                }
            }

            throw new AccountNotFoundException("account not found");
        }

      
        public IEnumerable<Account> GetAll()
        {
            List<Account> result = new List<Account>();
            foreach(var pair in _data)
            {
                foreach(var account in pair.Value)
                {
                    result.Add(account);
                }
            }
            return result;
        }
    }
}
