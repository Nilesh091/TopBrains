class Program
{
  public static void Main()
  {
    Console.WriteLine(Convert(5));
    Console.WriteLine(Convert(1));
    Console.WriteLine(Convert(0));
  }
  public static double Convert(int n)
  {
    return Math.Round(n * 30.48, 2, MidpointRounding.AwayFromZero);
  }
}