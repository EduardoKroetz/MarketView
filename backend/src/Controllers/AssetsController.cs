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
    private readonly string _brapiBaseUrl;
    private readonly string _brapiApiKey;
    private readonly HttpClient _client;

    public AssetsController(RedisService redisService, IConfiguration configuration)
    {
        _redisService = redisService;
        _brapiBaseUrl = $"https://brapi.dev/api";
        _brapiApiKey = configuration["Brapi:ApiKey"] ?? throw new Exception("Invalid brapi api key");
        _client = new HttpClient();
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

        //Get from cache
        var cacheValue = await _redisService.GetCacheValueAsync<MarketData>($"{asset}_asset_data");
        if (cacheValue != null)
        {
            return Ok(Result.SucessResult(cacheValue, "Success!"));
        }

        //Request to Brapi api
        var response = await _client.GetAsync($"{_brapiBaseUrl}/quote/{asset.ToUpper()}?fundamental=true&modules=summaryProfile&interval=1d&range=3mo&token={_brapiApiKey}");
        if (!response.IsSuccessStatusCode)
        {
            return BadRequest(Result.BadResult("Um erro ocorreu ao buscar os dados"));
        }

        var jString = await response.Content.ReadAsStringAsync();
        var data = JsonConvert.DeserializeObject<ResultMarketData>(jString)!.Results[0];

        //Set content in cache
        var jsonString = JsonConvert.SerializeObject(data);
        await _redisService.SetCacheValueAsync($"{asset}_asset_data", jsonString);

        return Ok(Result.SucessResult(data, "Success!"));
    }
}
