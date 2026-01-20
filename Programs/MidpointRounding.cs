class MidpointRounding
{
  public static void Main()
  {
    Console.WriteLine("Enter the radius of the circle.");
    double d = Convert.ToDouble(Console.ReadLine());
    Console.WriteLine("Area of the circle with radius " + d + " is " + AreaPredictions(d));
  }

  public static double AreaPredictions(double radius)
  {
    double area = 22 * radius * radius / 7;
    return Math.Round(area, 2, System.MidpointRounding.AwayFromZero);
  }
}