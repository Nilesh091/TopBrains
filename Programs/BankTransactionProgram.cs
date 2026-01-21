class BankTransactionProgram
{
  public static void Main()
  {
    int initialBalance = 100;

    int[] transactions = new int[] { 50, -30, -200, 80, -150, -60, 0 };
    Console.WriteLine("Final Balance = " + CalculatebalanceAmount(initialBalance, transactions));
  }
  public static int CalculatebalanceAmount(int initialBalance, int[] transactions)
  {
    foreach (var v in transactions)
    {
      if (v >= 0)
      {
        initialBalance += v;
      }
      else if (v < 0)
      {
        if ((initialBalance + v) >= 0) initialBalance += v;
      }
    }
    return initialBalance;
  }
}