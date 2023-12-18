namespace OrderManagement.Api.Entities.Common;

/// <summary>
/// Base entity that aggregates can inherit from
/// </summary>
public abstract class BaseEntity
{
    private readonly List<IDomainEvent> _domainEvents=new ();


    public void RaiseEvent(IDomainEvent @event) => _domainEvents.Add(@event);

    public void ClearEvents() => _domainEvents.Clear();

    public List<IDomainEvent> GetDomainEvents => _domainEvents;
}