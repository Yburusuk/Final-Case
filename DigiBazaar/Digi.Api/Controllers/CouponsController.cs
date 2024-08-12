using MediatR;
using Microsoft.AspNetCore.Mvc;
using Digi.Base.Response;
using Digi.Business.Cqrs;
using Digi.Schema;

namespace Digi.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CouponsController : ControllerBase
{
    private readonly IMediator mediator;
    
    public CouponsController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet]
    public async Task<ApiResponse<List<CouponResponse>>> Get()
    {
        var operation = new GetAllCouponQuery();
        var result = await mediator.Send(operation);
        
        return result;
    }
    
    [HttpPost]
    public async Task<ApiResponse<CouponResponse>> Post([FromBody] CouponRequest value)
    {
        var operation = new CreateCouponCommand(value);
        var result = await mediator.Send(operation);
        
        return result;
    }
    
    [HttpPost("Generate")]
    public async Task<ApiResponse<CouponResponse>> GenerateCoupon([FromBody] GenerateCouponRequest value)
    {
        var operation = new GenerateCouponCommand(value);
        var result = await mediator.Send(operation);
        
        return result;
    }
    
    [HttpDelete]
    public async Task<ApiResponse> Delete(long CouponId)
    {
        var operation = new DeleteCouponCommand(CouponId);
        var result = await mediator.Send(operation);
        
        return result;
    }
}