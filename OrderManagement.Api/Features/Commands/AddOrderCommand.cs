using MediatR;

namespace OrderManagement.Api.Features.Commands;

public record AddOrderCommand(string OrderName) : IRequest;