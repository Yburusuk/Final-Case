using MediatR;
using Microsoft.AspNetCore.Mvc;
using Digi.Base.Response;
using Digi.Business.Cqrs;
using Digi.Schema;

namespace Digi.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WalletBalancesController : ControllerBase
{
    private readonly IMediator mediator;
    
    public WalletBalancesController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet]
    public async Task<ApiResponse<decimal>> Get(string Username)
    {
        var operation = new GetWalletBalanceQuery(Username);
        var result = await mediator.Send(operation);
        
        return result;
    }
}