using MediatR;
using OrderManagement.Api.Entities.Common;

namespace OrderManagement.Api.Entities.Events;

public record SendCreatedOrderEmailEvent(Guid OrderId,string Message):IDomainEvent;