using System.Security.Claims;
using Digi.Base.Sessions;
using Microsoft.AspNetCore.Http;

namespace Digi.Base.Token;

public static class JwtManager
{
    public static Session GetSession(HttpContext context)
    {
        Session session = new Session();
        var identity = context.User.Identity as ClaimsIdentity;
        var claims = identity.Claims;
        
        session.FirstName = GetClaimValue(claims, "FirstName");
        session.LastName = GetClaimValue(claims, "LastName");
        session.UserName = GetClaimValue(claims, "UserName");
        session.IsActive = Convert.ToBoolean(GetClaimValue(claims, "IsActive"));
        session.UserId = GetClaimValue(claims, "UserId");
        session.Role = GetClaimValue(claims, "Role");
        session.Email = GetClaimValue(claims, "Email");
        session.WalletBalance = Convert.ToDecimal(GetClaimValue(claims, "WalletBalance"));
        
        return session;
    }

    private static string GetClaimValue(IEnumerable<Claim> claims, string name)
    {
        var claim = claims.FirstOrDefault(c => c.Type == name);
        return claim?.Value;
    }
}