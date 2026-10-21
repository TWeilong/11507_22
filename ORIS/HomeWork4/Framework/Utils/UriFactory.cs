using http_server.Framework.Configuration;

namespace http_server.Framework.Utils;

public static class UriFactory
{
    private static UriBuilder CreateUri(Server server)
    {
        UriBuilder uriBuilder = new();

        uriBuilder.Scheme = "http";
        uriBuilder.Host = server.Host;
        uriBuilder.Port = server.Port;

        return uriBuilder;
    }

    public static Uri GetUri(this Server server)
    {
        return CreateUri(server).Uri;
    }
}