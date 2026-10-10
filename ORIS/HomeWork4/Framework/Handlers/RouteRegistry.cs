using System.Reflection;
using System.Text;
using http_server.Framework.Attributes;
using http_server.Framework.Configuration;

namespace http_server.Framework.Routing;

public static class RouteRegistry
{
    public static readonly Dictionary<string, RouteDescriptor> RouteDescriptors = new();

    public static void Register()
    {
        var assembly = Assembly.GetExecutingAssembly();

        StringBuilder stringBuilder = new();

        foreach (var controller in assembly.GetTypes()
                     .Where(t => Attribute.IsDefined(t, typeof(HttpControllerAttribute))))
        {

            var atrController = controller.GetCustomAttributes()
                                 .FirstOrDefault(a => a is HttpControllerAttribute);

            var atrControllerValue = atrController?
                                 .GetType()
                                 .GetProperty("Route")?
                                 .GetValue(atrController);

            var methods = controller
                .GetMethods()
                .Where(m => m.GetCustomAttributes().Any(a => a is HttpMethodAttribute)).ToArray();


            // Сборка ключа 
            foreach (var method in methods)
            {
                var atrMethod = method.GetCustomAttributes(true)
                    .FirstOrDefault(a => a is HttpMethodAttribute);

                var atrMethodValue = atrMethod?
                    .GetType()
                    .GetProperty("Route")?
                    .GetValue(atrMethod);


                var atrMethodName = atrMethod?.GetType().Name.Replace("Attribute", "").ToUpper();

                stringBuilder.Append(atrMethodName);
                stringBuilder.Append("|");

                stringBuilder.Append(atrControllerValue);
                stringBuilder.Append("|");

                stringBuilder.Append(atrMethodValue);

                var resultRoute = stringBuilder.ToString();

                RouteDescriptors[resultRoute] = new(controller, method);

                stringBuilder.Clear();
            }
        }
    }
}