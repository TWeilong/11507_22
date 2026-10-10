using System.Net;

namespace http_server.Framework.Factories;

public class ControllerFactory
{
    public static object Create(
        Type controllerType,
        HttpListenerContext context)
    {
        var constructor = controllerType
            .GetConstructors()
            .Single();

        var constructorParams = constructor.GetParameters();

        var arguments = new object[constructorParams.Length];

        for (int i = 0; i < constructorParams.Length; i++)
        {
            var parameterType = constructorParams[i].ParameterType;

            if (parameterType == typeof(HttpListenerResponse))
            {
                arguments[i] = context.Response;
            }
            else if (parameterType == typeof(HttpListenerRequest))
            {
                arguments[i] = context.Request;
            }
            else if (parameterType == typeof(HttpListenerContext))
            {
                arguments[i] = context;
            }
            else
            {
                throw new InvalidOperationException(
                    $"Неизвестная зависимость: {parameterType.Name}");
            }
        }

        return Activator.CreateInstance(
            controllerType,
            arguments
        )!;
    }
}