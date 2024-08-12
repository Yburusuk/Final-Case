namespace Digi.Base.Sessions;

public class Session
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; }
    public bool IsActive { get; set; }
    public string Email { get; set; }
    public string UserId { get; set; }
    public string Role { get; set; }
    public decimal WalletBalance { get; set; }
}