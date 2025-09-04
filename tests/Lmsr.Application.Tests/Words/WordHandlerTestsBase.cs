namespace Lmsr.Application.Tests;
public abstract class WordHandlerTestsBase : HandlerTestsBase
{
protected readonly Mock<IWordRepository> MockWordRepo = new();
protected readonly Mock<ICourseRepository> MockCourseRepo = new();
protected readonly Mock<IWordTermUniquenessSpecification> MockWordTermUniquenessSpec;
protected Word CapturedWord = null;
protected readonly string UniqueWord = "UniqueWord";
protected readonly string DuplicateWord = "DuplicateWord";
protected int CourseId = 1;

protected WordHandlerTestsBase()
{
MockUnitOfWork.Setup(m => m.WordRepo)
.Returns(MockWordRepo.Object);
MockUnitOfWork.Setup(m => m.CourseRepo)
.Returns(MockCourseRepo.Object);
SetupMockWordTermUniquenessSpec();
SetupMockWordRepo();
SetupMockCourseRepo();
}
protected void SetupMockCourseRepo()
{
MockCourseRepo.Setup(m => m.GetByIdAsync(1))
.ReturnsAsync(new Course("course1", DefaultUserId, false));
}
protected void SetupMockWordRepo()
{
MockWordRepo.Setup(m => m.AddAsync(It.IsAny<Word>()))
.Callback<Word>(w => {
w.Id = 1;
CapturedWord = w;
})
.Returns(Task.CompletedTask);
}

protected void SetupMockWordTermUniquenessSpec()
{
MockWordTermUniquenessSpec.Setup(m => m.IsWordTermUnique(UniqueWord, CourseId))
.Returns(true);
MockWordTermUniquenessSpec.Setup(m => m.IsWordTermUnique(DuplicateWord, CourseId))
.Returns(false);
}
}