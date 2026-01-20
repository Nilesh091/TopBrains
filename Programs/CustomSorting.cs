class Student
{
  public string Name { get; set; }
  public int Age { get; set; }
  public int Mark { get; set; }
}
class CustomSorting
{
  public static void Main()
  {
    List<Student> list = new List<Student>()
    {
      new Student(){Name="Nilu",Age=21,Mark=89},
      new Student(){Name="Om",Age=22,Mark=93},
      new Student(){Name="Yash",Age=23,Mark=88},
      new Student(){Name="Dev",Age=23,Mark=92},
      new Student(){Name="Lok",Age=21,Mark=89}
    };
    List<Student> sortedList = SortedListOfStudents(list);
    foreach (var v in sortedList)
    {
      Console.WriteLine("Name: " + v.Name + " Age: " + v.Age + " Mark: " + v.Mark);
    }
  }

  public static List<Student> SortedListOfStudents(List<Student> list)
  {
    return list.OrderByDescending(s => s.Mark).ThenBy(s => s.Age).ToList();
  }
}