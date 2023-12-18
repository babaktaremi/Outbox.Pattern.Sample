using MediatR;
using OrderManagement.Api.Entities;
using OrderManagement.Api.Persistence;

namespace OrderManagement.Api.Features.Commands;

public class AddOrderCommandHandler : IRequestHandler<AddOrderCommand>
{
    readonly OrderDbContext _dbContext;

    public AddOrderCommandHandler(OrderDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Handle(AddOrderCommand request, CancellationToken cancellationToken)
    {
        var order = Order.Create(Guid.NewGuid(), request.OrderName);

        _dbContext.Orders.Add(order);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}