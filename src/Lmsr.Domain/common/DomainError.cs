namespace Lmsr.Domain.Common;
public record  DomainError (string ErrorCode, string ErrorSubject, string ErrorMessage);