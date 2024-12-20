using System.Diagnostics;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using STOCKS;
using STOCKS.Clients;
using stocks.Data.Entities;
using STOCKS.Data.Repository.Connection;
using STOCKS.Models;

namespace stocks.Clients.Stocks
{
    public class StocksHttpClient : IStocksHttpClient
    {
        public IConnectionRepository _connectionRepository { get; }

        public StocksHttpClient(IConnectionRepository connectionRepository)
        {
            _connectionRepository = connectionRepository;
        }

        public StockOverviewApiModel GetCompanyOverview(string companySymbol)
        {
            var connection = GetTiingoConnection();

            try
            {
                using var client = new HttpClient
                {
                    BaseAddress = new Uri(connection.BaseUrl),
                    Timeout = TimeSpan.FromSeconds(30)
                };

                var response = client.GetAsync($"/tiingo/daily/{companySymbol}?token={DecodeBase64(connection.ClientSecret)}").Result;

                if(response.StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception($"There was an error trying to get a response from the external source. StatusCode: {response?.StatusCode}, Content: {response?.Content}");
                }

                return JsonConvert.DeserializeObject<StockOverviewApiModel>(response.Content.ReadAsStringAsync().Result);
            }
            catch (Exception e)
            {
                throw new Exception($"An error occured in StocksHttpClient: {e.Message}");
            }
        }

        public List<StockOverviewApiModel> GetCompanyOverviews(List<string> companySymbols)
        {
            var stopwatch = new Stopwatch();
            var tasks = new List<Task<HttpResponseMessage>>();
            var connection = GetTiingoConnection();

            var modelsList = new List<StockOverviewApiModel>();
            
            if(connection == null || connection.BaseUrl == null || connection.ClientSecret == null)
            {
                throw new Exception("The connection or the base Url was null, unable to retrieve data from external source");
            }

            try
            {
                using (var client = new HttpClient { BaseAddress = new Uri(connection.BaseUrl), Timeout = TimeSpan.FromSeconds(120) })
                {
                    companySymbols.ForEach(symbol =>
                    {
                        var response = client.GetAsync($"/tiingo/daily/{symbol}?token={DecodeBase64(connection.ClientSecret)}");

                        tasks.Add(response);
                    });
                    
                    Task.WaitAll(tasks.ToArray());
                }
            }
            catch (Exception e)
            {
                throw new Exception($"An error occured in StocksHttpClient: {e.Message}");
            }

            tasks.ForEach(task =>
            {
                try
                {
                    var data = task.Result;

                    if (data == null || data.StatusCode != HttpStatusCode.OK || data.Content == null)
                    {
                        Console.Write(
                            $"There was an error trying to get a response from the external source. StatusCode: {data?.StatusCode}, Content: {data?.Content}");
                    }

                    modelsList.Add(
                        JsonConvert.DeserializeObject<StockOverviewApiModel>(
                            data.Content.ReadAsStringAsync().Result));
                }
                catch (Exception e)
                {
                    Console.WriteLine($"An error occured in StocksHttpClient: {e.Message}");
                }
            });

                stopwatch.Stop();

                Console.Write(
                    $"Time to gather all Companies stock data: {stopwatch.ElapsedMilliseconds / 1000} seconds");
                
            return modelsList;
        }

        public async Task<StockOverviewandResponse> GetTimeSerie(StockOverview stockOverview, DateTime fromDate)
        {
            var connection = GetTiingoConnection();
            
            try
            {
                using var client = new HttpClient
                {
                    BaseAddress = new Uri(connection.BaseUrl),
                    Timeout = TimeSpan.FromSeconds(30)
                };

                var query = $"/tiingo/daily/{stockOverview.Symbol}/prices?startDate={fromDate:yyyy-MM-dd}&format=json&token={DecodeBase64(connection.ClientSecret)}";

                return new StockOverviewandResponse
                {
                    StockOverview = stockOverview,
                    Response = await client.GetAsync(query)
                };
            }
            catch (Exception e)
            {
                throw new Exception($"An error occured in StocksHttpClient: {e.Message}");
            }
        }

        private string DecodeBase64(string encodedCs)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(encodedCs)); 
        }

        private Connection GetTiingoConnection()
        {
            var connection = _connectionRepository
                .GetByName("Tiingo");

            if(connection == null || connection.BaseUrl == null || connection.ClientSecret == null)
            {
                throw new Exception("The connection or the base Url was null, unable to retrieve data from external source");
            }

            return connection;
        }
    }
}