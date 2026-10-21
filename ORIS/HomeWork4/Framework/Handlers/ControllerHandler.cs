using System.Net;
using System.Web;
using http_server.Framework.Abstractions.Base;
using http_server.Framework.Abstractions.Interfaces;
using http_server.Framework.Factories;
using http_server.Framework.Routing;

namespace http_server.Framework.Handlers;

internal class ControllerHandler(IResourceController resourceController) : Handler
{
    public override async Task HandlerRequest(HttpListenerContext context)
    {
        try
        {
            var segments = context.Request.Url?.PathAndQuery
                .Trim('/')
                .Split('/');
            if (segments?.Length != 2)
            {
                await resourceController.SendErrorPage("404", context);
                return;
            }
            Console.WriteLine("1");
            var request = context.Request;

            var requestMethod = request.HttpMethod;
            var controllerRoute = segments?[0];
            var methodRoute = segments?[1];

            var routeKey = $"{requestMethod}|{controllerRoute}|{methodRoute}";

            if (!RouteRegistry.RouteDescriptors.TryGetValue(
                    routeKey, out var routeDescriptor))
            {
                await resourceController.SendErrorPage("404", context);
                return;
            }
            Console.WriteLine("2");
            var controller = ControllerFactory.Create(routeDescriptor.Controller, context);
            var method = routeDescriptor.Method;

            var resultCheckNull = await resourceController.CheckNull(controller, method, context);
            if (resultCheckNull is false) return;

            var methodParams = routeDescriptor.MethodsParams;


            object[] requestParameters = new object[methodParams.Length];

            switch (requestMethod.ToLower())
            {
                case "post" or "put" or "path":
                    {
                        using var stream = new StreamReader(request.InputStream, request.ContentEncoding);
                        var body = await stream.ReadToEndAsync();
                        var bodyParams = HttpUtility.ParseQueryString(body);

                        for (int i = 0; i < requestParameters.Length; i++)
                        {
                            requestParameters[i] = bodyParams[methodParams[i].Name] ?? string.Empty;
                        }
                    }
                    break;
            }

            var result = method.Invoke(controller, requestParameters);
            if (result is Task task)
            {
                await task;
            }
        }

        finally
        {
            context.Response.Close();
        }
    }
}