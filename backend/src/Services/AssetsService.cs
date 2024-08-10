using Newtonsoft.Json;
using src.DTOs;

namespace src.Services;

public class AssetsService
{
    private readonly string _brapiApiKey;
    private readonly string _newsApiKey;
    private readonly HttpClient _client;

    public AssetsService(IConfiguration configuration)
    {
        _brapiApiKey = configuration["Brapi:ApiKey"] ?? throw new Exception("Invalid brapi api key");
        _newsApiKey = configuration["NewsApi:ApiKey"] ?? throw new Exception("Invalid news api key");
        _client = new HttpClient();
        _client.DefaultRequestHeaders.UserAgent.ParseAdd("MarketView/1.0");
    }

    public async Task<List<NewArticle>> GetAssetsNewsFromNewsApi(string symbol)
    {
        var responseNews = await _client.GetAsync($"https://newsapi.org/v2/everything?q={symbol}&language=pt&sortBy=popularity&apiKey={_newsApiKey}&pageSize=10");
        var newsContent = new List<NewArticle>();
        if (responseNews.IsSuccessStatusCode)
        {
            newsContent = JsonConvert.DeserializeObject<ResponseNews>(await responseNews.Content.ReadAsStringAsync())!.Articles;
        }
        return newsContent;
    }

    public async Task<List<NewArticle>> GetLastTenNewsArticles()
    {
        var responseNews = await _client.GetAsync($"https://newsapi.org/v2/everything?q=mercado%20financeiro&language=pt&sortBy=publishedAt&apiKey={_newsApiKey}&pageSize=10");
        var newsContent = new List<NewArticle>();
        if (responseNews.IsSuccessStatusCode)
        {
            newsContent = JsonConvert.DeserializeObject<ResponseNews>(await responseNews.Content.ReadAsStringAsync())!.Articles;
        }
        return newsContent;
    }

    public async Task<MarketData> GetAssetsDataFromBrapi(string symbol)
    {
        var response = await _client.GetAsync($"https://brapi.dev/api/quote/{symbol}?fundamental=true&modules=summaryProfile&interval=1d&range=3mo&token={_brapiApiKey}");
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Ocorreu um erro ao buscar os dados, tente novamente mais tarde");
        }
        var data = JsonConvert.DeserializeObject<ResultMarketData>(await response.Content.ReadAsStringAsync())!.Results[0];
        return data;
    }

 
    
}
