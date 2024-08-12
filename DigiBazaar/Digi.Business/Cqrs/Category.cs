using MediatR;
using Digi.Base.Response;
using Digi.Schema;

namespace Digi.Business.Cqrs;

public record CreateCategoryCommand(CategoryRequest Request) : IRequest<ApiResponse<CategoryResponse>>;
public record UpdateCategoryCommand(long CategoryId, CategoryRequest Request) : IRequest<ApiResponse>;
public record DeleteCategoryCommand(long CategoryId) : IRequest<ApiResponse>;

public record GetAllCategoryQuery() : IRequest<ApiResponse<List<CategoryResponse>>>;
