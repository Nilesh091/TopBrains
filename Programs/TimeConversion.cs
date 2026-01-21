using System.Data;

class TimeConversion
{
  public static void Main()
  {
    Console.WriteLine("Enter Seconds: ");
    int min = Convert.ToInt32(Console.ReadLine());
    string str = ConvertMin(min);
    Console.WriteLine("Converted Minutes:" + str);
  }
  public static string ConvertMin(int n)
  {
    int minutes = n / 60;
    int seconds = n % 60;
    return $"{minutes}:{seconds:D2}";
  }
}