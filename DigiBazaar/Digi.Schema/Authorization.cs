namespace Digi.Schema;

public class AuthRequest
{
    public string UserName { get; set; }
    public string Password { get; set; }
}


public class AuthResponse
{
    public long Id { get; set; }
    public DateTime ExpireTime { get; set; }
    public string AccessToken { get; set; }
    public string UserName { get; set; }
}

public class ChangePasswordRequest
{
    public string OldPassword { get; set; }
    public string NewPassword { get; set; }
}


public class  RegisterUserRequest
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}