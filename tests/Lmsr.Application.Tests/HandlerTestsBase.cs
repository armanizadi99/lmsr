namespace Lmsr.Application.Tests;
public abstract class HandlerTestsBase
{
protected readonly Mock<IUnitOfWork> MockUnitOfWork = new();
protected readonly Mock<IUserContext> MockUserContext = new();
protected readonly string DefaultUserId = Guid.NewGuid().ToString();

protected HandlerTestsBase()
{
MockUserContext.Setup(m => m.UserId).Returns(DefaultUserId);
}
}