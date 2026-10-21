using System.Reflection;

namespace http_server.Framework.Routing;

public class RouteDescriptor(Type controller, MethodInfo method)
{
    public Type Controller { get; } = controller;

    public MethodInfo Method { get; } = method;

    public ParameterInfo[] MethodsParams { get; } = method.GetParameters();
}