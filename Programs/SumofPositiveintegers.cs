class SumofPositiveintegers
{
  public static void Main()
  {
    int[] arr = new int[] { 1, 2, 3, 4, -2, 4, -9, 0, 3, -2, 4 };
    Console.WriteLine(SumofPositive(arr));
  }
  public static int SumofPositive(int[] arr)
  {
    int sum = 0;
    foreach (var v in arr)
    {
      if (v == 0) break;
      else if (v < 0) continue;
      sum += v;
    }
    return sum;
  }
}