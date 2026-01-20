using System;
using System.IO;
using System.Linq;
class FilterLogs
{
  public static void Main()
  {
    var lines = File.ReadLines("Log.txt");
    var filteredLines = lines.Where(s => s.Contains("[ERROR]")).ToArray();

    File.WriteAllLines("error.txt", filteredLines);
    Console.WriteLine("Error logged successfully.");
  }
}