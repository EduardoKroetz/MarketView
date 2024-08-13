using src.Services;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<RedisService>();
builder.Services.AddHostedService<AssetsCachingService>();
builder.Services.AddScoped<AssetsService>();

var configurationOptions = new ConfigurationOptions
{
    EndPoints = { "redis:6379" },
    AbortOnConnectFail = false,
    ConnectRetry = 5,
    ConnectTimeout = 5000 // 5 segundos
};

var redisConnectionString = builder.Configuration.GetConnectionString("Redis") ?? throw new Exception("Invalid redis connection string");
var connectionMultiplexer = ConnectionMultiplexer.Connect(configurationOptions);
//var server = connectionMultiplexer.GetServer("localhost", 6379);
//var keys = server.Keys();

//foreach (var key in keys)
//{
//    await connectionMultiplexer.GetDatabase().KeyDeleteAsync(key);
//}

builder.Services.AddSingleton<IConnectionMultiplexer>(connectionMultiplexer);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(configurePolicy =>
{
    configurePolicy.AllowAnyOrigin();
    configurePolicy.AllowAnyMethod();
    configurePolicy.AllowAnyHeader();
});
app.UseHttpsRedirection();
app.MapControllers();
app.Run();
 