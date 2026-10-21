using System.Net;
using http_server.Framework.Services;

namespace http_server.Framework.Abstractions.Interfaces;

public interface IFileService
{
    Task FilePush(string filePath, HttpListenerResponse response);
}