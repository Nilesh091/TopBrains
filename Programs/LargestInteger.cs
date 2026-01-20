class LargestInteger
{
  public static void Main()
  {
    int largest = Largest(20, 30, 40);
    Console.WriteLine("largest between these three variables 20,30,40 is : " + largest);
  }
  public static int Largest(int n1, int n2, int n3)
  {
    int temp = n1;
    if (n2 > temp) temp = n2;
    if (n3 > temp) temp = n3;

    return temp;
  }
}