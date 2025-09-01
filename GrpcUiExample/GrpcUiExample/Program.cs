using GrpcDemo.Services;
using GrpcUiExample.Services;
using Microsoft.AspNetCore.Server.Kestrel.Core;

var builder = WebApplication.CreateBuilder(args);

// Add CORS policy dynamically based on configuration
builder.Services.AddCors(options =>
{
    var allowedOrigins = builder.Configuration
        .GetSection("Origins")
        .GetChildren()
        .Select(x => x.Value)
        .ToArray();

    options.AddPolicy("DynamicCORS", policy =>
    {
        policy.WithOrigins(allowedOrigins)
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials(); // Optional: For cookies/authentication if needed
    });
});

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();

// Configure Kestrel for gRPC
builder.WebHost.ConfigureKestrel(options =>
{
    //options.ListenLocalhost(6001, o => o.Protocols = HttpProtocols.Http2);
    options.ListenLocalhost(5001, o => o.UseHttps());
});

var app = builder.Build();

// Enable CORS middleware
app.UseCors("DynamicCORS");

// Enable gRPC-Web middleware
app.UseGrpcWeb();

// Handle preflight (OPTIONS) requests for gRPC-Web
app.Use(async (context, next) =>
{
    if (context.Request.Method == "OPTIONS")
    {
        context.Response.Headers.Add("Access-Control-Allow-Origin", "*"); // Allow all for testing
        context.Response.Headers.Add("Access-Control-Allow-Methods", "POST, GET, OPTIONS");
        context.Response.Headers.Add("Access-Control-Allow-Headers", "*");
        context.Response.StatusCode = 204; // No Content
    }
    else
    {
        await next();
    }
});

// Swagger and gRPC Reflection for development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapGrpcReflectionService();
}

// Map gRPC services with gRPC-Web enabled
app.MapGrpcService<UserServiceImpl>()
   .EnableGrpcWeb();

app.MapGrpcService<GreeterService>()
   .EnableGrpcWeb();

// Default endpoint
app.MapGet("/", () => "gRPC Server is running");

app.Run();
