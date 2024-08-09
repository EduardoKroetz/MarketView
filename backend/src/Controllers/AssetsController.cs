using Microsoft.AspNetCore.Mvc;

namespace src.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssetsController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly string _alphaVantageApiKey;
        private readonly string _newsApiKey;
        private readonly string _alphaVantageBaseUrl;
        private readonly HttpClient _client;

        public AssetsController(IConfiguration configuration)
        {
            _configuration = configuration;
            _alphaVantageApiKey = configuration["AlphaVantage:ApiKey"] ?? throw new Exception("Invalid alpha vantage api key");
            _newsApiKey = configuration["AlphaVantage:ApiKey"] ?? throw new Exception("Invalid alpha vantage api key");
            _alphaVantageBaseUrl = $"https://www.alphavantage.co/query?apikey={_alphaVantageApiKey}";
            _client = new HttpClient();
        }

        [HttpGet]
        public async Task GetAssetsByNameAsync([FromQuery] string name, [FromQuery] int? skip, [FromQuery] int? take)
        {
            var response = _client.GetAsync($"{_alphaVantageBaseUrl}&symbol=");   
        }

        [HttpGet]
        public async Task GetAssetByIdAsync([FromQuery] string id)
        {

        }
    }
}
