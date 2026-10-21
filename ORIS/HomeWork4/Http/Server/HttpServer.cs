using System.Net;
using http_server.Framework.Abstractions.Base;
using http_server.Framework.Utils;

namespace http_server.Framework.Http;

public class HttpServer(
    HttpListener listener,
    Handler fileHandler,
    Handler controllerHandler)
{

    private readonly HttpListener _server = listener
                                            ?? throw new ArgumentNullException($"{nameof(listener)} не должен быть null");

    private readonly Handler _fileHandler = fileHandler
                                            ?? throw new ArgumentNullException($"{nameof(fileHandler)} не должен быть null");

    private readonly Handler _controllerHandler = controllerHandler
                                                  ?? throw new ArgumentNullException($"{nameof(controllerHandler)} не должен быть null");


    private void StartServer()
    {
        var prefix = DataDeserializer.Prefix;
        var server = prefix.Server;

        Uri uri = server.GetUri();
        var uriString = uri.ToString();

        _server.Prefixes.Add(uriString);

        while (!_server.IsListening)
        {
            ConsoleCommand.PrintMenu();
            ConsoleCommand.CommandProcessing(_server);
        }

        Console.Clear();
        Console.WriteLine($"Сервер запущен\n{uriString}");
    }

    private async Task AsyncListen()
    {
        try
        {
            while (_server.IsListening)
            {
                var context = await _server.GetContextAsync();
                await AsyncCreateResponse(context);
            }
        }
        catch (HttpListenerException)
        {
            Console.WriteLine("Ошибка обработана");
        }
    }

    private async Task AsyncCreateResponse(HttpListenerContext context)
    {

        _fileHandler.Successor = _controllerHandler;
        await _fileHandler.HandlerRequest(context);

    }


    public void Run()
    {

        StartServer();
        var taskCommandProcessing = Task.Run(() =>
        {
            while (_server.IsListening)
            {
                ConsoleCommand.CommandProcessing(_server);
            }
        });


        var taskAsyncListen = Task.Run(async () => { await AsyncListen(); });

        Task.WaitAll(taskAsyncListen, taskCommandProcessing);
    }
}