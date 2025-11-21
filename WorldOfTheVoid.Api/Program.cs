using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using WorldOfTheVoid.Auth;
using WorldOfTheVoid.Auth.Extensions;
using WorldOfTheVoid.Domain.Entities;
using WorldOfTheVoid.Domain.PerioticTasks;
using WorldOfTheVoid.Domain.Ports;
using WorldOfTheVoid.Domain.Services;
using WorldOfTheVoid.Extensions;
using WorldOfTheVoid.Features;
using WorldOfTheVoid.Infrastructure.DbContext;
using WorldOfTheVoid.Infrastructure.Repositories;
using WorldOfTheVoid.Interfaces;
using WorldOfTheVoid.Services;
using WorldOfTheVoid.Utilities;

var builder = WebApplication.CreateBuilder(args);

// Add MVC controllers
builder.Services.AddControllers(options =>
{
    options.ModelBinderProviders.Insert(0, new EntityIdModelBinderProvider());
});
builder.Services.AddJwtAuthentication(builder.Configuration);

builder.Services.AddScoped<IUserContextService, UserContextService>();

// Add Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.AddSwagger();
builder.Services.Configure<JsonOptions>(options =>
{
    options.JsonSerializerOptions.Converters.Add(new Vector3JsonConverter());
    options.JsonSerializerOptions.Converters.Add(new EntityIdJsonConverter());
});

builder.Services.AddDbContext<GameDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("GameDatabase")));

builder.Services.AddScoped<IPasswordHasher<Account>, PasswordHasher<Account>>();
builder.Services.AddHttpContextAccessor();

// Domain Services
builder.Services.AddScoped<IAccountService, AccountService>();

// Infrastructure services
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IWorldRepository, WorldRepository>();

// Application
builder.Services.AddHandlersFromAssemblyContaining<Program>();

builder.Services.AddScoped<IPeriodicTask, GrowPopulationPeriodicTask>();
builder.Services.AddScoped<IPeriodicTask, MoveCharactersTask>();
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
