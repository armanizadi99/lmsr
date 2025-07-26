namespace Lmsr.Domain.Common;
public abstract class BaseEntity<T>
{
public virtual T Id {get; internal set; }
}