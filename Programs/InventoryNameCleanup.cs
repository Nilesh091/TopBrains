using System.Globalization;
using System.Text;
using System.Transactions;

class InventoryNameCleanup
{
  public static void Main()
  {
    string str = "hellllooo Worrlldd";
    string s1 = RemoveConsecutiveDuplicates(str);
    string s2 = TrimExtraSpaces(s1);
    string s3 = ToTitleCase(s2);
    Console.WriteLine(s3);
  }
  public static string RemoveConsecutiveDuplicates(string str)
  {
    StringBuilder sb = new StringBuilder();
    char prev = '\0';
    foreach (char c in str)
    {
      if (c != prev)
      {
        sb.Append(c);
        prev = c;
      }
    }
    return sb.ToString();
  }

  public static string TrimExtraSpaces(string str)
  {
    if (string.IsNullOrWhiteSpace(str)) return string.Empty;
    return string.Join(" ", str.Split(' ', StringSplitOptions.RemoveEmptyEntries));
  }

  public static string ToTitleCase(string str)
  {
    return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str.ToLower());
  }
}