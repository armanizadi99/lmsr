using Lmsr.Infrastructure;
using Lmsr.Application.Behaviors;
using Lmsr.Application.Courses;
using Lmsr.Application;
using MediatR;
using Lmsr.Presentation;
using Serilog;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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
builder.Services.AddControllers();

builder.Services.AddAuthentication("Bearer")
.AddJwtBearer("Bearer", options =>
{
options.Authority = "https://localhost:5001"; // URL of your IdentityServer project
options.TokenValidationParameters = new TokenValidationParameters
{
ValidateAudience = false
};
});

builder.Services.AddAuthorization();
builder.Services.AddHttpContextAccessor();
builder.Services.AddCors(options =>
{
options.AddPolicy("AllowReactApp", builder =>
builder.WithOrigins("http://localhost:3000", "https://localhost:3000") // your SPA origins
.AllowAnyHeader()
.AllowAnyMethod());
});

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("AllowReactApp"); // Place before UseAuthorization
app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ValidationExceptionMiddleware>();
app.MapControllers();

app.Run();
