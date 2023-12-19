using MediatR;
using OrderManagement.Api.Entities.Events;
using OrderManagement.Api.Features.Publishers.Models;

namespace OrderManagement.Api.Features.EventHandlers;

public class SendCreatedOrderEmailEventHandler:INotificationHandler<DomainEventWrapper<SendCreatedOrderEmailEvent>>
{
    public Task Handle(DomainEventWrapper<SendCreatedOrderEmailEvent> notification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException(); //Not implemented on purpose . To show and test the capabilities of Outbox Pattern
    }
}