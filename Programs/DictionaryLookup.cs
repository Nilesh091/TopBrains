using System.Runtime.CompilerServices;

class DictionaryLookup
{
  public static void Main()
  {
    // EmpIdSalary
    Dictionary<int, int> employdetais = new Dictionary<int, int>();
    employdetais.Add(121, 30000);
    employdetais.Add(122, 40000);
    employdetais.Add(123, 12000);
    employdetais.Add(124, 23000);
    int sumOfSalary = 0;
    foreach (var v in employdetais)
    {
      sumOfSalary += v.Value;
    }
    Console.WriteLine("The sum of salary of all the employee = " + sumOfSalary);

  }
}