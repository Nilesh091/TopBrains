class MultiplicationTable
{
  public static void Main()
  {
    int[] arr = MulTable(3, 7);
    foreach (int v in arr)
    {
      Console.WriteLine(v);
    }
  }
  public static int[] MulTable(int of, int upto)
  {
    List<int> li = new List<int>();
    for (int i = 1; i <= upto; i++)
    {
      li.Add(of * i);
    }
    return li.ToArray();
  }
}