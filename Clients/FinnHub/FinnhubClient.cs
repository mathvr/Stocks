using System.Text;
using STOCKS;
using STOCKS.Data.Repository;
using STOCKS.Data.Repository.Connection;

namespace stocks.Clients.FinnHub;

public class FinnhubClient(IConnectionRepository _connectionRepository) : IFinnhubClient
{
    public async Task<HttpResponseMessage>? GetFinancialsBySymbol(string symbol, string? forYear = null)
    {
        var from = forYear != null ? $"{forYear}-01-01" : DateTime.Now.AddYears(-1).ToString("yyyy-MM-dd");
        var to = forYear != null ? $"{forYear}-12-31" : DateTime.Now.ToString("yyyy-MM-dd");

        var connection = GetFinnHubApiConnection();
        
        try
        {
            using var client = new HttpClient
            {
                BaseAddress = new Uri(connection.BaseUrl), 
                Timeout = TimeSpan.FromSeconds(30),
            };
            
            client.DefaultRequestHeaders.Add("X-Finnhub-Token", DecodeBase64(connection.ClientSecret));
            
            var response = client.GetAsync($"{connection.BaseUrl}/api/v1/stock/financials-reported?symbol={symbol}&from={from}&to={to}");

            return await response;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Failed to get Financial data for Symbol : {symbol}. Error: {e.Message}");
        }

        return null;
    }
    
    private string DecodeBase64(string encodedCs)
    {
        return Encoding.UTF8.GetString(Convert.FromBase64String(encodedCs)); 
    }
    
    private Connection GetFinnHubApiConnection()
    {
        var connection = _connectionRepository
            .GetByName("Finnhub");
        
        if(connection == null || connection.BaseUrl == null || connection.ClientSecret == null)
        {
            throw new Exception("The connection or the base Url was null, unable to retrieve data from external source");
        }

        return connection;
    }
}