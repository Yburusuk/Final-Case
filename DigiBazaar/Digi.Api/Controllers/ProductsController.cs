using Digi.Base.Response;
using Digi.Business.Cqrs;
using Digi.Schema;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Digi.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IMediator mediator;
    
    public ProductsController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet]
    public async Task<ApiResponse<List<ProductResponse>>> Get()
    {
        var operation = new GetAllProductQuery();
        var result = await mediator.Send(operation);
        return result;
    }

    [HttpGet("ByCategory/{CategoryName}")]
    public async Task<ApiResponse<List<ProductResponse>>> GetByCategory([FromRoute]string CategoryName)
    {
        var operation = new GetProductByCategoryQuery(CategoryName);
        var result = await mediator.Send(operation);
        return result;
    }
    
    [HttpGet("ByStatus/{IsActive}")]
    public async Task<ApiResponse<List<ProductResponse>>> GetByStatus([FromRoute]bool IsActive)
    {
        var operation = new GetProductByStatusQuery(IsActive);
        var result = await mediator.Send(operation);
        return result;
    }

    [HttpPost]
    public async Task<ApiResponse<ProductResponse>> Post([FromBody] ProductRequest value)
    {
        var operation = new CreateProductCommand(value);
        var result = await mediator.Send(operation);
        return result;
    }

    [HttpPut]
    public async Task<ApiResponse> Put(long ProductId, ProductRequest value, long CategoryId = 0, bool IsActive = true)
    {
        var operation = new UpdateProductCommand(ProductId, value, CategoryId, IsActive);
        var result = await mediator.Send(operation);
        return result;
    }
    
    [HttpPut("{ProductId}/stock")]
    public async Task<ApiResponse> StockUpdate(long ProductId, int stock)
    {
        var operation = new UpdateProductStockCommand(ProductId, stock);
        var result = await mediator.Send(operation);
        return result;
    }

    [HttpDelete("{ProductId}")]
    public async Task<ApiResponse> Delete(long ProductId)
    {
        var operation = new DeleteProductCommand(ProductId);
        var result = await mediator.Send(operation);
        return result;
    }
}