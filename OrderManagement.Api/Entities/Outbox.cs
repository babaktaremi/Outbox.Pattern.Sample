using OrderManagement.Api.Entities.Common;

namespace OrderManagement.Api.Entities;

public class Outbox(Guid id, IDomainEvent domainEvent)
{
    public Guid Id { get; set; } = id;
    public IDomainEvent DomainEvent { get; set; } = domainEvent;
}