using Microsoft.AspNetCore.Mvc;
using OrderManagementSystem.Models;
using OrderManagementSystem.Repositories;

namespace OrderManagementSystem.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
  private readonly IOrderRepository _orderRepository;

  public OrderController(IOrderRepository orderRepository)
  {
    _orderRepository = orderRepository;
  }

  [HttpGet]
  [Route("GetAllOrders")]
  public async Task<ActionResult> GetAllOrder()
  {
    var orderdet = await _orderRepository.GetAllOrder();
    return Ok(orderdet);
  }

  [HttpPost]
  [Route("Add")]
  public async Task<ActionResult> Add([FromBody] Order orderdet)
  {
    string orderid = await _orderRepository.Add(orderdet);
    return Ok(orderid);
  }

  // [HttpGet]
  // [Route("GetByCustomerId/{id}")]
  // public async Task<ActionResult> GetByCustomerId(string id)
  // {
  //   var orders = await _orderRepository.GetByCustomerId(id);
  //   if (orders == null)
  //   {
  //     return NotFound();
  //   }

  //   return Ok(orders);
  // }

  [HttpGet]
  [Route("GetById/{id}")]
  public async Task<ActionResult> GetById(string id)
  {
    var orderdet = await _orderRepository.GetById(id);
    if (orderdet == null)
    {
      return NotFound();
    }

    return Ok(orderdet);
  }

  [HttpDelete]
  [Route("Cancel/{id}")]
  public async Task<IActionResult> Cancel(string id)
  {
    string resp = await _orderRepository.Cancel(id);
    return Ok(resp);
  }
}
