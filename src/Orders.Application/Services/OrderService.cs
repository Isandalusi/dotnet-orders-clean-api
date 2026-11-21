using Orders.Application.DTOs;
using Orders.Application.Interfaces;
using Orders.Domain.Entities;

namespace Orders.Application.Services;

public class OrderService
{
    private readonly IOrderRepository _repository;

    public OrderService(IOrderRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<OrderResponse>> GetAllAsync(OrderStatus? status = null, CancellationToken ct = default)
    {
        var orders = await _repository.GetAllAsync(status, ct);
        return orders.Select(OrderResponse.FromEntity).ToList();
    }

    public async Task<OrderResponse?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var order = await _repository.GetByIdAsync(id, ct);
        return order is null ? null : OrderResponse.FromEntity(order);
    }

    public async Task<OrderResponse> CreateAsync(CreateOrderRequest request, CancellationToken ct = default)
    {
        var normalized = request.OrderNumber.Trim().ToUpperInvariant();
        if (await _repository.ExistsByOrderNumberAsync(normalized, ct))
            throw new InvalidOperationException($"Order '{normalized}' already exists.");

        var order = new Order(normalized, request.CustomerName, request.TotalAmount);
        var created = await _repository.AddAsync(order, ct);
        return OrderResponse.FromEntity(created);
    }

    public async Task<OrderResponse?> ConfirmAsync(int id, CancellationToken ct = default)
    {
        var order = await _repository.GetByIdAsync(id, ct);
        if (order is null) return null;
        order.Confirm();
        await _repository.SaveChangesAsync(ct);
        return OrderResponse.FromEntity(order);
    }

    public async Task<OrderResponse?> DispatchAsync(int id, CancellationToken ct = default)
    {
        var order = await _repository.GetByIdAsync(id, ct);
        if (order is null) return null;
        order.Dispatch();
        await _repository.SaveChangesAsync(ct);
        return OrderResponse.FromEntity(order);
    }
}
