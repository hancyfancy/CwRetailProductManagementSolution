using Microsoft.AspNetCore.Cors;

var permittedSpecificOrigins = "_permittedSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: permittedSpecificOrigins,
        policy =>
        {
            policy.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod()
            .WithExposedHeaders("Access-Control-Allow-Origin", "Access-Control-Allow-Methods");
        });
});

var app = builder.Build();

app.UseAuthorization();

app.MapControllers();

app.UseCors(permittedSpecificOrigins);

app.Run();
