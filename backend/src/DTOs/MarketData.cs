namespace src.DTOs;

public class MarketData
{
    public string LongName { get; set; }
    public string Symbol { get; set; }
    public string UsedInterval { get; set; }
    public string UsedRange { get; set; }
    public string LogoUrl { get; set; }
    public SummaryProfile SummaryProfile { get; set; }

    public List<HistoricalData> HistoricalDataPrice { get; set; }
}

public record ResultMarketData(List<MarketData> Results);

public class SummaryProfile
{
    public string Address1 { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Country { get; set; }
    public string Website { get; set; }
    public string Industry { get; set; }
    public string Sector { get; set; }
    public string LongBusinessSummary { get; set; }
}


public class HistoricalData
{
    public long Date { get; set; }
    public double Open { get; set; }
    public double High { get; set; }
    public double Low { get; set; }
    public double Close { get; set; }
    public long Volume { get; set; }
    public double AdjustedClose { get; set; }
}
