using System.Data.Common;

class Programming
{
  public static void Main()
  {
    int m = Convert.ToInt32(Console.ReadLine());
    int n = Convert.ToInt32(Console.ReadLine());
    for (int i = m; i <= n; i++)
    {
      if (S(i))
      {
        Console.WriteLine(i + " This is a Luckey number");
      }
    }
  }
  public static bool S(int s)
  {
    return (SumOfDigit(s * s) == SumOfDigit(s) * SumOfDigit(s));
  }
  public static int SumOfDigit(int n)
  {
    int subSum = 0;
    while (n > 0)
    {
      subSum += n % 10;
      n = n / 10;
    }
    return subSum;
  }
}