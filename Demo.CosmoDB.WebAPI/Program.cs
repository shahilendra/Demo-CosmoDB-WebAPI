using Demo.CosmoDB.Comman;
using Demo.CosmoDB.Models;
using Demo.CosmoDB.Repository;
using Demo.CosmoDB.Repository.Abstraction;
using Demo.CosmoDB.Services;
using Demo.CosmoDB.Services.Abstraction;
using Demo.CosmoDB.WebAPI.Middlewares;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Caching.Distributed;

var builder = WebApplication.CreateBuilder(args);
var logger = LoggerFactory.Create(config =>
{
    config.AddConsole();
}).CreateLogger("CosmosDbClient");
// Add services to the container.
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("Values"));
var appSettings = builder.Configuration.GetSection("Values").Get<AppSettings>();
var dbClient = new CosmosClient(appSettings.EndpointUri, appSettings.PrimaryKey);
builder.Services.AddSingleton<CosmosDbClient<Employees>>(factory => new CosmosDbClient<Employees>(dbClient, appSettings.DatabaseName, logger));
builder.Services.AddSingleton<CosmosDbClient<Demo.CosmoDB.Models.User>>(factory => new CosmosDbClient<Demo.CosmoDB.Models.User>(dbClient, appSettings.DatabaseName, logger));
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddSingleton<ICacheService<Employees>>(factory => new CacheService<Employees>(TimeSpan.FromMinutes(appSettings.CacheExpiration), factory.GetService<IDistributedCache>()));
builder.Services.AddSingleton<ICacheService<Demo.CosmoDB.Models.User>>(factory => new CacheService<Demo.CosmoDB.Models.User>(TimeSpan.FromMinutes(appSettings.CacheExpiration), factory.GetService<IDistributedCache>()));

builder.Services.AddDistributedRedisCache(options =>
{
    options.Configuration = appSettings.RedisConnection;
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.ConfigureExceptionHandler(app.Logger);
app.UseHttpsRedirection();
app.UseMiddleware<JwtMiddleware>();
app.UseAuthorization();

app.MapControllers();

app.Run();
