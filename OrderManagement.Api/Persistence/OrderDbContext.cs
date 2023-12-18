using Microsoft.EntityFrameworkCore;
using OrderManagement.Api.Entities;
using OrderManagement.Api.Entities.Common;

namespace OrderManagement.Api.Persistence;

public class OrderDbContext:DbContext
{
    public OrderDbContext(DbContextOptions<OrderDbContext> options):base(options)
    {
        
    }

    public DbSet<Outbox> OutboxMessages => base.Set<Outbox>();
    public DbSet<Order> Orders=> base.Set<Order>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(Order).Assembly);

        base.OnModelCreating(modelBuilder);
    }

    private void AddOutboxMessages()
    {
        var messages = base.ChangeTracker.Entries<BaseEntity>()
            .Select(c => c.Entity)
            .SelectMany(c =>
            {
                var domainEvents =new List<IDomainEvent>(c.GetDomainEvents);

                c.ClearEvents();

                return domainEvents;
            }).ToList();

        foreach (var message in messages)
        {
            this.OutboxMessages.Add(new Outbox(Guid.NewGuid(), message));
        }
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        AddOutboxMessages();

        return await base.SaveChangesAsync(cancellationToken);
    }
}