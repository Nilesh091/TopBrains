using NotificationService.Application.Services;
using NotificationService.Domain.Entity;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 🔥 Application Layer
builder.Services.AddScoped<EmailService>();

// 🔥 Infrastructure Layer
builder.Services.AddScoped<IEmailSender, SmtpEmailSender>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();