using System.Text;
using STOCKS;
using STOCKS.Clients;
using stocks.Data.Entities;
using STOCKS.Data.Repository.Connection;
using STOCKS.Models;

namespace stocks.Clients.News;

public class NewsHttpClient : INewsHttpClient
{
    private readonly IConnectionRepository _connectionRepository;

    public NewsHttpClient(IConnectionRepository connectionRepository)
    {
        _connectionRepository = connectionRepository;
    }

    public async Task<StockOverviewandResponse> GetNewsForTitleAsync(int daysAgo, StockOverview stockOverview)
    {
        var connection = GetNewsApiConnection();
        var from = DateTime.Now.AddDays(-daysAgo).ToString("yyyy-MM-dd");
        var to = DateTime.Now.ToString("yyyy-MM-dd");
        
        try
        {
            using var client = new HttpClient
            {
                BaseAddress = new Uri(connection.BaseUrl), 
                Timeout = TimeSpan.FromSeconds(30),
            };
            
            client.DefaultRequestHeaders.Add("User-Agent", "C# App");
            
            var requestUri = $"/v2/everything?q={stockOverview.Symbol}&from={from}&to={to}&apiKey={DecodeBase64(connection.ClientSecret)}";

            return new StockOverviewandResponse
            {
                StockOverview = stockOverview,
                Response = await client.GetAsync(requestUri)
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }

    private Connection GetNewsApiConnection()
    {
        var connection = _connectionRepository
            .GetByName("NewsApi");
        
        if(connection == null || connection.BaseUrl == null || connection.ClientSecret == null)
        {
            throw new Exception("The connection or the base Url was null, unable to retrieve data from external source");
        }

        return connection;
    }
    
    private string DecodeBase64(string encodedCs)
    {
        return Encoding.UTF8.GetString(Convert.FromBase64String(encodedCs)); 
    }
}