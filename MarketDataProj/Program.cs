using MarketData.Common.Configs;
using MarketData.DAL;
using MarketData.Data;
using MarketData.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.Configuration;
using static System.Collections.Specialized.BitVector32;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var logger = new LoggerConfiguration()
  .ReadFrom.Configuration(builder.Configuration)
  .Enrich.FromLogContext()
  .CreateLogger();


builder.Services.AddOptions();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);
//var section = (IConfiguration)Configuration.GetSection("DatabaseSettings");

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<DbSettingsConfig>(builder.Configuration.GetSection("DatabaseSettings"));
builder.Services.AddTransient(typeof(IDbContext), typeof(MongoDbContext));
builder.Services.AddTransient(typeof(IDbMarketRepository), typeof(DbMarketRepository));
builder.Services.AddSingleton(typeof(IMarketDataService), typeof(MarketDataService));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

