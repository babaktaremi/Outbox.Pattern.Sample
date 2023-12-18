using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderManagement.Api.Entities;

namespace OrderManagement.Api.Persistence.Configuration;

public class OrderEntityConfiguration:IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(c => c.OrderId);

        builder.Property(c => c.OrderName).HasMaxLength(2000);
    }
}