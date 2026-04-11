namespace OrderManagementSystem.Models;

public class Order
{
  public string Id { get; set; } = string.Empty;
  public string ProductId { get; set; } = string.Empty;
  public double Cost { get; set; }
  public DateTime Placed { get; set; }
  public string CustomerId { get; set; } = string.Empty;
  public string Status { get; set; } = string.Empty;
}
