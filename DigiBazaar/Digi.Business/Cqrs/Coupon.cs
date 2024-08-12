using MediatR;
using Digi.Base.Response;
using Digi.Schema;

namespace Digi.Business.Cqrs;

public record CreateCouponCommand(CouponRequest Request) : IRequest<ApiResponse<CouponResponse>>;
public record GenerateCouponCommand(GenerateCouponRequest Request) : IRequest<ApiResponse<CouponResponse>>;
public record DeleteCouponCommand(long CouponId) : IRequest<ApiResponse>;

public record GetAllCouponQuery() : IRequest<ApiResponse<List<CouponResponse>>>;
