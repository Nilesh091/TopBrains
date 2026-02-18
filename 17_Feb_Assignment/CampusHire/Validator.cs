using System;

namespace CampusHire
{
    public class Validator
    {
        public static bool ValidateId(string id)
        {
            return id.Length == 8 && id.StartsWith("CH");
        }

        public static bool ValidateName(string name)
        {
            return name.Length >= 4 && name.Length <= 15;
        }

        public static bool ValidatePassingYear(int year)
        {
            return year <= DateTime.Now.Year;
        }
    }
}
