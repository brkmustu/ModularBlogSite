namespace CoreModule.Application.CrossCuttingConcerns;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
public class ValidationDecoratorAttribute : Attribute
{
}
