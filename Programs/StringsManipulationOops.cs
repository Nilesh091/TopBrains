using System;
using System.Globalization;

public interface IShape
{
  double GetArea();
}

public abstract class Shape : IShape
{
  public abstract double GetArea();
}


public class Circle : Shape
{
  private double Radius;

  public Circle(double radius)
  {
    Radius = radius;
  }

  public override double GetArea()
  {
    return Math.PI * Radius * Radius;
  }
}

public class Rectangle : Shape
{
  private double Width;
  private double Height;

  public Rectangle(double width, double height)
  {
    Width = width;
    Height = height;
  }

  public override double GetArea()
  {
    return Width * Height;
  }
}

public class Triangle : Shape
{
  private double Base;
  private double Height;

  public Triangle(double b, double h)
  {
    Base = b;
    Height = h;
  }

  public override double GetArea()
  {
    return 0.5 * Base * Height;
  }
}

public class AreaCalculator
{
  public static double ComputeTotalArea(string[] shapes)
  {
    double total = 0.0;

    foreach (var shape in shapes)
    {
      var parts = shape.Split(' ');
      IShape obj = null;

      switch (parts[0])
      {
        case "C":
          obj = new Circle(
              double.Parse(parts[1], CultureInfo.InvariantCulture));
          break;
        case "R":
          obj = new Rectangle(
              double.Parse(parts[1], CultureInfo.InvariantCulture),
              double.Parse(parts[2], CultureInfo.InvariantCulture));
          break;
        case "T":
          obj = new Triangle(
              double.Parse(parts[1], CultureInfo.InvariantCulture),
              double.Parse(parts[2], CultureInfo.InvariantCulture));
          break;
      }

      total += obj.GetArea();
    }

    return Math.Round(total, 2, MidpointRounding.AwayFromZero);
  }
}
