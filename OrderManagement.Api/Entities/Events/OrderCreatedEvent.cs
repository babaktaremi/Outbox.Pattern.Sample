using MediatR;
using OrderManagement.Api.Entities.Common;

namespace OrderManagement.Api.Entities.Events;

public class OrderCreatedEvent:IDomainEvent
{
    public Guid OrderId { get; }

    public OrderCreatedEvent(Guid orderId)
    {
        OrderId = orderId;
    }
}