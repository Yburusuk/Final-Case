using Microsoft.AspNetCore.Http;

namespace Digi.Base.Sessions;

public interface ISessionContext
{
    public HttpContext HttpContext { get; set; }
    public Session Session { get; set; }
}