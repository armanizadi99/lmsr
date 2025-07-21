namespace Lmsr.Domain.Common;
public class DomainException : Exception
{
public DomainException(string message) : base(message){}
}

public class ValidationException : DomainException
{
public ValidationException (string message) : base(message){}
}

public class DomainRouleViolationException : DomainException
{
public DomainRouleViolationException(string message) : base(message){}
}

public class InvalidDomainOperationException : 	DomainException
{
public InvalidDomainOperationException(string message) : base(message){}
}