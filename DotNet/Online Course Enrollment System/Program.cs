using Microsoft.EntityFrameworkCore;
using Online_Course_Enrollment_System.Model.Context;
using Online_Course_Enrollment_System.Services;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<Connection>(options =>
    options.UseSqlServer("Server=localhost,1433;Database=Online_Course_Enrollment_System;User Id=sa;Password=2004@Nilu;TrustServerCertificate=True;"));

//builder.Services.AddControllers();
builder.Services.AddScoped<ICourseManagementService, CourseManagement>();
builder.Services.AddScoped<IStudentManagementService, Student_Management>();
var app = builder.Build();


// app.MapGet("/", () => "Hello World!");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.Run();
