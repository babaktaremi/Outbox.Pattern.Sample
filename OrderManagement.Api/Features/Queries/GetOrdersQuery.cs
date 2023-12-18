using MediatR;

namespace OrderManagement.Api.Features.Queries;

public record GetOrdersQuery():IRequest<List<GetOrdersQueryResult>>;