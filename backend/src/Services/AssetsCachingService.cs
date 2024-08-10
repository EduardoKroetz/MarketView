namespace src.Services;

using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using src.DTOs;

public class AssetsCachingService : BackgroundService
{
    private readonly HttpClient _httpClient;
    private readonly RedisService _redisService;
    private readonly string _apiKey;

    public AssetsCachingService(RedisService redisService, IConfiguration configuration)
    {
        _httpClient = new HttpClient();
        _redisService = redisService;
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
            await _redisService.SetCacheValueAsync("assets_data", jsonString);
        }
    }
}