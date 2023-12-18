using MediatR;
using OrderManagement.Api.Entities.Events;

namespace OrderManagement.Api.Features.EventHandlers;

public class SendCreatedOrderEmailEventHandler:INotificationHandler<SendCreatedOrderEmailEvent>
{
    public Task Handle(SendCreatedOrderEmailEvent notification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException(); //Not implemented on purpose . To show and test the capabilities of Outbox Pattern
    }
}