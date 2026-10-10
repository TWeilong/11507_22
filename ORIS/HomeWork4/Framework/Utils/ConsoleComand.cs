using System.Net;
using http_server.Framework.Http;

namespace http_server.Framework.Utils;

public static class ConsoleCommand
{

    public static void PrintMenu()
    {
        Console.Clear();

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("==========================");
        Console.ResetColor();

        Console.WriteLine("Доступные команды:\n" +
                          "start - запустить сервер\n" +
                          "stop - остановить сервер");

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("==========================");
        Console.ResetColor();


        Console.Write("Введите команду: ");
    }


    public static string InputCommand()
    {
        var command = Console.ReadLine()?.ToLower().Trim() ?? string.Empty;
        return command;
    }

    public static void CommandProcessing(HttpListener serverListener)
    {
        var command = InputCommand();

        switch (command)
        {
            case "start":

                if (serverListener.IsListening)
                {
                    Console.WriteLine("Сервер уже запущен");
                }
                else
                {
                    serverListener.Start();
                }

                break;

            case "stop":
                try
                {
                    if (serverListener.IsListening)
                    {
                        Console.WriteLine("\nСервер остановлен");
                        serverListener.Stop();
                    }
                    else
                    {
                        Console.WriteLine("Сервер не запущен!");
                    }


                }
                catch (HttpListenerException)
                {
                    Console.WriteLine($"Ошибка обработана");
                }
                break;
        }
    }
}
