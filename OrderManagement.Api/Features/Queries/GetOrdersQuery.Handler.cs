using MediatR;
using MongoDB.Driver;
using OrderManagement.Api.Entities;

namespace OrderManagement.Api.Features.Queries;

public class GetOrdersQueryHandler(IMongoDatabase mongoDatabase):IRequestHandler<GetOrdersQuery,List<GetOrdersQueryResult>>
{
    public async Task<List<GetOrdersQueryResult>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
       

        var emptyQuery = Builders<Order>.Filter.Empty;

        var orders =await mongoDatabase.GetCollection<Order>(nameof(Order)).Find(emptyQuery)
            .Project(c=>new GetOrdersQueryResult(c.OrderId,c.OrderName))
            .ToListAsync(cancellationToken);

        return orders;
    }
}