using Microsoft.EntityFrameworkCore;
using stocks.Data.Entities;
using STOCKS.Data.Repository;
using STOCKS.Models;

namespace stocks.Services.Computation;

public class ComputationService : IComputationService
{
    private readonly IRepository<StockOverview> _stockOverviewRepository;
    private readonly IRepository<StockHistory>  _stockHistoryRepository;
    private readonly IRepository<Split> _splitRepository;

    public ComputationService(IRepository<StockOverview> stockOverviewRepository, IRepository<StockHistory> stockHistoryRepository, IRepository<Split> splitRepository)
    {
        _stockOverviewRepository = stockOverviewRepository;
        _stockHistoryRepository = stockHistoryRepository;
        _splitRepository = splitRepository;
    }
    public StockProgressionModels GetByMostProgression(DateTime startDate, DateTime endDate)
    {
        var existingSymbols = GetExistingSymbols();
        
        var splits = GetSplits(existingSymbols, startDate, endDate);
            
        var models = _stockHistoryRepository
            .GetAsQueryableAsNoTracking()
            .Include(s => s.StockOverview)
            .Where(s => s.Date >= startDate && s.Date <= endDate)
            .ToList()
            .AsParallel()
            .GroupBy(s => s.StockOverview.Symbol)
            .Select(s =>
            {
                var differenceData = GetHistoryDifference(s, splits, startDate, endDate);
                var name = $"{s.FirstOrDefault()?.StockOverview?.Name} ({s.FirstOrDefault()?.StockOverview?.Symbol})";
                var lastValue = s.OrderByDescending(s => s.Date).FirstOrDefault();
                return new StockProgressionModel
                {
                    Name = name,
                    Difference = differenceData.Difference,
                    Percent = Decimal.Round(differenceData.Percent,0),
                    HasSplit = splits.Any(split => split.Symbol.Equals(s.Key)),
                    CurrentValue = lastValue?.CloseValue,
                    ValueDate = lastValue?.Date.ToShortDateString(),
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

    private StockDifference GetHistoryDifference(IGrouping<string, StockHistory> group, List<Split> splits, DateTime startDate, DateTime endDate)
    {
        var endValue = group
            .OrderByDescending(d => d.Date)
            .First();

        var firstValue = group
            .OrderBy(d => d.Date)
            .First();

        var split = splits
            .Where(s => s.Symbol.Equals(group.Key));

        if (split.Any())
        {
            var ratio = GetSplitRatio(splits);
            endValue.CloseValue *= ratio;
            endValue.OpenValue *= ratio;
        }

        return new StockDifference
        {
            Difference = endValue.CloseValue - firstValue.OpenValue ?? 0,
            Percent = ((endValue.CloseValue - firstValue.CloseValue) / (endValue.CloseValue + firstValue.OpenValue) / 2) * 100 ?? 0
        };

    }

    private decimal GetSplitRatio(IEnumerable<Split> splits)
    {
        var ratio = 1m;
        
        foreach (var split in splits)
        {
            var currentRatio = split.NewValue / split.InitialValue;
            ratio *= currentRatio;
        }

        return ratio;
    }

    private List<string> GetExistingSymbols()
    {
        return _stockOverviewRepository
            .GetAsQueryableAsNoTracking()
            .Select(s => s.Symbol)
            .ToList();
    }

    private List<Split> GetSplits(List<string> existingSymbols, DateTime startDate, DateTime endDate)
    {
        return _splitRepository
            .GetAsQueryableAsNoTracking()
            .Where(s =>
                s.ExecutionDate >= startDate
                && s.ExecutionDate <= endDate
                && existingSymbols.Contains(s.Symbol))
            .ToList();
    }
}