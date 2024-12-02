using Newtonsoft.Json;
using stocks.Clients.FinnHub;
using stocks.Data.Entities;
using STOCKS.Data.Repository;
using STOCKS.Mappers;
using STOCKS.Models;
using STOCKS.Models.Financials;

namespace stocks.Services.Financials;

public class FinancialsService (IRepository<Data.Entities.Financials> _financialRepository, IFinnhubClient _finnhubClient, IRepository<StockOverview> _stockOverviewRepository, IStocksMapper _mapper) : IFinancialsService
{
    public ServiceResponse CreateForAllSymbols(string? forYear = null)
    {
        var activeSymbols = _stockOverviewRepository
            .GetAsQueryableAsNoTracking()
            .Select(o => o.Symbol)
            .ToList();

        var currentFinancials = _financialRepository
            .GetAsQueryableAsNoTracking()
            .Select(s => new {s.Year, s.Symbol})
            .Distinct()
            .ToDictionary(s => s.Symbol, s => s.Year);
        
        var entities = activeSymbols
            .Select(s =>
            {
                var response = _finnhubClient.GetFinancialsBySymbol(s, forYear);
                var data = response?.Result?.Content?.ReadAsStringAsync()?.Result;
                
                return data == null ? null : JsonConvert.DeserializeObject<GetFinancialsApiModel>(data);
            })
            .SelectMany(_mapper.MapToFinancials)
            .ToList();

        try
        {
            foreach (var entity in entities.Where(e => !currentFinancials.ContainsKey(e.Symbol) && !currentFinancials.ContainsKey(e.Symbol)))
            {
                _financialRepository.Add(entity);
                _financialRepository.Save();
            }
            
            Console.WriteLine("Add financials successful");
            return new ServiceResponse
            {
                Message = "Add Financials successful",
                WasSuccessfull = true
            };
        }
        catch (Exception e)
        {
            Console.Error.WriteLine($"An error occured while adding new financials ... {e.Message}");
            return new ServiceResponse
            {
                Message = "An error occured while adding new financials",
                WasSuccessfull = false,
            };
        }
    }

    public TServiceResponse<List<FinancialDto>> GetBySymbol(string symbol)
    {
        try
        {
            var data = _financialRepository
                .GetAsQueryableAsNoTracking()
                .Where(f => f.Symbol == symbol)
                .ToList()
                .GroupBy(f => f.Year)
                .Select(f =>
                {
                    return new FinancialDto
                    {
                        Symbol = f.First().Symbol,
                        Year = f.First().Year,
                        FromDate = f.First().FromDate.ToString("yyyy-MM-dd"),
                        ToDate = f.First().ToDate.ToString("yyyy-MM-dd"),
                        Properties = f.Select(s => new PropertyDto
                        {
                            Name = s.PropertyName,
                            Value = !s.Unit.Contains("/") ? $"${(s.Value/1000000):F0}M" : s.Value.ToString("F0"),
                            Unit = s.Unit
                        })
                            .OrderBy(s => s.Name)
                            .ToList()
                    };
                }).ToList();

            return new TServiceResponse<List<FinancialDto>>
            {
                Data = data,
                Message = "Get Financials successful",
                WasSuccessfull = true
            };
        }
        catch (Exception e)
        {
            Console.Error.WriteLine($"An error occured while getting financials ... {e.Message}");
            return new TServiceResponse<List<FinancialDto>>
            {
                Message = "An error occured while getting financials",
                WasSuccessfull = false
            };
        }
    }
}