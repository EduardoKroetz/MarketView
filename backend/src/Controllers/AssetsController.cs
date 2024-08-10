using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using src.DTOs;
using src.DTOs.GetAssets;
using src.Services;

namespace src.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AssetsController : Controller
{
    private readonly RedisService _redisService;
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public AssetsController(RedisService redisService, IConfiguration configuration)
    {
        _configuration = configuration;
        _redisService = redisService;
        _httpClient = new HttpClient();
    }

    [HttpGet]
    public async Task<IActionResult> GetBySymbolAsync([FromQuery] string symbol, [FromQuery] int page = 1, [FromQuery] int pageSize = 50)
    {
        if (string.IsNullOrEmpty(symbol))
        {
            return BadRequest(Result.BadResult("Informe o símbolo do ativo"));
        }

        //Get from cache
        var cacheValue = await _redisService.GetCacheValueAsync<List<StockInfo>>($"{symbol}_data");
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
}
