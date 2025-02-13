using System.Text.Json.Serialization;
using Serilog;
using Datum.Blog.Application.Extensions;
using Datum.Blog.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog();

// Add Logging
builder.Services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));

// Add Controllers
builder.Services.AddControllers().AddJsonOptions(options => { options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull; });

// Add Routing
builder.Services.AddRouting(options => options.LowercaseUrls = true);

// Add Controllers
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services
    .AddInfrastructure(builder.Configuration)
    .AddApplication()
    .AddApplicationServices();

// Build App
var app = builder.Build();

// Configure the HTTP request pipeline
//app.UseHttpsRedirection();
app.UseSerilogRequestLogging();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Datum.Blog.Api");
    c.RoutePrefix = string.Empty; // Set Swagger UI at the app's root
});
app.UseAuthorization();
app.MapControllers();
app.UseRouting();

app.Run();