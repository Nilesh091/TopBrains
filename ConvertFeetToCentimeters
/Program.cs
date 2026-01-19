class Program
{
  public static void Main()
  {
    
  }
  public static double Convert(int n)
  {
    return Math.Round(n * 30.48, 2, MidpointRounding.AwayFromZero);
  }
}