using MediatR;
using Digi.Base.Response;
using Digi.Schema;

namespace Digi.Business.Cqrs;

public record AddProductToCategoryCommand(ProductCategoryRequest Request) : IRequest<ApiResponse<CategoryResponse>>;
public record DeleteProductCategoryCommand(ProductCategoryRequest Request) : IRequest<ApiResponse>;