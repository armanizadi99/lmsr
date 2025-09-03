namespace Lmsr.Application.Tests;
public abstract class CourseHandlerTestsBase : HandlerTestsBase
{
protected readonly Mock<ICourseTitleUniquenessSpecification> MockTitleUniquenessSpec = new();
protected readonly Mock<ICourseRepository> MockCourseRepo = new();
protected readonly string UniqueTitle = "UniqueTitle";
protected readonly string DuplicateTitle = "DuplicateTitle";
protected Course CapturedCourse = null;

protected CourseHandlerTestsBase()
{
MockUnitOfWork.Setup(m => m.CourseRepo).Returns(MockCourseRepo.Object);
SetupMockTitleUniquenessSpec();
SetupMockCourseRepo();
}

protected void SetupMockTitleUniquenessSpec()
{
MockTitleUniquenessSpec.Setup(m => m.IsTitleUnique(UniqueTitle)).Returns(true);
MockTitleUniquenessSpec.Setup(m => m.IsTitleUnique(DuplicateTitle)).Returns(false);
}

protected void SetupMockCourseRepo()
{
MockCourseRepo.Setup(m => m.AddAsync(It.IsAny<Course>()))
.Callback<Course>(c =>{
c.Id = 1;
CapturedCourse = c;
})
.Returns(Task.CompletedTask);
MockCourseRepo.Setup(m => m.Delete(It.IsAny<Course>()))
.Callback<Course>(c => CapturedCourse = c);

MockCourseRepo.Setup(m => m.GetByIdAsync(1))
.ReturnsAsync(new Course("course1", DefaultUserId, false));
MockCourseRepo.Setup(m => m.GetByIdAsync(2))
.ReturnsAsync(new Course("course2", Guid.NewGuid().ToString(), false));
}
}