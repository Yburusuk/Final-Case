using Digi.Base.Response;
using Digi.Business.Cqrs;
using Digi.Schema;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Digi.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrderDetailsController : ControllerBase
{
    private readonly IMediator mediator;
    
    public OrderDetailsController(IMediator mediator)
    {
        this.mediator = mediator;
    }
    
    [HttpGet("{Username}")]
    public async Task<ApiResponse<List<OrderDetailResponse>>> Get(string Username)
    {
        var operation = new GetOrderDetailQuery(Username);
        var result = await mediator.Send(operation);
        return result;
    }

    [HttpGet]
    public async Task<ApiResponse<List<OrderDetailResponse>>> GetAll()
    {
        var operation = new GetAllOrderDetailQuery();
        var result = await mediator.Send(operation);
        return result;
    }
    
    [HttpPost]
    public async Task<ApiResponse<OrderDetailResponse>> Post([FromBody] OrderDetailRequest value)
    {
        var operation = new AddProductToOrderDetailCommand(value);
        var result = await mediator.Send(operation);
        return result;
    }

    [HttpDelete]
    public async Task<ApiResponse<OrderDetailResponse>> Delete(OrderDetailRequest value)
    {
        var operation = new RemoveProductFromOrderDetailCommand(value);
        var result = await mediator.Send(operation);
        return result;
    }
}