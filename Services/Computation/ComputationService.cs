using Microsoft.EntityFrameworkCore;
using stocks.Data.Entities;
using STOCKS.Data.Repository.StockHistory;
using STOCKS.Data.Repository.StockOverview;
using STOCKS.Models;

namespace stocks.Services.Computation;

public class ComputationService : IComputationService
{
    private readonly IStockOverviewRepository _stockOverviewRepository;
    private readonly IStockHistoryRepository _stockHistoryRepository;

    public ComputationService(IStockOverviewRepository stockOverviewRepository, IStockHistoryRepository stockHistoryRepository)
    {
        _stockOverviewRepository = stockOverviewRepository;
        _stockHistoryRepository = stockHistoryRepository;
    }
    public StockProgressionModels GetByMostProgression(DateTime startDate, DateTime endDate)
    {
        var models = _stockHistoryRepository
            .GetAsQueryableAsNoTracking()
            .Include(s => s.StockOverview)
            .Where(s => s.Date >= startDate && s.Date <= endDate)
            .ToList()
            .AsParallel()
            .GroupBy(s => s.StockOverview.Symbol)
            .Select(s =>
            {
                var differenceData = GetHistoryDifference(s);
                return new StockProgressionModel
                {
                    Symbol = s.Key,
                    Difference = differenceData.Difference,
                    Percent = Decimal.Round(differenceData.Percent,0),
                    HasSplit = default //TODO Manage Splits
                };
            })
            .OrderByDescending(s => s.Percent)
            .ToList();

        return new StockProgressionModels
        {
            StartDate = DateOnly.FromDateTime(startDate),
            EndDate = DateOnly.FromDateTime(endDate),
            Models = models,
            ResultsCount = models.Count
        };
    }

    private StockDifference GetHistoryDifference(IGrouping<string, StockHistory> group)
    {
        var endValue = group
            .OrderByDescending(d => d.Date)
            .First();

        var firstValue = group
            .OrderBy(d => d.Date)
            .First();

        return new StockDifference
        {
            Difference = endValue.CloseValue - firstValue.OpenValue ?? 0,
            Percent = ((endValue.CloseValue - firstValue.CloseValue) / (endValue.CloseValue + firstValue.OpenValue) / 2) * 100 ?? 0
        };

    }
}