using System;
using System.Collections.Generic;
using Domain;
using Exceptions;

namespace Services
{
    public class MemberUtility
    {
        private SortedDictionary<decimal, List<Member>> _data
            = new SortedDictionary<decimal, List<Member>>(
                Comparer<decimal>.Create((x, y) => y.CompareTo(x))
              );


        public void AddMember()
        {
            Console.Write("Enter Member Id: ");
            string id = Console.ReadLine();

            Console.Write("Enter Name: ");
            string name = Console.ReadLine();

            Console.Write("Enter Fine Amount: ");
            decimal fine = Convert.ToDecimal(Console.ReadLine());

            if (fine < 0)
                throw new InvalidFineException("Fine cannot be negative.");

            Member member = new Member
            {
                MemberId = id,
                Name = name,
                FineAmount = fine
            };

            if (!_data.ContainsKey(fine))
            {
                _data[fine] = new List<Member>();
            }

            _data[fine].Add(member);

            Console.WriteLine("Member Added Successfully.");
        }

        public void DisplayMemberByFine()
        {
            if (_data.Count == 0)
            {
                Console.WriteLine("No members found.");
                return;
            }

            foreach (var pair in _data)
            {
                foreach (var member in pair.Value)
                {
                    Console.WriteLine($"ID: {member.MemberId}, Name: {member.Name}, Fine: {member.FineAmount}");
                }
            }
        }

        public void PayFine()
        {
            Console.Write("Enter Member Id: ");
            string id = Console.ReadLine();

            foreach (var pair in _data)
            {
                var member = pair.Value.Find(m => m.MemberId == id);

                if (member != null)
                {
                    decimal oldFine = member.FineAmount;

                    pair.Value.Remove(member);

                    if (pair.Value.Count == 0)
                        _data.Remove(oldFine);

                    Console.WriteLine("Fine Paid Successfully.");
                    return;
                }
            }

            throw new MemberNotFoundException("Member not found.");
        }

        public IEnumerable<Member> GetAll()
        {
            List<Member> result = new List<Member>();

            foreach (var pair in _data)
            {
                result.AddRange(pair.Value);
            }

            return result;
        }
    }
}
