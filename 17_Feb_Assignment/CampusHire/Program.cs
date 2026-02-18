namespace CampusHire
{
  class Program
  {
    public static void Main()
    {
      ApplicantManagement am = new ApplicantManagement();
      int choice;

      do
      {
        Console.WriteLine("\n1. Add Applicant");
        Console.WriteLine("2. Display All");
        Console.WriteLine("3. Search");
        Console.WriteLine("4. Update");
        Console.WriteLine("5. Delete");
        Console.WriteLine("6. Exit");

        Console.Write("Enter choice: ");
        choice = int.Parse(Console.ReadLine());

        switch (choice)
        {
          case 1:
            AddApplicant(am);
            break;

          case 2:
            am.DisplayAll();
            break;

          case 3:
            Console.Write("Enter ID to search: ");
            var found = am.Search(Console.ReadLine());
            if (found != null)
              Console.WriteLine($"Found: {found.ApplicantName}");
            else
              Console.WriteLine("Not found");
            break;

          case 4:
            Console.Write("Enter ID to update: ");
            string id = Console.ReadLine();
            AddApplicant(am);
            break;

          case 5:
            Console.Write("Enter ID to delete: ");
            am.Delete(Console.ReadLine());
            break;

          case 6:
            Console.WriteLine("Exiting...");
            break;

          default:
            Console.WriteLine("Invalid choice");
            break;
        }

      } while (choice != 6);
    }
    public static void AddApplicant(ApplicantManagement am)
    {
      Console.Write("Enter ID: ");
      string id = Console.ReadLine();

      if (!Validator.ValidateId(id))
      {
        Console.WriteLine("Invalid ID format.");
        return;
      }

      Console.Write("Enter Name: ");
      string name = Console.ReadLine();

      if (!Validator.ValidateName(name))
      {
        Console.WriteLine("Invalid Name length.");
        return;
      }

      Console.Write("Enter Current Location: ");
      string current = Console.ReadLine();

      Console.Write("Enter Preferred Location: ");
      string preferred = Console.ReadLine();

      Console.Write("Enter Core Competency: ");
      string skill = Console.ReadLine();

      Console.Write("Enter Passing Year: ");
      int year = int.Parse(Console.ReadLine());

      if (!Validator.ValidatePassingYear(year))
      {
        Console.WriteLine("Invalid Passing Year.");
        return;
      }

      Applicant applicant = new Applicant
      {
        ApplicantId = id,
        ApplicantName = name,
        CurrentLocation = current,
        PreferredLocation = preferred,
        CoreCompetency = skill,
        PassingYear = year
      };

      am.AddApplicant(applicant);
    }
  }
}
