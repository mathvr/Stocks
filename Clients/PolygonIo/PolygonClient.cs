using System.ComponentModel.DataAnnotations;
using System.Text;
using Newtonsoft.Json;
using STOCKS;
using STOCKS.Data.Repository.Connection;
using STOCKS.Models;

namespace stocks.Clients.PolygonIo;

public class PolygonClient : IPolygonClient
{
    private readonly IConnectionRepository _connectionRepository;

    public PolygonClient(IConnectionRepository connectionRepository)
    {
        _connectionRepository = connectionRepository;
    }

    public PolygonSplitApiModel? GetPolygonSplitModel()
    {
        var connection = GetPolygonConnection();
        
        try
        {
            using var client = new HttpClient
            {
                BaseAddress = new Uri(connection.BaseUrl),
                Timeout = TimeSpan.FromSeconds(30)
            };

            var query = $"/v3/reference/splits?limit=1000&apiKey={DecodeBase64(connection.ClientSecret)}";

            var data = client.GetAsync(query).Result?.Content?.ReadAsStringAsync().Result;

            if (string.IsNullOrEmpty(data))
            {
                return null;
            }
            
            return JsonConvert.DeserializeObject<PolygonSplitApiModel>(data);
        }
        catch (Exception e)
        {
            throw new Exception($"An error occured in StocksHttpClient: {e.Message}");
        }
    }
    
    public PolygonSplitApiModel? GetPolygonSplitModelFromUrl(string url)
    {
        var connection = GetPolygonConnection();
        
        try
        {
            using var client = new HttpClient
            {
                BaseAddress = new Uri(connection.BaseUrl),
                Timeout = TimeSpan.FromSeconds(30)
            };

            var query = $"{url.Remove(0, connection.BaseUrl.Length -1)}&apiKey={DecodeBase64(connection.ClientSecret)}";

            var data = client.GetAsync(query).Result?.Content?.ReadAsStringAsync().Result;

            if (string.IsNullOrEmpty(data))
            {
                return null;
            }
            
            return JsonConvert.DeserializeObject<PolygonSplitApiModel>(data);
        }
        catch (Exception e)
        {
            throw new Exception($"An error occured in StocksHttpClient: {e.Message}");
        }
    }
    
    
    private Connection GetPolygonConnection()
    {
        var connection = _connectionRepository
            .GetByName("Polygon");

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