using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using STOCKS.Clients;
using stocks.Data.Entities;
using STOCKS.Data.Repository.StockHistory;
using STOCKS.Data.Repository.StockOverview;
using STOCKS.Mappers;
using STOCKS.Models;

namespace stocks.Services.TimeSeries;

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

    public ServiceResponse SaveAllSeries(DateTime fromDate)
    {
        var overviews = _stockOverviewRepository
            .GetAsQueryable()
            .ToList();

        GetAndSaveData(overviews, fromDate);

        return new ServiceResponse
        {
            WasSuccessfull = true,
            Message = "Data saved successfully!"
        };
    }

    private bool SaveSeries(List<TimeSerieApiModel>? models, StockOverview stockOverview)
    {
        if (models == null || !models.Any())
        {
            return false;
        }

        try
        {
            var savedHistories = _stockHistoryRepository
                .GetAsQueryableAsNoTracking()
                .Where(s => s.StockOverview.Symbol == stockOverview.Symbol)
                .Select(s => s.Date)
                .ToList();

            foreach (var timeSerie in models.Where(t => !savedHistories.Contains(t.Date)).Distinct())
            {
                var entity = _stocksMapper.MapTimeSerieToEntity(timeSerie, stockOverview);
                _stockHistoryRepository.Add(entity);
            }

            _stockHistoryRepository.Save();

            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Histories :: TimeSeriesService - An error occured while trying to save the time series: {e.Message}");
            return false;
        }
    }

    private void GetAndSaveData(List<StockOverview> overviews, DateTime fromDate)
    {
        var responseList = new List<Task<StockOverviewandResponse>>();
        
        overviews
            .AsParallel()
            .ForEach(stock =>
        {
            try
            {
                Console.WriteLine($"Histories :: TimeSeriesService - Getting Time Series for symbol: {stock.Symbol}");
                var response = _stocksHttpClient.GetTimeSerie(stock, fromDate);
                    
                responseList.Add(response);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Histories :: TimeSeriesService - An error occured while trying to get the time series: {e.Message}");
            }
        });
        
        responseList
            .ForEach(task =>
            {
                var data = task.Result;
                var result = data?.Response?.Content.ReadAsStringAsync().Result;
                
                if (!string.IsNullOrEmpty(result) && data?.StockOverview?.Symbol != null)
                {
                    var timeSeries = JsonConvert.DeserializeObject<List<TimeSerieApiModel>>(result);
                    SaveSeries(timeSeries, data.StockOverview);
                }
                else
                {
                    Console.WriteLine($"Histories :: TimeSeriesService - Warning, trying to recover API data, but empty");
                }
            });
    }
}