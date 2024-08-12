using Digi.Base.Response;
using Digi.Schema;

namespace Digi.Business.Service.Token;

public interface IAuthService
{
    Task<ApiResponse<AuthResponse>> Login(AuthRequest request);
    Task<ApiResponse> Logout();
    Task<ApiResponse> ChangePassword(ChangePasswordRequest request);
    Task<ApiResponse> Register(RegisterUserRequest request);
}