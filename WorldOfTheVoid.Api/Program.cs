using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using WorldOfTheVoid.Domain.Ports;
using WorldOfTheVoid.Extensions;
using WorldOfTheVoid.Features;
using WorldOfTheVoid.Infrastructure.DbContext;
using WorldOfTheVoid.Infrastructure.Repositories;
using WorldOfTheVoid.Interfaces;
using WorldOfTheVoid.Utilities;

var builder = WebApplication.CreateBuilder(args);

// Add MVC controllers
builder.Services.AddControllers();

// Add Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "World of the Void API",
        Version = "v1"
    });
});
builder.Services.Configure<JsonOptions>(options =>
{
    options.JsonSerializerOptions.Converters.Add(new Vector3JsonConverter());
    options.JsonSerializerOptions.Converters.Add(new EntityIdJsonConverter());
});

builder.Services.AddDbContext<GameDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("GameDatabase")));

builder.Services.AddScoped<IWorldRepository, WorldRepository>();

builder.Services.AddHandlersFromAssemblyContaining<Program>();

builder.Services.AddScoped<IPeriodicTask, GrowPopulationPeriodicTask>();

builder.Services.AddHostedService<PeriodicWorker>();


var app = builder.Build();

// Auto-migrate DB
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<GameDbContext>();
    db.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Map controllers
app.MapControllers();

app.Run();