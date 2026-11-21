using Microsoft.EntityFrameworkCore;
using Orders.Application.DTOs;
using Orders.Application.Services;
using Orders.Domain.Entities;
using Orders.Infrastructure.Persistence;
using Xunit;

namespace Orders.Tests;

public class OrderServiceTests
{
    private static OrderService CreateService(string dbName)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(dbName)
            .Options;
        var context = new AppDbContext(options);
        var repo = new OrderRepository(context);
        return new OrderService(repo);
    }

    [Fact]
    public async Task CreateAsync_NormalizesOrderNumberAndStartsPending()
    {
        var service = CreateService(nameof(CreateAsync_NormalizesOrderNumberAndStartsPending));

        var created = await service.CreateAsync(new CreateOrderRequest("ord-1001", "Acme Logistics", 25000m));

        Assert.Equal("ORD-1001", created.OrderNumber);
        Assert.Equal("Pending", created.Status);
        Assert.True(created.Id > 0);
    }

    [Fact]
    public async Task CreateAsync_DuplicateOrderNumber_ThrowsInvalidOperationException()
    {
        var service = CreateService(nameof(CreateAsync_DuplicateOrderNumber_ThrowsInvalidOperationException));
        await service.CreateAsync(new CreateOrderRequest("ORD-2001", "Cliente A", 1000m));

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            service.CreateAsync(new CreateOrderRequest("ord-2001", "Cliente B", 2000m)));
    }

    [Fact]
    public async Task ConfirmAndDispatch_ValidTransitions_Succeed()
    {
        var service = CreateService(nameof(ConfirmAndDispatch_ValidTransitions_Succeed));
        var created = await service.CreateAsync(new CreateOrderRequest("ORD-3001", "Cliente C", 4500m));

        var confirmed = await service.ConfirmAsync(created.Id);
        var dispatched = await service.DispatchAsync(created.Id);

        Assert.NotNull(confirmed);
        Assert.Equal("Confirmed", confirmed!.Status);
        Assert.NotNull(dispatched);
        Assert.Equal("Dispatched", dispatched!.Status);
    }

    [Fact]
    public async Task DispatchAsync_WhenPending_ThrowsInvalidOperationException()
    {
        var service = CreateService(nameof(DispatchAsync_WhenPending_ThrowsInvalidOperationException));
        var created = await service.CreateAsync(new CreateOrderRequest("ORD-4001", "Cliente D", 3000m));

        await Assert.ThrowsAsync<InvalidOperationException>(() => service.DispatchAsync(created.Id));
    }

    [Fact]
    public void Cancel_WhenDispatched_ThrowsInvalidOperationException()
    {
        var order = new Order("ORD-5001", "Cliente E", 1500m);
        order.Confirm();
        order.Dispatch();

        Assert.Throws<InvalidOperationException>(() => order.Cancel());
    }
}
