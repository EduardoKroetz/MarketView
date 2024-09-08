namespace src.Services;

using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using src.DTOs;
using StackExchange.Redis;

public class AssetsCachingService : BackgroundService
{
    private readonly HttpClient _httpClient;
    private readonly IDistributedCache _cache;
    private readonly string _apiKey;

    public AssetsCachingService(HttpClient httpClient, IDistributedCache cache, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _cache = cache;
        _apiKey = configuration["Brapi:ApiKey"] ?? throw new Exception("Invalid brapi api key");    
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await CacheAssetsAsync();
            // Run the task once a week
            await Task.Delay(TimeSpan.FromDays(7), stoppingToken);
        }
    }

    private async Task CacheAssetsAsync()
    {
        var response = await _httpClient.GetAsync($"https://brapi.dev/api/quote/list?token={_apiKey}");
        if (response.StatusCode == HttpStatusCode.OK)
        {
            var content = await response.Content.ReadAsStringAsync();
            var assets = JsonConvert.DeserializeObject<GetAssets>(content);
            var jsonString = JsonConvert.SerializeObject(assets);

            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(7)
            };

            await _cache.SetStringAsync("assets_data", jsonString, cacheOptions);
        }
    }
}