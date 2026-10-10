using System.Net;
using System.Reflection;

namespace http_server.Framework.Abstractions.Interfaces;

public interface IResourceController
{
    public Task<bool> GetPageNotFound(string filePath, HttpListenerContext context);
    public Task SendErrorPage(string nameError, HttpListenerContext context);

    public Task<bool> CheckNull(Object type, MethodInfo methodInfo, HttpListenerContext context);

}