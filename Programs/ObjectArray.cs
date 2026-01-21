class ObjectArray
{
  public static void Main()
  {
    object[] obj = new object[] { 12, 13, "hell0", "nr", 20, 30, 40 };
    Console.WriteLine("Sum of integers in object array is: " + SumInObjectSrray(obj));
  }
  public static int SumInObjectSrray(object[] objArr)
  {
    int sum = 0;
    foreach (var v in objArr)
    {
      if (v is int x)
      {
        sum += x;
      }
    }
    return sum;
  }
}