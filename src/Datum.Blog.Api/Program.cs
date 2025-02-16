using Datum.Blog.Application.Extensions;
using Datum.Blog.Infrastructure.Configuration;
using Datum.Blog.Infrastructure.Extensions;
using Datum.Blog.Infrastructure.Notification;
using Serilog;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Configuração do Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog();

// Configuração do Logging
builder.Services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));

// Configuração do JSON
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});

// Configuração do CORS
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

// Configuração de Roteamento
builder.Services.AddRouting(options => options.LowercaseUrls = true);

// Configuração do SignalR
builder.Services.AddSignalR(options =>
{
    options.EnableDetailedErrors = true;
}).AddJsonProtocol();

// Configuração do Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerConfiguration();

// Configuração de Autenticação JWT
builder.Services.AddJwtAuthentication(builder.Configuration);

// Registro dos serviços da infraestrutura e aplicação
builder.Services
    .AddInfrastructure(builder.Configuration)
    .AddApplication()
    .AddApplicationServices();

var app = builder.Build();

// Configuração do pipeline de requisição
app.UseSerilogRequestLogging();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Datum.Blog.Api");
    c.RoutePrefix = string.Empty;  // Definir o Swagger UI no root da aplicação
});

// Uso da autenticação e autorização
app.UseAuthentication();
app.UseAuthorization();

// Configuração do CORS
app.UseCors("AllowAngularApp");

app.MapControllers();

// Mapear o Hub SignalR
app.MapHub<NotificationHub>("/notificationHub");


app.UseWebSockets();

// Iniciar a aplicação
app.Run();
