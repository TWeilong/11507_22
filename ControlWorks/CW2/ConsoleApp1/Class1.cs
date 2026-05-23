using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class MethodExecutor
    {
        public object Execute(object obj, string methodName)
        {
            Type type = obj.GetType();
            MethodInfo method = type.GetMethod(methodName);

            if (method == null)
            {
                return null;
            }

            ParameterInfo[] parameters =method.GetParameters();

            if (parameters.Length == 0)
            {
                return method.Invoke(obj, null);
            }

            if (parameters.Length == 1 && parameters[0].ParameterType == typeof(int))
            {
                PropertyInfo prop =type.GetProperty("Count");
                int count =(int)prop.GetValue(obj);
                return method.Invoke(obj,new object[] { count });
            }

            return null;
        }
    }
}
