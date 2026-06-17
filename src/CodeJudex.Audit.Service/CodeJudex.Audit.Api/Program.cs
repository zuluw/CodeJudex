using CodeJudex.Audit.Api.Middleware;
using CodeJudex.Audit.Application.DependencyInjection;
using CodeJudex.Audit.Infrastructure.DependencyInjection;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();  
    app.UseSwaggerUI(); 
}

app.UseSerilogRequestLogging();

app.MapControllers();

app.Run();
