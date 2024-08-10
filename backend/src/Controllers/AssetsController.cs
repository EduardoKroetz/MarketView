using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using src.DTOs;
using src.Services;

namespace src.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AssetsController : Controller
{
    private readonly RedisService _redisService;
    private readonly AssetsService _assetsService;

    public AssetsController(RedisService redisService, AssetsService assetsService)
    {
        _redisService = redisService;
        _assetsService = assetsService;
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchAssetAsync([FromQuery] string symbol, [FromQuery] int page = 1, [FromQuery] int pageSize = 50)
    {
        if (string.IsNullOrEmpty(symbol))
        {
            return BadRequest(Result.BadResult("Informe o símbolo do ativo"));
        }

        //Get from cache
        var cacheValue = await _redisService.GetCacheValueAsync<List<SearchStockInfo>>($"{symbol}_data");
        if (cacheValue != null)
        {
            return Ok(Result.SucessResult(cacheValue.Skip(( page - 1 ) * pageSize).Take(pageSize).ToList(), "Success!"));
        }

        //Request to cache
        var assetsCacheValue = await _redisService.GetCacheValueAsync<GetAssets>("assets_data") ?? throw new Exception("Não foi possível buscar os ativos do cache, tente novamente mais tarde");

        //search for the asset based on the symbol
        var assets = assetsCacheValue.Stocks
            .Where(x =>
                x.Stock.Contains(symbol, StringComparison.OrdinalIgnoreCase) ||
                x.Name.Contains(symbol, StringComparison.OrdinalIgnoreCase))
            .Skip(( page - 1 ) * pageSize)
            .Take(pageSize)
            .ToList();

        //Set cache for this symbol
        var jsonString = JsonConvert.SerializeObject(assets);
        await _redisService.SetCacheValueAsync($"{symbol}_data", jsonString);

        return Ok(Result.SucessResult(assets, "Success!"));
    }


    [HttpGet("{asset}")]
    public async Task<IActionResult> GetAssetAsync([FromRoute] string asset)
    {
        if (string.IsNullOrEmpty(asset))
        {
            return BadRequest(Result.BadResult("Invalid asset"));
        }

        //Get caches from redis
        var assetData = await _redisService.GetCacheValueAsync<MarketData>($"{asset}_asset_data");
        var assetNews = await _redisService.GetCacheValueAsync<List<NewArticle>>($"{asset}_asset_news");

        if (assetData != null && assetNews != null)
        {
            return Ok(Result.SucessResult(new { AssetData = assetData, AssetNews = assetNews }, "Success!"));
        }

        var symbol = asset.ToUpper();

        if (assetData == null)
        {
            //Request to Brapi Api
            assetData = await _assetsService.GetAssetsDataFromBrapi(symbol);

            //Set asset data in cache
            var jsonString = JsonConvert.SerializeObject(assetData);
            await _redisService.SetCacheValueAsync($"{asset}_asset_data", jsonString);
        }

        if (assetNews == null)
        {
            //Request to NewsApi
            assetNews = await _assetsService.GetAssetsNewsFromNewsApi(symbol);

            // Set asset news in cache with 24 hours expiration
            var jsonString = JsonConvert.SerializeObject(assetNews);
            await _redisService.SetCacheValueAsync($"{asset}_asset_news", jsonString, TimeSpan.FromHours(24));
        }

        return Ok(Result.SucessResult(new { AssetData = assetData, AssetNews = assetNews }, "Success!"));
    }


    [HttpGet("last-news")]
    public async Task<IActionResult> GetLastNewsAsync()
    {
        //Get cache from redis
        var lastNews = await _redisService.GetCacheValueAsync<List<NewArticle>>("last_news");

        if (lastNews != null)
        {
            return Ok(Result.SucessResult(lastNews, "Success!"));
        }

        //Get last news
        lastNews = await _assetsService.GetLastTenNewsArticles();

        //Set last new in redis cache
        var jsonString = JsonConvert.SerializeObject(lastNews);
        await _redisService.SetCacheValueAsync("last_news", jsonString, TimeSpan.FromHours(3));

        return Ok(Result.SucessResult(lastNews, "Success!"));
    }
}
