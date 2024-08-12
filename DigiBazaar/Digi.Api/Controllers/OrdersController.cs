using Digi.Base.Response;
using Digi.Business.Cqrs;
using Digi.Schema;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Digi.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrdersController : ControllerBase
{
    private readonly IMediator mediator;
    
    public OrdersController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet]
    public async Task<ApiResponse<List<OrderResponse>>> Get(bool IsActive = true)
    {
        var operation = new GetOrderByStatusQuery(IsActive);
        var result = await mediator.Send(operation);
        return result;
    }
    
    [HttpPost]
    public async Task<ApiResponse<OrderResponse>> Post([FromBody] OrderRequest request)
    {
        var operation = new BuyOrderCommand(request);
        var result = await mediator.Send(operation);
        return result;
    }
}