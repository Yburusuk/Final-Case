using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Digi.Base.Response;
using Digi.Business.Service.Token;
using Digi.Schema;


namespace Digi.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthorizationController : ControllerBase
{
    private readonly IAuthService authService;

    public AuthorizationController(IAuthService authService)
    {
        this.authService = authService;
    }

    [HttpPost("Login")]
    [AllowAnonymous]
    public async Task<ApiResponse<AuthResponse>> Login([FromBody] AuthRequest request)
    {
        var loginResult = await authService.Login(request);
        return loginResult;
    }
    
    [HttpPost("Logout")]
    [AllowAnonymous]
    public async Task<ApiResponse> Logout()
    {
        var response = await authService.Logout();
        return response;
    }
    
    [HttpPost("ChangePassword")]
    [AllowAnonymous]
    public async Task<ApiResponse> Logout([FromBody] ChangePasswordRequest request)
    {
        var response = await authService.ChangePassword(request);
        return response;
    }
    
    [HttpPost("Register")]
    [AllowAnonymous]
    public async Task<ApiResponse> Register([FromBody] RegisterUserRequest request)
    {
        var response = await authService.Register(request);
        return response;
    }

}