using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Options;
using src.Exceptions;
using src.Services;
using StackExchange.Redis;
using System.IO.Compression;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient();
builder.Services.AddStackExchangeRedisCache(options =>
{
    if (builder.Environment.IsProduction())
    {
        options.ConfigurationOptions = new ConfigurationOptions
        {
            AbortOnConnectFail = false,
            ConnectTimeout = 5000,
            EndPoints = { "redis:6379" }
        };
    }
    else
    {
        options.ConfigurationOptions = new ConfigurationOptions
        {
            AbortOnConnectFail = false,
            ConnectTimeout = 5000,
            EndPoints = { "localhost:6379" }
        };
    }
});

//Clean stored cache 
if (builder.Environment.IsProduction())
{
    var connectionMultiplexer = ConnectionMultiplexer.Connect("redis:6379");
    var server = connectionMultiplexer.GetServer("redis:6379");
    var keys = server.Keys();
    foreach (var key in keys)
    {
        connectionMultiplexer.GetDatabase().KeyDelete(key);
    }
    Console.WriteLine($"Clear cache -- Keys count: {server.Keys()}");
}
else
{
    var connectionMultiplexer = ConnectionMultiplexer.Connect("localhost:6379");
    var server = connectionMultiplexer.GetServer("localhost:6379");
    var keys = server.Keys();
    foreach (var key in keys)
    {
        connectionMultiplexer.GetDatabase().KeyDelete(key);
    }
    Console.WriteLine($"Clear cache -- Keys count: {server.Keys()}");

}

builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(builder.Environment.IsProduction() ? "redis:6379" : "localhost:6379"));

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
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
 