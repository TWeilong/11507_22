using System.Net;
using System.Reflection;
using http_server.Framework.Abstractions.Interfaces;

namespace http_server.Framework.HttpControllers;

public class ResourceController(IResourceValidator resourceValidator,
                                IFileService fileService)
                                : IResourceController
{

    public async Task<bool> GetPageNotFound(string filePath, HttpListenerContext context)
    {
        if (!resourceValidator.IsValid(filePath))
        {
            await SendErrorPage("404", context);
            return false;
        }
        return true;
    }

    public async Task<bool> CheckNull(Object? type, MethodInfo? methodInfo, HttpListenerContext context)
    {
        if (type is null || methodInfo is null)
        {
            await SendErrorPage("404", context);
            return false;
        }
        return true;
    }

    // TODO доделать обработку ошибок - ввел не число -> КАБУМ 
    public async Task SendErrorPage(string nameError, HttpListenerContext context)
    {
        var currentPath = Directory.GetCurrentDirectory();

        var pagePath = Path.Combine(currentPath, "static", "errorPages", $"{nameError}.html");

        var response = context.Response;
        response.StatusCode = int.Parse(nameError);

        await fileService.FilePush(pagePath, response);
    }

}