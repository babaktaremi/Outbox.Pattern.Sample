using MediatR;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using OrderManagement.Api.Entities;
using OrderManagement.Api.Entities.Events;
using OrderManagement.Api.Persistence;

namespace OrderManagement.Api.Features.EventHandlers;

public class OrderCreatedEventHandler(ILogger<OrderCreatedEventHandler> logger,OrderDbContext orderDbContext,IMongoDatabase ordersMongoDatabase)
    : INotificationHandler<OrderCreatedEvent>
{
   
    public async Task Handle(OrderCreatedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogWarning("Order Created With Id {orderId} , Sending Email...",notification.OrderId);

        var order = await orderDbContext.Orders.AsNoTracking()
            .FirstOrDefaultAsync(c => c.OrderId.Equals(notification.OrderId), cancellationToken);

        if (order == null) 
            return;

        var orderCollection =  ordersMongoDatabase.GetCollection<Order>(nameof(Order));

        await orderCollection.InsertOneAsync(order, new InsertOneOptions() { BypassDocumentValidation = false },
            cancellationToken);
    }
}