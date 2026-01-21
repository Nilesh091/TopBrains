class GreatestCommonDivisor
{
  public static void Main()
  {
    Console.WriteLine("Enter the number 1:");
    int n1 = Convert.ToInt32(Console.ReadLine());
    Console.WriteLine("Enter the number 2:");
    int n2 = Convert.ToInt32(Console.ReadLine());

    int gcd = Gcd(n1, n2);
    Console.WriteLine("GCD of 48 and 18 is: " + gcd);
  }
  public static int Gcd(int a, int b)
  {
    if (b == 0) return a;
    return (Gcd(b, a % b));
  }
}