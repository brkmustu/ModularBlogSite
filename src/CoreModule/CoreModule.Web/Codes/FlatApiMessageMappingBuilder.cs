namespace CoreModule.Web.Codes;

using SimpleInjector;
using System.Reflection;

public sealed class FlatApiMessageMappingBuilder : IMessageMappingBuilder
{
    private readonly string patternFormat;
    private readonly object dispatcher;
    private readonly MethodInfo genericMethod;

    public FlatApiMessageMappingBuilder(object dispatcher, string patternFormat = "/api/{0}")
    {
        this.patternFormat = patternFormat;
        this.dispatcher = dispatcher;
        this.genericMethod = dispatcher.GetType().GetMethod("InvokeAsync")
            ?? throw new ArgumentException("InvokeAsync method is missing.");
    }

    public (string, string[], Delegate) BuildMapping(Type messageType, Type? returnType)
    {
        if (messageType.IsAbstract)
        {
            return ("", new[] {""}, null);
        }

        (Type[] GenericArguments, Type GenericFuncType) args = returnType != null
            ? (new[] { messageType, returnType }, typeof(Func<,,>))
            : (new[] { messageType }, typeof(Func<,>));

        MethodInfo method = this.genericMethod.MakeGenericMethod(args.GenericArguments);

        Type funcType = args.GenericFuncType.MakeGenericType(
            method.GetParameters().Append(method.ReturnParameter).Select(p => p.ParameterType).ToArray());

        Delegate handler = Delegate.CreateDelegate(funcType, dispatcher, method);

        var pattern = string.Format(this.patternFormat, GetMessageRoute(messageType));

        return (pattern, new[] { HttpMethods.Post }, handler);
    }

    private static string GetMessageRoute(Type messageType) =>

        // ToFriendlyName builds an easy to read type name. Namespaces will be omitted, and generic types
        // will be displayed in a C#-like syntax.
        TypesExtensions.ToFriendlyName(messageType)

        // Replace generic markers. Typically they are allowed as root, but that would be frowned upon.
        .Replace("<", string.Empty).Replace(">", string.Empty);
}