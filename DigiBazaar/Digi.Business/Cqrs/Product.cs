using MediatR;
using Digi.Base.Response;
using Digi.Schema;

namespace Digi.Business.Cqrs;

public record CreateProductCommand(ProductRequest Request) : IRequest<ApiResponse<ProductResponse>>;
public record UpdateProductCommand(long ProductId, ProductRequest Request, long CategoryId, bool IsActive) : IRequest<ApiResponse>;
public record UpdateProductStockCommand(long ProductId, int Stock) : IRequest<ApiResponse>;
public record DeleteProductCommand(long ProductId) : IRequest<ApiResponse>;

public record GetAllProductQuery() : IRequest<ApiResponse<List<ProductResponse>>>;
public record GetProductByCategoryQuery(string CategoryName) : IRequest<ApiResponse<List<ProductResponse>>>;
public record GetProductByStatusQuery(bool IsActive) : IRequest<ApiResponse<List<ProductResponse>>>;