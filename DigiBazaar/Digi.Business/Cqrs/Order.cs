using MediatR;
using Digi.Base.Response;
using Digi.Schema;

namespace Digi.Business.Cqrs;

public record BuyOrderCommand(OrderRequest Request) : IRequest<ApiResponse<OrderResponse>>;
public record GetOrderByStatusQuery(bool IsActive) : IRequest<ApiResponse<List<OrderResponse>>>;