namespace CoreModule.Application.Common.Contracts;

public class Result<TData>
{
    public bool Succeeded { get; set; }
    public string Message { get; set; }
    public string[] Errors { get; set; }
    public TData Data { get; set; }

    internal Result(bool succeeded, TData data)
    {
        Succeeded = succeeded;
        Data = data;
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
}
