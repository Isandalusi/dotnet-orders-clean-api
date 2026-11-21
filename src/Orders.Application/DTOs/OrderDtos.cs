using Orders.Domain.Entities;

namespace Orders.Application.DTOs;

public record CreateOrderRequest(string OrderNumber, string CustomerName, decimal TotalAmount);

public record OrderResponse(
    int Id,
    string OrderNumber,
    string CustomerName,
    decimal TotalAmount,
    string Status,
    DateTime CreatedAtUtc)
{
    public static OrderResponse FromEntity(Order order) => new(
        order.Id,
        order.OrderNumber,
        order.CustomerName,
        order.TotalAmount,
        order.Status.ToString(),
        order.CreatedAtUtc);
}
