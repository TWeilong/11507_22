using http_server.Framework.Abstractions.Interfaces;

namespace http_server.Framework.Http;

public class ResourceValidator : IResourceValidator
{
    public bool IsValid(string path)
    {
        if (File.Exists(path))
        {
            return true;
        }
        return false;
    }
}