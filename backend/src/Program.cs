using src.Services;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<RedisService>();
builder.Services.AddHostedService<AssetsCachingService>();

var redisConnectionString = builder.Configuration.GetConnectionString("Redis") ?? throw new Exception("Invalid redis connection string");
var connectionMultiplexer = ConnectionMultiplexer.Connect(redisConnectionString);

builder.Services.AddSingleton<IConnectionMultiplexer>(connectionMultiplexer);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
 