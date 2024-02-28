using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using STOCKS;
using STOCKS.Clients;
using STOCKS.Data.Repository;
using STOCKS.Data.Repository.StockHistory;
using STOCKS.Data.Repository.StockOverview;
using STOCKS.Mappers;
using STOCKS.Models;

namespace stocks.Services;

public class TimeSeriesService : ITimeSeriesService
{
    private readonly IStocksHttpClient _stocksHttpClient;
    private readonly IStocksMapper _stocksMapper;
    private readonly IStockOverviewRepository _stockOverviewRepository;
    private readonly IStockHistoryRepository _stockHistoryRepository;

    public TimeSeriesService(IStocksHttpClient stocksHttpClient, IStocksMapper stocksMapper, IStockOverviewRepository stockOverviewRepository, IStockHistoryRepository stockHistoryRepository)
    {
        _stocksHttpClient = stocksHttpClient;
        _stocksMapper = stocksMapper;
        _stockOverviewRepository = stockOverviewRepository;
        _stockHistoryRepository = stockHistoryRepository;
    }

    public TServiceResponse<TimeSerieApiModel> GetTimeSerie(string symbol, int? intervalMinutes)
    {
        if (string.IsNullOrEmpty(symbol))
        {
            return new TServiceResponse<TimeSerieApiModel>
            {
                WasSuccessfull = false,
                Message = "No symbol provided"
            };
        }

        var data = _stocksHttpClient.GetTimeSerie(symbol, intervalMinutes ?? 60);

        return null;
    }

    public ServiceResponse SaveAllSeries(int? intervalMinutes)
    {
        var symbols = _stockOverviewRepository
            .GetAsQueryableAsNoTracking()
            .Select(s => s.Symbol)
            .ToList();

        GetAndSaveData(symbols, intervalMinutes ?? 60);

        return new ServiceResponse
        {
            WasSuccessfull = true,
            Message = "Data saved successfully!"
        };
    }

    private bool SaveSeries(TimeSerieApiModel? model)
    {
        try
        {
            var currentData = _stockHistoryRepository
                .GetAsQueryableAsNoTracking()
                .Where(s => s.StockOverview.Symbol == model.Meta.Symbol)
                .Select(s => s.Date.DateTime)
                .ToList();

            var stockOverview = _stockOverviewRepository
                .GetAsQueryableAsNoTracking()
                .FirstOrDefault(s => s.Symbol == model.Meta.Symbol);

            if (stockOverview != null)
            {
                model.TimeSerie
                    .Where(t => !currentData.Contains(t.Key))
                    .Distinct()
                    .Select(t => _stocksMapper.MapTimeSerieToEntity(t, stockOverview))
                    .ForEach(sh =>
                    {
                        _stockHistoryRepository.Add(sh);
                    });
                
                _stockHistoryRepository.Save();
                
                return true;
            }

            Console.WriteLine($"Histories :: TimeSeriesService - Could not save time series for Overview : {model?.Meta?.Symbol}");
            return false;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Histories :: TimeSeriesService - An error occured while trying to save the time series: {e.Message}");
            return false;
        }
    }

    private void GetAndSaveData(List<string> symbols, int? intervalMinutes)
    {
        var responseList = new List<Task<HttpResponseMessage>>();
        
        symbols.ForEach(symbol =>
        {
            try
            {
                Console.WriteLine($"Histories :: TimeSeriesService - Getting Time Series for symbol: {symbol}");
                var response = _stocksHttpClient.GetTimeSerie(symbol, intervalMinutes ?? 60);
                    
                responseList.Add(response);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Histories :: TimeSeriesService - An error occured while trying to get the time series: {e.Message}");
            }
        });
        
        responseList
            .Select(d => d.Result.Content.ReadAsStringAsync().Result)
            .ForEach(json =>
            {
                if (!string.IsNullOrEmpty(json))
                {
                    var data = JsonConvert.DeserializeObject<TimeSerieApiModel>(json);
                    if (data != null && data.TimeSerie.Any())
                    {
                        SaveSeries(data);
                    }
                    else
                    {
                        Console.WriteLine($"Histories :: TimeSeriesService - Warning, trying to recover API data, but empty");
                    }
                }
            });
    }
}