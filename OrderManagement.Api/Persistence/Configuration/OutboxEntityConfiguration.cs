using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using OrderManagement.Api.Entities;
using OrderManagement.Api.Entities.Common;

namespace OrderManagement.Api.Persistence.Configuration;

public class OutboxEntityConfiguration:IEntityTypeConfiguration<Outbox>
{
    public void Configure(EntityTypeBuilder<Outbox> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.DomainEvent)
            .HasConversion(@event => JsonConvert.SerializeObject(@event, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            }), eventModel => JsonConvert.DeserializeObject<IDomainEvent>(eventModel, new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All
            }));
    }
}