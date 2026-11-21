using Microsoft.AspNetCore.Mvc;
using Orders.Application.DTOs;
using Orders.Application.Services;
using Orders.Domain.Entities;

namespace Orders.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly OrderService _service;

    public OrdersController(OrderService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<OrderResponse>>> GetAll([FromQuery] OrderStatus? status, CancellationToken ct)
    {
        var orders = await _service.GetAllAsync(status, ct);
        return Ok(orders);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<OrderResponse>> GetById(int id, CancellationToken ct)
    {
        var order = await _service.GetByIdAsync(id, ct);
        return order is null ? NotFound() : Ok(order);
    }

    [HttpPost]
    public async Task<ActionResult<OrderResponse>> Create([FromBody] CreateOrderRequest request, CancellationToken ct)
    {
        try
        {
            var created = await _service.CreateAsync(request, ct);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { error = ex.Message });
        }
    }

    [HttpPost("{id:int}/confirm")]
    public async Task<ActionResult<OrderResponse>> Confirm(int id, CancellationToken ct)
    {
        try
        {
            var updated = await _service.ConfirmAsync(id, ct);
            return updated is null ? NotFound() : Ok(updated);
        }
        catch (InvalidOperationException ex)
        {
            return UnprocessableEntity(new { error = ex.Message });
        }
    }

    [HttpPost("{id:int}/dispatch")]
    public async Task<ActionResult<OrderResponse>> Dispatch(int id, CancellationToken ct)
    {
        try
        {
            var updated = await _service.DispatchAsync(id, ct);
            return updated is null ? NotFound() : Ok(updated);
        }
        catch (InvalidOperationException ex)
        {
            return UnprocessableEntity(new { error = ex.Message });
        }
    }
}
