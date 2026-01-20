class SwapWithoutTemp
{
  public static void Main()
  {
    int n1 = 20;
    int n2 = 30;
    Swap1(ref n1, ref n2);
    Console.WriteLine(n1 + "  " + n2);
    int j, k;
    Swap2(n1, n2, out j, out k);
    Console.WriteLine(j + "  " + k);
  }
  public static void Swap1(ref int n1, ref int n2)
  {
    n1 = n1 + n2;
    n2 = n1 - n2;
    n1 = n1 - n2;
  }
  public static void Swap2(int n1, int n2, out int j, out int k)
  {
    j = n2;
    k = n1;
  }
}