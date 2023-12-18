using MongoDB.Bson.Serialization;
using OrderManagement.Api.Entities;

namespace OrderManagement.Api.Persistence.Configuration;

public static class MongoDbMapping
{
    public static void RegisterOrderMapping()
    {
        BsonClassMap.RegisterClassMap<Order>(c =>
        {
            c.AutoMap();
            c.MapIdProperty(p => p.OrderId);

        });
    }
}