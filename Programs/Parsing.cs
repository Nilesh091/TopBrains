class Parsing
{
  public static void Main()
  {
    string[] arr = new string[] { "12", "23", "45", "asd", "&d", "23", "0" };
    Console.WriteLine("The sum of valid integer string is: " + SumOfVAlidCharacters(arr));
  }
  public static int SumOfVAlidCharacters(string[] starr)
  {
    int sum = 0;
    foreach (var v in starr)
    {
      if (int.TryParse(v, out int num))
      {
        sum += num;
      }
    }
    return sum;
  }
}