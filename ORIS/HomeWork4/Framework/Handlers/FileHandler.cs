using System.Net;
using http_server.Framework.Abstractions.Base;
using http_server.Framework.Abstractions.Interfaces;
using http_server.Framework.Services;
using ResourceController = http_server.Framework.HttpControllers.ResourceController;

namespace http_server.Framework.Handlers;

internal class FileHandler(IResourceValidator resourceValidator
                            , IFileService fileService)
                            : Handler
{

    public override async Task HandlerRequest(HttpListenerContext context)
    {
        var request = context.Request;

        Console.WriteLine($"\nЗапрос пришел : {request.Url?.LocalPath}");

        var userPath = request.Url?.LocalPath.Trim('/');

        bool isFile = userPath!.Contains(".");

        if (isFile)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "static", userPath);

            var response = context.Response;

            Console.WriteLine(filePath);


            ResourceController resourceController = new(resourceValidator, fileService);


            var result = await resourceController.GetPageNotFound(filePath, context);
            if (!result) return;

            await FileService.Instance.FilePush(filePath, response);

            Console.WriteLine("Запрос обработан");
            context.Response.Close();
        }

        else if (Successor != null)
        {
            await Successor.HandlerRequest(context);
        }
    }
}