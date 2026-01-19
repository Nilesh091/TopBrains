using System;
using System.Collections.Generic;
using System.Linq;

public class Solution
{
  public static void Main()
  {
    string first = Console.ReadLine();
    string second = Console.ReadLine();

    Console.WriteLine(ProcessString(first, second));
  }

  public static string ProcessString(string first, string second)
  {
    HashSet<char> vowels = new HashSet<char>
        {
            'a','e','i','o','u'
        };

    HashSet<char> secondConsonants = new HashSet<char>();

    foreach (char c in second.ToLower())
    {
      if (!vowels.Contains(c))
      {
        secondConsonants.Add(c);
      }
    }

    // Step 1: Remove common consonants from first word
    List<char> filtered = new List<char>();

    foreach (char c in first)
    {
      char lower = char.ToLower(c);

      if (vowels.Contains(lower) || !secondConsonants.Contains(lower))
      {
        filtered.Add(c);
      }
    }

    // Step 2: Remove consecutive duplicate characters
    string result = "";
    for (int i = 0; i < filtered.Count; i++)
    {
      if (i == 0 || filtered[i] != filtered[i - 1])
      {
        result += filtered[i];
      }
    }

    return result;
  }
}
