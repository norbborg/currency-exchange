namespace Currency.Exchange.Models;

public class Result<T>
{
    public static Result<T> ErrorResult(string[] errors)
    {
        return new Result<T>
        {
            IsError = true,
            Errors = errors
        };
    }

    public static Result<T> SuccessResult(T data)
    {
        return new Result<T>
        {
            Data = data
        };
    }

    public bool IsError { get; private set; }

    public string[] Errors { get; private set; }

    public T Data { get; private set; }
}