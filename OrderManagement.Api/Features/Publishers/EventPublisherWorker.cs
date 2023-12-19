using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Api.Entities;
using OrderManagement.Api.Entities.Common;
using OrderManagement.Api.Entities.Events;
using OrderManagement.Api.Features.Publishers.Models;
using OrderManagement.Api.Persistence;

namespace OrderManagement.Api.Features.Publishers;

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
                      
                        var eventWrapperModel =
                            Activator.CreateInstance(
                                typeof(DomainEventWrapper<>).MakeGenericType(@event.DomainEvent.GetType()),
                                new object[] { @event.DomainEvent });

                        if (eventWrapperModel is not null) 
                            await publisher.Publish(eventWrapperModel, stoppingToken);

                        eventsToRemove.Add(@event);

                    }
                    catch (Exception e)
                    {
                        logger.LogError(e, "There was an error publishing event ");
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
                _logger.LogError(e, e.Message);
            }

            await Task.Delay(2000, stoppingToken);
        }
    }
}