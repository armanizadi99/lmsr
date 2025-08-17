namespace Lmsr.Domain.Common;
public class Result<T>
{
public bool IsSuccess { get; private set; }
public T Value { get; private set; }
public DomainError Error { get; private set;}

public static Result<T> Success(T value) => new Result<T> { IsSuccess = true, Value = value };
public static Result<T> Failure(DomainError error) => new Result<T> { IsSuccess = false, Error = error };
}

public class Result
{
public bool IsSuccess { get;  private set; }
public DomainError Error { get;  private set; }

public static Result Success() => new Result { IsSuccess = true};
public static Result Failure(DomainError error) => new Result { IsSuccess = false, Error = error };
}