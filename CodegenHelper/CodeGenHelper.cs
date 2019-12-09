using System;
using System.Reflection;
using System.Text;

namespace CodegenHelper
{
    public class CodeGenHelper
    {
        public static string GenerateMethodDeclaration(MethodInfo mi)
        {
            var paramList = GenerateParametersList(mi, false);
            var sb = new StringBuilder();
            sb.Append(GetTypeName(mi.ReturnType));
            sb.Append(" ");
            sb.Append(mi.Name);
            sb.Append("(");
            sb.Append(paramList);
            sb.Append(")");
            return sb.ToString();
        }

        public static string GenerateParametersList(MethodInfo mi, bool namesOnly)
        {
            var parameters = mi.GetParameters();
            var parametersString = new StringBuilder();
            for (var j = 0; j < parameters.Length; j++)
            {
                if (j > 0) parametersString.Append(",");
                if (parameters[j].IsOut)
                    parametersString.Append("out ");
                else if (parameters[j].ParameterType.IsByRef)
                    parametersString.Append("ref ");

                if (!namesOnly)
                {
                    parametersString.Append(GetTypeName(parameters[j].ParameterType));
                    parametersString.Append(" ");
                }
                parametersString.Append(parameters[j].Name);
            }
            return parametersString.ToString();
        }

        public static string GenerateArgumentsList(MethodInfo mi)
        {
            var parameters = mi.GetParameters();
            var parametersString = new StringBuilder();
            for (var j = 0; j < parameters.Length; j++)
            {
                if (j > 0) parametersString.Append(",");
                if (parameters[j].IsOut)
                    parametersString.Append("out ");
                else if (parameters[j].ParameterType.IsByRef)
                    parametersString.Append("ref ");

                parametersString.Append(parameters[j].Name);
            }
            return parametersString.ToString();
        }

        public static string GetTypeName(Type type)
        {
            if (type == typeof(void))
                return "void";

            if (type.IsGenericType)
            {
                if (type.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    return type.GetGenericArguments()[0].FullName + "?";
                }
            }

            string name = type.FullName.Replace('+', '.');
            if (type.IsByRef) name = name.Substring(0, name.Length - 1);
            return name;
        }

    }
}
