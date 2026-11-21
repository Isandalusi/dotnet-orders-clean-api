using Microsoft.EntityFrameworkCore;
using Orders.Application.Interfaces;
using Orders.Domain.Entities;

namespace Orders.Infrastructure.Persistence;

public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _db;

    public OrderRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<Order>> GetAllAsync(OrderStatus? status = null, CancellationToken ct = default)
    {
        var query = _db.Orders.AsNoTracking();
        if (status.HasValue)
            query = query.Where(o => o.Status == status.Value);

        return await query.OrderByDescending(o => o.CreatedAtUtc).ToListAsync(ct);
    }

    public Task<Order?> GetByIdAsync(int id, CancellationToken ct = default) =>
        _db.Orders.FirstOrDefaultAsync(o => o.Id == id, ct);

    public Task<bool> ExistsByOrderNumberAsync(string orderNumber, CancellationToken ct = default) =>
        _db.Orders.AnyAsync(o => o.OrderNumber == orderNumber, ct);

    public async Task<Order> AddAsync(Order order, CancellationToken ct = default)
    {
        _db.Orders.Add(order);
        await _db.SaveChangesAsync(ct);
        return order;
    }

    public Task SaveChangesAsync(CancellationToken ct = default) =>
        _db.SaveChangesAsync(ct);
}
