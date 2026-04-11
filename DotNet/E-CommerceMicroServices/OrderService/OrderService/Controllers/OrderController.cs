using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderService.Models;
using OrderService.Repositories;

namespace OrderService.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  [Authorize]
  public class OrderController : ControllerBase
  {
    private readonly IOrderRepository _orderRepository;

    public OrderController(IOrderRepository orderRepository)
    {
      _orderRepository = orderRepository;
    }

    // 🔹 Get All Orders
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
    {
      return Ok(await _orderRepository.GetAllOrders());
    }

    // 🔹 Get Order by ID
    [HttpGet("{id}")]
    public async Task<ActionResult<Order>> GetOrder(int id)
    {
      var order = await _orderRepository.GetOrderById(id);
      if (order == null) return NotFound();
      return Ok(order);
    }

    // 🔹 Create a New Order
    [HttpPost]
    public async Task<ActionResult<Order>> CreateOrder([FromBody] Order order)
    {
      var newOrder = await _orderRepository.CreateOrder(order);
      return CreatedAtAction(nameof(GetOrder), new { id = newOrder.Id }, newOrder);
    }

    // 🔹 Update an Order
    [HttpPut("{id}")]
    public async Task<ActionResult<Order>> UpdateOrder(int id, [FromBody] Order order)
    {
      if (id != order.Id) return BadRequest("Order ID mismatch");

      await _orderRepository.UpdateOrder(order);
      return Ok(order);
    }

    // 🔹 Delete an Order
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteOrder(int? id)
    {
      if (id == null) return NotFound();
      await _orderRepository.DeleteOrder(id);
      return NoContent();
    }
  }
}
