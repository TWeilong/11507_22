using System.Net;
using http_server.Framework.Attributes;
using http_server.Framework.Services;

namespace http_server.Framework.HttpControllers;

[HttpController("auth")]
public class AuthController(HttpListenerResponse response)
{
    [Get("login")]
    public async Task Login()
    {
        Console.WriteLine("Запрос пришел");
        var fileService = FileService.Instance;
        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "static", "rockstargames", "index.html");
        await fileService.FilePush(filePath, response);
    }

    [Post("login")]
    public void Login(string email, string password)
    {
        Console.WriteLine($"Емаил: {email} | Пароль: {password}");
    }
}