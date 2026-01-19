using System.Diagnostics.SymbolStore;
using System.Formats.Asn1;
using System.Globalization;

class Program
{
  public static void Main()
  {
    Console.WriteLine("Enter height of the person");
    int n = Convert.ToInt32(Console.ReadLine());
    Console.WriteLine("We can considerthe person in: " + HeightCheck(n) + " Catagory");
  }
  public static string HeightCheck(int height)
  {
    if (height < 150) return "Short";
    else if (height >= 150 && height < 180) return "Average";
    return "Tall";
  }
}