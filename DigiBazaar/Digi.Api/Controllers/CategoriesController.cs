using Digi.Base.Response;
using Digi.Business.Cqrs;
using Digi.Schema;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Digi.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly IMediator mediator;
    
    public CategoriesController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet]
    public async Task<ApiResponse<List<CategoryResponse>>> Get()
    {
        var operation = new GetAllCategoryQuery();
        var result = await mediator.Send(operation);
        return result;
    }

    [HttpPost]
    public async Task<ApiResponse<CategoryResponse>> Post([FromBody] CategoryRequest value)
    {
        var operation = new CreateCategoryCommand(value);
        var result = await mediator.Send(operation);
        return result;
    }

    [HttpPut]
    public async Task<ApiResponse> Put(long CategoryId, CategoryRequest value)
    {
        var operation = new UpdateCategoryCommand(CategoryId, value);
        var result = await mediator.Send(operation);
        return result;
    }

    [HttpDelete("{CategoryId}")]
    public async Task<ApiResponse> Delete(long CategoryId)
    {
        var operation = new DeleteCategoryCommand(CategoryId);
        var result = await mediator.Send(operation);
        return result;
    }
}