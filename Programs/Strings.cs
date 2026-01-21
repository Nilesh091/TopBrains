using System.Formats.Asn1;
using System.Reflection.Metadata.Ecma335;

interface IArea
{
  double Area();
}
abstract class Shape : IArea
{
  public abstract double Area();
}

class Circle : Shape
{
  private double radius;
  public Circle(double r)
  {
    this.radius = r;
  }
  public override double Area()
  {
    return 22 * radius * radius / 7;
  }
}
class Rectange : Shape
{
  private double length;
  private double bredth;
  public Rectange(double l, double b)
  {
    this.length = l;
    this.bredth = b;
  }
  public override double Area()
  {
    return length * bredth;
  }
}

class Triangle : Shape
{
  private double b;
  private double h;
  public Triangle(double b, double h)
  {
    this.b = b;
    this.h = h;
  }
  public override double Area()
  {
    return 0.5 * b * h;
  }

}


class Strings
{
  public static void Main()
  {
    string[] input = Console.ReadLine().Split(' ');
    if (input[0] == "C")
    {
      Circle c = new Circle(double.Parse(input[1]));
      Console.WriteLine($"The area of the Circle is: {c.Area():F2}");
    }
    else if (input[0] == "R")
    {
      Rectange r = new Rectange(double.Parse(input[1]), double.Parse(input[2]));
      Console.WriteLine($"Area of Rectangle: {r.Area():F2} ");
    }
    else if (input[0] == "T")
    {
      Triangle t = new Triangle(double.Parse(input[1]), double.Parse(input[2]));
      Console.WriteLine($"Area of triangle is: {t.Area():F2}");
    }

  }
}