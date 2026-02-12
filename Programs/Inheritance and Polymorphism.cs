using System;
using System.Collections.Generic;
using System.Globalization;

public abstract class Employee
{
  public abstract decimal CalculatePay();
}

// Hourly Employee
public class HourlyEmployee : Employee
{
  private decimal Rate;
  private decimal Hours;

  public HourlyEmployee(decimal rate, decimal hours)
  {
    Rate = rate;
    Hours = hours;
  }

  public override decimal CalculatePay()
  {
    return Rate * Hours;
  }
}

// Salaried Employee
public class SalariedEmployee : Employee
{
  private decimal MonthlySalary;

  public SalariedEmployee(decimal monthlySalary)
  {
    MonthlySalary = monthlySalary;
  }

  public override decimal CalculatePay()
  {
    return MonthlySalary;
  }
}

// Commission Employee
public class CommissionEmployee : Employee
{
  private decimal Commission;
  private decimal BaseSalary;

  public CommissionEmployee(decimal commission, decimal baseSalary)
  {
    Commission = commission;
    BaseSalary = baseSalary;
  }

  public override decimal CalculatePay()
  {
    return BaseSalary + Commission;
  }
}

public class PayrollCalculator
{
  public static decimal ComputeTotalPayroll(string[] employees)
  {
    decimal total = 0m;

    foreach (var emp in employees)
    {
      var parts = emp.Split(' ');
      Employee employee = null;

      switch (parts[0])
      {
        case "H":
          employee = new HourlyEmployee(
              decimal.Parse(parts[1], CultureInfo.InvariantCulture),
              decimal.Parse(parts[2], CultureInfo.InvariantCulture));
          break;

        case "S":
          employee = new SalariedEmployee(
              decimal.Parse(parts[1], CultureInfo.InvariantCulture));
          break;

        case "C":
          employee = new CommissionEmployee(
              decimal.Parse(parts[1], CultureInfo.InvariantCulture),
              decimal.Parse(parts[2], CultureInfo.InvariantCulture));
          break;
      }

      total += employee.CalculatePay();
    }

    return Math.Round(total, 2);
  }
}
