using MediatR;
using Digi.Base.Response;
using Digi.Schema;

namespace Digi.Business.Cqrs;

public record AddProductToOrderDetailCommand(OrderDetailRequest Request) : IRequest<ApiResponse<OrderDetailResponse>>;
public record RemoveProductFromOrderDetailCommand(OrderDetailRequest Request) : IRequest<ApiResponse<OrderDetailResponse>>;

public record GetOrderDetailQuery(string Username) : IRequest<ApiResponse<List<OrderDetailResponse>>>;
public record GetAllOrderDetailQuery() : IRequest<ApiResponse<List<OrderDetailResponse>>>;