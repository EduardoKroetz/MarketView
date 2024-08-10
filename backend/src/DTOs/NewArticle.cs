namespace src.DTOs;

public class NewArticle
{
    public string Author { get; set; }
    public string Title { get; set; }
    public string PublishedAt { get; set; }
    public string Url { get; set; }
    public string UrlToImage { get; set; }
}

public record ResponseNews(List<NewArticle> Articles);