using stocks.Clients.PolygonIo;
using stocks.Data.Entities;
using STOCKS.Data.Repository;
using STOCKS.Mappers;
using STOCKS.Models;

namespace stocks.Services.Splits;

public class SplitService : ISplitService
{
    private readonly IPolygonClient _polygonClient;
    private readonly IRepository<Split> _splitRepository;
    private readonly IRepository<StockOverview> _stockOverviewRepository;
    private readonly IStocksMapper _stocksMapper;

    public SplitService(IPolygonClient polygonClient, IRepository<Split> splitRepository, IRepository<StockOverview> stockOverviewRepository, IStocksMapper stocksMapper)
    {
        _polygonClient = polygonClient;
        _splitRepository = splitRepository;
        _stockOverviewRepository = stockOverviewRepository;
        _stocksMapper = stocksMapper;
    }

    
    public ServiceResponse AddSplits()
    {
        var splitResponse = _polygonClient.GetPolygonSplitModel();
        var nextUrl = string.Empty;
        
        var existingSplitIds = _splitRepository
            .GetAsQueryableAsNoTracking()
            .Select(s => s.SplitApiId)
            .ToList();

        if (splitResponse == null)
        {
            return new ServiceResponse
            {
                WasSuccessfull = false,
                Message = "Could not get Split data from Polygon API"
            };
        }

        if(!SaveSplits(splitResponse, existingSplitIds))
        {
            return new ServiceResponse
            {
                WasSuccessfull = false,
                Message = "An error occured while saving the Split data"
            };
        }
        nextUrl = splitResponse.nextUrl;

        while (!string.IsNullOrEmpty(nextUrl))
        {
            Thread.Sleep(20000);
            var nextSplitResponse = _polygonClient.GetPolygonSplitModelFromUrl(nextUrl);
            
            if (nextSplitResponse == null)
            {
                return new ServiceResponse
                {
                    WasSuccessfull = false,
                    Message = "Could not get Split data from Polygon API"
                };
            }
            nextUrl = nextSplitResponse.nextUrl;
            
            if(!SaveSplits(splitResponse, existingSplitIds))
            {
                return new ServiceResponse
                {
                    WasSuccessfull = false,
                    Message = "An error occured while saving the Split data"
                };
            }
        }

        return new ServiceResponse
        {
            WasSuccessfull = true,
            Message = "Successfully added Split data from Polygon API"
        };
    }
    
    private bool SaveSplits(PolygonSplitApiModel polygonSplitApiModel, List<string> existingSplitIds)
    {
        try
        {
            var splits = polygonSplitApiModel.Results
                .Where(s => !existingSplitIds.Contains(s.SplitApiId))
                .ToList();

            foreach (var split in splits)
            {
                _splitRepository.Add(_stocksMapper.MapSplitToEntity(split));
            }
            
            _splitRepository.Save();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }

        return true;
    }
    
}