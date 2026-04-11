namespace OrderService.Models
{
  public class Order
  {
    public int Id { get; set; }
    public int UserId { get; set; } // The user who placed the order
    public string? ProductName { get; set; }
    public int Quantity { get; set; }
    public decimal TotalPrice { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
  }
}

