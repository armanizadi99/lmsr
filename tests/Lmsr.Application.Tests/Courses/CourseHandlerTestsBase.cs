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
var courses = new List<Course>();
var course1 = new Course("english basic words", "arman", false);
course1.Id = 1;
var course2 = new Course("english intermediate words", "arman", false);
course2.Id = 2;
var course3 = new Course("english curses", "baduser", true);
course3.Id = 3;
courses.AddRange(course1, course2, course3);
MockCourseRepo.Setup(m => m.GetAllCoursesAsync())
.ReturnsAsync(courses);

}
}