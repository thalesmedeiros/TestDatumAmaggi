using Datum.Blog.Application.Extensions;
using Datum.Blog.Infrastructure.Configuration;
using Datum.Blog.Infrastructure.Extensions;
using Datum.Blog.Infrastructure.Notification;
using Serilog;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Configura��o do Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog();

// Configura��o do Logging
builder.Services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));

// Configura��o do JSON
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});

// Configura��o do CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
           .WithMethods("POST", "GET")
           .AllowAnyHeader()
           .AllowCredentials();
 
    });

    options.AddPolicy("AllowAngularApp", policy =>
    {
        policy.AllowAnyOrigin()
           .WithMethods("GET", "POST")
           .AllowAnyHeader();
    });
});

// Configura��o de Roteamento
builder.Services.AddRouting(options => options.LowercaseUrls = true);

// Configura��o do SignalR
builder.Services.AddSignalR(options =>
{
    options.EnableDetailedErrors = true;
}).AddJsonProtocol();

// Configura��o do Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerConfiguration();

// Configura��o de Autentica��o JWT
builder.Services.AddJwtAuthentication(builder.Configuration);

// Registro dos servi�os da infraestrutura e aplica��o
builder.Services
    .AddInfrastructure(builder.Configuration)
    .AddApplication()
    .AddApplicationServices();

var app = builder.Build();

// Configura��o do pipeline de requisi��o
app.UseSerilogRequestLogging();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Datum.Blog.Api");
    c.RoutePrefix = string.Empty;  // Definir o Swagger UI no root da aplica��o
});

// Uso da autentica��o e autoriza��o
app.UseAuthentication();
app.UseAuthorization();

// Configura��o do CORS
app.UseCors("AllowAngularApp");

app.MapControllers();

// Mapear o Hub SignalR
app.MapHub<NotificationHub>("/notificationHub");


app.UseWebSockets();

// Iniciar a aplica��o
app.Run();
