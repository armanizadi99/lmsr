namespace Lmsr.Domain.Common;
public class DomainException : Exception
{
public DomainException(string message) : base(message){}
}

public class DomainValidationException : DomainException
{
public DomainValidationException (string message) : base(message){}
}

public class DomainRouleViolationException : DomainException
{
public DomainRouleViolationException(string message) : base(message){}
}

public class InvalidDomainOperationException : 	DomainException
{
public InvalidDomainOperationException(string message) : base(message){}
}