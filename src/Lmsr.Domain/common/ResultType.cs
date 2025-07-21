namespace Lmsr.Domain.Common;
public class Result<T>
{
public bool IsSuccess { get; private set; }
public T Value { get; private set; }
public List<string> Errors { get; private set;}

public static Result<T> Success(T value) => new Result<T> { IsSuccess = true, Value = value };
public static Result<T> Failure(List<string> errors) => new Result<T> { IsSuccess = false, Errors = errors };
}

public class Result
{
public bool IsSuccess { get;  private set; }
public List<string> Errors { get;  private set; }

public static Result Success() => new Result { IsSuccess = true};
public static Result Failure(List<string> errors) => new Result { IsSuccess = false, Errors = errors };
}