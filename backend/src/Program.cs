using Microsoft.AspNetCore.ResponseCompression;
using src.Exceptions;
using src.Services;
using StackExchange.Redis;
using System.IO.Compression;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddSingleton<RedisService>();
builder.Services.AddHostedService<AssetsCachingService>();
builder.Services.AddScoped<AssetsService>();
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true; 
    options.Providers.Add<GzipCompressionProvider>();
});

builder.Services.Configure<GzipCompressionProviderOptions>(options =>
{
    options.Level = CompressionLevel.Fastest;
});

var configurationOptions = new ConfigurationOptions
{
    EndPoints = { "redis:6379" },
    AbortOnConnectFail = false,
    ConnectRetry = 5,
    ConnectTimeout = 5000 // 5 segundos
};

var connectionMultiplexer = ConnectionMultiplexer.Connect(configurationOptions);

//Reset all cache
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
app.UseResponseCompression();
app.UseExceptionHandler();
app.MapControllers();
app.Run();
 