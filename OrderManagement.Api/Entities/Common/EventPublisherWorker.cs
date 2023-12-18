using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Api.Persistence;

namespace OrderManagement.Api.Entities.Common;

public class EventPublisherWorker(IServiceScopeFactory scopeFactory, ILogger<EventPublisherWorker> logger)
    : BackgroundService
{
    readonly IServiceScopeFactory _scopeFactory = scopeFactory;
    readonly ILogger<EventPublisherWorker> _logger = logger;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var serviceProvider = _scopeFactory.CreateScope();

                var dbContext = serviceProvider.ServiceProvider.GetRequiredService<OrderDbContext>();
                var publisher = serviceProvider.ServiceProvider.GetRequiredService<IPublisher>();

                var events = await dbContext.OutboxMessages.ToListAsync(cancellationToken: stoppingToken);

                var eventsToRemove = new List<Outbox>();

                foreach (var @event in events)
                {
                    try
                    {
                        await publisher.Publish(@event.DomainEvent, stoppingToken);
                        eventsToRemove.Add(@event);

                    }
                    catch (Exception e)
                    {
                        logger.LogError(e,"There was an error publishing event {eventName}",@event.DomainEvent.GetType().Name);
                    }
                }

                if (eventsToRemove.Any())
                {
                    dbContext.OutboxMessages.RemoveRange(eventsToRemove);

                    await dbContext.SaveChangesAsync(stoppingToken);
                }

            }
            catch (Exception e)
            {
                _logger.LogError(e,e.Message);
            }

            await Task.Delay(2000,stoppingToken);
        }
    }
}