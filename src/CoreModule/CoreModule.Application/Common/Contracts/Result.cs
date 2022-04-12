namespace CoreModule.Application.Common.Contracts;

public class Result
{
    internal Result(bool succeeded)
    {
        Succeeded = succeeded;
    }
    internal Result(bool succeeded, IEnumerable<string> errors)
    {
        Succeeded = succeeded;
        Errors = errors.ToArray();
    }
    internal Result(bool succeeded, string message)
    {
        Succeeded = succeeded;
        Message = message;
    }

    public bool Succeeded { get; set; }
    public string Message { get; set; }
    public string[] Errors { get; set; }

    public static Result Success()
    {
        return new Result(true);
    }
    public static Result<TData> Success<TData>(TData data)
    {
        return new Result<TData>(true, data);
    }
    public static Result Success(string message)
    {
        return new Result(true, message);
    }
    public static Result Failure(IEnumerable<string> errors)
    {
        return new Result(false, errors);
    }
    public static Result<TData> Failure<TData>(IEnumerable<string> errors)
    {
        return new Result<TData>(false, errors);
    }
}
