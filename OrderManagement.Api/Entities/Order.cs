using OrderManagement.Api.Entities.Common;
using OrderManagement.Api.Entities.Events;

namespace OrderManagement.Api.Entities;

public class Order(Guid orderId, string orderName) : BaseEntity
{
    public Guid OrderId { get;private set; } = orderId;

    public string OrderName { get;private set; } = orderName;

    public static Order Create(Guid orderId, string orderName)
    {
        var order = new Order(orderId,orderName);
        order.RaiseEvent(new OrderCreatedEvent(orderId));
        order.RaiseEvent(new SendCreatedOrderEmailEvent(orderId,"Congrats!. Your Order Placed Successfully"));

        return order;
    }
}