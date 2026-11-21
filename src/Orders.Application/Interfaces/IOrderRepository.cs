using Orders.Domain.Entities;

namespace Orders.Application.Interfaces;

public interface IOrderRepository
{
    Task<List<Order>> GetAllAsync(OrderStatus? status = null, CancellationToken ct = default);
    Task<Order?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<bool> ExistsByOrderNumberAsync(string orderNumber, CancellationToken ct = default);
    Task<Order> AddAsync(Order order, CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
}
