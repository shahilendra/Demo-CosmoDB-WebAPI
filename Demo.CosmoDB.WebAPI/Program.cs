using Demo.CosmoDB.Comman;
using Demo.CosmoDB.Models;
using Demo.CosmoDB.Repository;
using Demo.CosmoDB.Repository.Abstraction;
using Demo.CosmoDB.Services;
using Demo.CosmoDB.Services.Abstraction;
using Demo.CosmoDB.WebAPI.Middlewares;
using Microsoft.Azure.Cosmos;

var builder = WebApplication.CreateBuilder(args);
var logger = LoggerFactory.Create(config =>
{
    config.AddConsole();
}).CreateLogger("CosmosDbClient");
// Add services to the container.
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("Values"));
var appSettings = builder.Configuration.GetSection("Values").Get<AppSettings>();
var dbClient = new CosmosClient(appSettings.EndpointUri, appSettings.PrimaryKey);
builder.Services.AddSingleton<CosmosDbClient<Employees>>(fictory=> new CosmosDbClient<Employees>(dbClient, appSettings.DatabaseName, logger));
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
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

app.UseAuthorization();

app.MapControllers();

app.Run();
