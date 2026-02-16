using System;
using System.Collections.Generic;
using Domain;
using Exceptions;

namespace Services
{
    public class MedicineUtility
    {
        private SortedDictionary<int, List<Medicine>> _medicines =
            new SortedDictionary<int, List<Medicine>>();

        public void AddMedicine(Medicine medicine)
        {
            if (medicine.Price <= 0)
                throw new InvalidPriceException("Price must be greater than 0");

            if (medicine.ExpiryYear < DateTime.Now.Year)
                throw new InvalidExpiryYearException("Expiry year cannot be in past");

            foreach (var pair in _medicines)
            {
                foreach (var med in pair.Value)
                {
                    if (med.Id == medicine.Id)
                        throw new DuplicateMedicineException("Medicine ID already exists");
                }
            }

            if (!_medicines.ContainsKey(medicine.ExpiryYear))
            {
                _medicines[medicine.ExpiryYear] = new List<Medicine>();
            }

            _medicines[medicine.ExpiryYear].Add(medicine);
        }

        public void GetAllMedicines()
        {
            foreach (var pair in _medicines)
            {
                foreach (var med in pair.Value)
                {
                    Console.WriteLine(
                        $"Details: {med.Id} {med.Name} {med.Price} {med.ExpiryYear}");
                }
            }
        }

        public void UpdateMedicinePrice(string id, int newPrice)
        {
            if (newPrice <= 0)
                throw new InvalidPriceException("Price must be greater than 0");

            foreach (var pair in _medicines)
            {
                foreach (var med in pair.Value)
                {
                    if (med.Id == id)
                    {
                        med.Price = newPrice;
                        return;
                    }
                }
            }

            throw new MedicineNotFoundException("Medicine not found");
        }
    }
}
