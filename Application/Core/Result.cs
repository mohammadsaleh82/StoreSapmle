namespace Application.Core;

public class Result<T>
{
    public bool IsSuccess { get; set; }

    public T Value { get; set; }

    public string errors { get; set; }

    public static Result<T> Success(T value)=>new Result<T>{IsSuccess=true,Value=value};
    public static Result<T> Failure(string errors)=>new Result<T>{IsSuccess=false,errors=errors};
}
