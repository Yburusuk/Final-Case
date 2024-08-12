using Digi.Base.Response;
using Digi.Business.Cqrs;
using Digi.Schema;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Digi.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductCategoriesController : ControllerBase
{
    private readonly IMediator mediator;
    
    public ProductCategoriesController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpPost]
    public async Task<ApiResponse<CategoryResponse>> Post([FromBody] ProductCategoryRequest value)
    {
        var operation = new AddProductToCategoryCommand(value);
        var result = await mediator.Send(operation);
        return result;
    }

    [HttpDelete]
    public async Task<ApiResponse> Delete(ProductCategoryRequest value)
    {
        var operation = new DeleteProductCategoryCommand(value);
        var result = await mediator.Send(operation);
        return result;
    }
}