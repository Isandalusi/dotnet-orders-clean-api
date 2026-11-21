namespace Orders.Domain.Entities;

public enum OrderStatus
{
    Pending = 0,
    Confirmed = 1,
    Dispatched = 2,
    Cancelled = 3
}

public class Order
{
    public int Id { get; private set; }
    public string OrderNumber { get; private set; } = string.Empty;
    public string CustomerName { get; private set; } = string.Empty;
    public decimal TotalAmount { get; private set; }
    public OrderStatus Status { get; private set; } = OrderStatus.Pending;
    public DateTime CreatedAtUtc { get; private set; } = DateTime.UtcNow;

    private Order() { }

    public Order(string orderNumber, string customerName, decimal totalAmount)
    {
        if (string.IsNullOrWhiteSpace(orderNumber))
            throw new ArgumentException("OrderNumber is required.", nameof(orderNumber));
        if (string.IsNullOrWhiteSpace(customerName))
            throw new ArgumentException("CustomerName is required.", nameof(customerName));
        if (totalAmount <= 0)
            throw new ArgumentException("TotalAmount must be greater than zero.", nameof(totalAmount));

        OrderNumber = orderNumber.Trim().ToUpperInvariant();
        CustomerName = customerName.Trim();
        TotalAmount = totalAmount;
        Status = OrderStatus.Pending;
        CreatedAtUtc = DateTime.UtcNow;
    }

    public void Confirm()
    {
        if (Status != OrderStatus.Pending)
            throw new InvalidOperationException("Only pending orders can be confirmed.");
        Status = OrderStatus.Confirmed;
    }

    public void Dispatch()
    {
        if (Status != OrderStatus.Confirmed)
            throw new InvalidOperationException("Only confirmed orders can be dispatched.");
        Status = OrderStatus.Dispatched;
    }

    public void Cancel()
    {
        if (Status == OrderStatus.Dispatched)
            throw new InvalidOperationException("Dispatched orders cannot be cancelled.");
        Status = OrderStatus.Cancelled;
    }
}
