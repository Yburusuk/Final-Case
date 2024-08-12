using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Digi.Base.Response;
using Digi.Base.Sessions;
using Digi.Base.Token;
using Digi.Data.Domain;
using Digi.Data.UnitOfWork;
using Digi.Schema;

namespace Digi.Business.Service.Token;

public class AuthService : IAuthService
{
    private readonly JwtConfig jwtConfig;
    private readonly UserManager<ApplicationUser> userManager;
    private readonly SignInManager<ApplicationUser> signInManager;
    private readonly ISessionContext sessionContext;
    private readonly IUnitOfWork unitOfWork;
    
    public AuthService(JwtConfig jwtConfig,UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager, 
        ISessionContext sessionContext, IUnitOfWork unitOfWork)
    {
        this.userManager = userManager;
        this.signInManager = signInManager;
        this.jwtConfig = jwtConfig;
        this.sessionContext = sessionContext;
        this.unitOfWork = unitOfWork;
    }
    
    public async Task<ApiResponse<AuthResponse>> Login(AuthRequest request)
    {
        var loginResult = await signInManager.PasswordSignInAsync(request.UserName, request.Password, true, false);
        if (!loginResult.Succeeded)
        {
            return new ApiResponse<AuthResponse>("Login Faild");
        }

        var user = await userManager.FindByNameAsync(request.UserName);
        if (user == null)
        {
            return new ApiResponse<AuthResponse>("Login Faild");
        }
        
        var responseToken = await GenerateToken(user);
        AuthResponse authResponse = new AuthResponse()
        {
            AccessToken = responseToken,
            UserName = request.UserName,
            ExpireTime = DateTime.Now.AddMinutes(jwtConfig.AccessTokenExpiration)
        };

        return new ApiResponse<AuthResponse>(authResponse);
    }

    public async Task<ApiResponse> Logout()
    {
       await signInManager.SignOutAsync();
       return new ApiResponse();
    }

    public async Task<ApiResponse> ChangePassword(ChangePasswordRequest request)
    {
        ApplicationUser applicationUser = await userManager.GetUserAsync(sessionContext.HttpContext.User);
        if (applicationUser == null)
        {
            return new ApiResponse("Login failed.");
        }
        var user = await userManager.FindByNameAsync(applicationUser.UserName);
        if (user == null)
        {
            return new ApiResponse("Login failed.");
        }

        await userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);
        return new ApiResponse();
    }

    public async Task<ApiResponse> Register(RegisterUserRequest request)
    {
        var newUser = new ApplicationUser
        {
            
            FirstName = request.FirstName,
            LastName = request.LastName,
            UserName = request.UserName,
            Email = request.Email,
            Role = "normal",
            WalletBalance = 0,
            CreatedDate = DateTime.Now,
            IsActive = true,
            EmailConfirmed = true,
            TwoFactorEnabled = false,
        };

        var newUserResponse = await userManager.CreateAsync(newUser, request.Password);
        if (!newUserResponse.Succeeded)
        {
            return new ApiResponse("Register failed.");
        }
        
        var newOrder = new Order
         {
             UserId = newUser.Id,
             UserName = newUser.UserName,
             OrderNumber = Guid.NewGuid().ToString("N").Substring(0, 9),
             TotalAmount = 0,
             CouponCode = "",
             CouponAmount = 0,
             PointsSpent = 0,
             PointsEarned = 0
         };

         await unitOfWork.OrderRepository.Insert(newOrder);
         await unitOfWork.Complete();
         
         var order = unitOfWork.OrderRepository.FirstOrDefault(x => x.OrderNumber == newOrder.OrderNumber);
         
         var orderDetails = new OrderDetail
         {
             UserId = newUser.Id,
             UserName = newUser.UserName,
             OrderId = order.Id,
         };
         
         await unitOfWork.OrderDetailRepository.Insert(orderDetails);
         await unitOfWork.Complete();

        return new ApiResponse();
    }

    public async Task<string> GenerateToken(ApplicationUser user)
    {
        Claim[] claims = GetClaims(user);
        var secret = Encoding.ASCII.GetBytes(jwtConfig.Secret);

        JwtSecurityToken jwtToken = new JwtSecurityToken(
            jwtConfig.Issuer,
            jwtConfig.Audience,
            claims,
            expires: DateTime.Now.AddMinutes(jwtConfig.AccessTokenExpiration),
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(secret),
                SecurityAlgorithms.HmacSha256Signature)
        );

        string token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
        return token;
    }

    private Claim[] GetClaims(ApplicationUser user)
    {
        List<Claim> claims = new List<Claim>()
        {
            new Claim("UserName", user.UserName),
            new Claim("UserId", user.Id.ToString()),
            new Claim("Email", user.Email),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Email, user.Email),
        };

        return claims.ToArray();
    }
}