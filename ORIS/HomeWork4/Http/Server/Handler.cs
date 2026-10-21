using System.Net;

namespace http_server.Framework.Abstractions.Base;

public abstract class Handler
{
    public Handler? Successor { get; set; }
    public abstract Task HandlerRequest(HttpListenerContext request);
}
