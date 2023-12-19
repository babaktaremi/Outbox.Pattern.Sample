using MediatR;
using OrderManagement.Api.Entities.Common;

namespace OrderManagement.Api.Features.Publishers.Models;

public record DomainEventWrapper<TDomainEvent>(TDomainEvent DomainEvent) : INotification
    where TDomainEvent:IDomainEvent;