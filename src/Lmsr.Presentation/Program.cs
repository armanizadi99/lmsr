using Lmsr.Infrastructure;
using Lmsr.Application.Behaviors;
using Lmsr.Application.Courses;
using Lmsr.Application;
using MediatR;
using Lmsr.Presentation;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
Log.Logger = new LoggerConfiguration()
.ReadFrom.Configuration(configuration)
.CreateLogger();

builder.Host.UseSerilog();
builder.Services.AddOpenApi();
builder.Services.AddMediatR(cfg =>
cfg.RegisterServicesFromAssembly(typeof(CreateCourseCommand).Assembly));
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

builder.Services.AddInfrastructure(builder.Configuration, builder.Environment);
builder.Services.AddApplication();
var app = builder.Build();


app.UseMiddleware<ValidationExceptionMiddleware>();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();



app.Run();
