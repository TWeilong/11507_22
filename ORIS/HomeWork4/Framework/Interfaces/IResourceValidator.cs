namespace http_server.Framework.Abstractions.Interfaces;

public interface IResourceValidator
{
    bool IsValid(string path);
}