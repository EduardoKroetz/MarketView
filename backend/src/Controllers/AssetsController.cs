using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using src.DTOs;
using src.Services;

namespace src.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AssetsController : Controller
{
    private readonly AssetsService _assetsService;
    private readonly IDistributedCache _cache;

    public AssetsController(AssetsService assetsService, IDistributedCache cache)
    {
        _assetsService = assetsService;
        _cache = cache;
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchAssetAsync([FromQuery] string symbol, [FromQuery] int page = 1, [FromQuery] int pageSize = 50)
    {
        if (string.IsNullOrEmpty(symbol))
        {
            return BadRequest(Result.BadResult("Informe o símbolo do ativo"));
        }

        var cacheValue = await _cache.GetStringAsync($"{symbol}_data");
        if (cacheValue != null)
        {
            var cachedAssets = JsonConvert.DeserializeObject<List<SearchStockInfo>>(cacheValue);
            return Ok(Result.SucessResult(cachedAssets.Skip(( page - 1 ) * pageSize).Take(pageSize).ToList(), "Success!"));
        }

        var assetsCacheValueString = await _cache.GetStringAsync("assets_data") ?? throw new Exception("Não foi possível buscar os ativos do cache, tente novamente mais tarde");
        var assetsCacheValue = JsonConvert.DeserializeObject<GetAssets>(assetsCacheValueString);

        var assets = assetsCacheValue.Stocks
            .Where(x =>
                x.Stock.Contains(symbol, StringComparison.OrdinalIgnoreCase) ||
                x.Name.Contains(symbol, StringComparison.OrdinalIgnoreCase))
            .Skip(( page - 1 ) * pageSize)
            .Take(pageSize)
            .ToList();

        var jsonString = JsonConvert.SerializeObject(assets);
        var cacheOptions = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
        };
        await _cache.SetStringAsync($"{symbol}_data", jsonString, cacheOptions);

        return Ok(Result.SucessResult(assets, "Success!"));
    }

    [HttpGet("{asset}")]
    public async Task<IActionResult> GetAssetDataAsync([FromRoute] string asset)
    {
        if (string.IsNullOrEmpty(asset))
        {
            return BadRequest(Result.BadResult("Invalid asset"));
        }

        var cacheValue = await _cache.GetStringAsync($"{asset}_asset_data");
        if (cacheValue != null)
        {
            var data = JsonConvert.DeserializeObject<MarketData>(cacheValue);
            return Ok(Result.SucessResult(data, "Success!"));
        }

        var symbol = asset.ToUpper();
        var assetData = await _assetsService.GetAssetsDataFromBrapi(symbol);

        var jsonString = JsonConvert.SerializeObject(assetData);
        await _cache.SetStringAsync($"{asset}_asset_data", jsonString, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
        });

        return Ok(Result.SucessResult(assetData, "Success!"));
    }

    [HttpGet("news/{asset}")]
    public async Task<IActionResult> GetAssetNewsAsync([FromRoute] string asset)
    {
        if (string.IsNullOrEmpty(asset))
        {
            return BadRequest(Result.BadResult("Invalid asset"));
        }

        var cacheValue = await _cache.GetStringAsync($"{asset}_asset_news");
        if (cacheValue != null)
        {
            var news = JsonConvert.DeserializeObject<List<NewArticle>>(cacheValue);
            return Ok(Result.SucessResult(news.OrderByDescending(x => x.PublishedAt).ToList(), "Success!"));
        }

        var symbol = asset.ToUpper();
        var assetNews = await _assetsService.GetAssetsNewsFromNewsApi(symbol);

        var jsonString = JsonConvert.SerializeObject(assetNews);
        await _cache.SetStringAsync($"{asset}_asset_news", jsonString, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24)
        });

        return Ok(Result.SucessResult(assetNews.OrderByDescending(x => x.PublishedAt).ToList(), "Success!"));
    }

    [HttpGet("last-news")]
    public async Task<IActionResult> GetLastNewsAsync()
    {
        var cacheValue = await _cache.GetStringAsync("last_news");
        if (cacheValue != null)
        {
            var lastNewsData = JsonConvert.DeserializeObject<List<NewArticle>>(cacheValue);
            return Ok(Result.SucessResult(lastNewsData, "Success!"));
        }

        var lastNews = await _assetsService.GetLastTenNewsArticles();

        var jsonString = JsonConvert.SerializeObject(lastNews);
        await _cache.SetStringAsync("last_news", jsonString, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(3)
        });

        return Ok(Result.SucessResult(lastNews, "Success!"));
    }

    [HttpGet("most-traded")]
    public async Task<IActionResult> GetTopTenMostTraded()
    {
        var cacheValue = await _cache.GetStringAsync("most_traded_assets");
        if (cacheValue != null)
        {
            var mostTradedData = JsonConvert.DeserializeObject<List<SearchStockInfo>>(cacheValue);
            return Ok(Result.SucessResult(mostTradedData, "Success!"));
        }

        var mostTraded = await _assetsService.GetTopTenMostTradedAssets();

        var jsonString = JsonConvert.SerializeObject(mostTraded);
        await _cache.SetStringAsync("most_traded_assets", jsonString, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(48)
        });

        return Ok(Result.SucessResult(mostTraded, "Success!"));
    }
}
