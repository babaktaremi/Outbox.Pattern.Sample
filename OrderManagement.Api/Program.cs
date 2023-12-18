using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Api;
using OrderManagement.Api.Entities.Common;
using OrderManagement.Api.Features.Commands;
using OrderManagement.Api.Features.Queries;
using OrderManagement.Api.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHostedService<EventPublisherWorker>();

builder.Services.AddMongoDb(builder.Configuration["MongoDbConfig:ConnectionString"]!,
    builder.Configuration["MongoDbConfig:DatabaseName"]!);


builder.Services.AddMediatR(configuration =>
{
    configuration.RegisterServicesFromAssemblyContaining<OrderDbContext>();
    configuration.AutoRegisterRequestProcessors = true;
});

builder.Services.AddDbContext<OrderDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("OrdersDb"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();



app.MapPost("/AddOrder", async (ISender sender, AddOrderCommand command) =>
{
    await sender.Send(command);

    return Results.Ok();
});

app.MapGet("/Orders", async (ISender sender) =>
{
    var orders = await sender.Send(new GetOrdersQuery());

    return Results.Ok(orders);
});

app.Run();


