using Microsoft.EntityFrameworkCore;
using stocks.Clients.OpenAi;
using stocks.Data.Entities;
using STOCKS.Data.Repository;
using STOCKS.Mappers;
using STOCKS.Models;

namespace stocks.Services.Reputation;

public class ReputationService : IReputationService
{
    private readonly IRepository<Data.Entities.Reputation> _reputationRepository;
    private readonly IRepository<StockOverview> _stockOverviewRepository;
    private readonly IOpenAiClient _openAiClient;
    private readonly IStocksMapper _mapper;

    public ReputationService(IRepository<Data.Entities.Reputation> reputationRepository, IRepository<StockOverview> stockOverviewRepository, IOpenAiClient openAiClient, IStocksMapper mapper)
    {
        _reputationRepository = reputationRepository;
        _stockOverviewRepository = stockOverviewRepository;
        _openAiClient = openAiClient;
        _mapper = mapper;
    }

    public ServiceResponse DownloadReputations()
    {
        var overviews = GetOverviewsToUpdate();

        var reputationModels = _openAiClient.GetReputations(overviews.Select(o => o.Symbol).ToList())
            .Select(m => m.Choices.FirstOrDefault()?.Message?.CompanyReputation)
            .ToList();

        try
        {
            foreach (var model in reputationModels)
            {
                if (model == null)
                {
                    continue;
                }
            
                var overview = overviews.FirstOrDefault(o => model?.Symbol == o.Symbol);

                if (overview == null)
                {
                    Console.WriteLine($"DownloadReputations :: ReputationService - Could not find overview for symbol {model?.Symbol}");
                    continue;
                }

                if (overview.Reputation != null)
                {
                    var reputation = overview.Reputation;
                    reputation = _mapper.MapReputationToEntity(model, overview);
                    reputation.ReputationFacts = _mapper.MapReputationFacts(model, overview.Reputation);
                    _reputationRepository.Update(reputation);
                }
                else
                {
                    var entity = _mapper.MapReputationToEntity(model, overview);
                    _reputationRepository.Add(entity);
                    _reputationRepository.Save();
                
                    entity.ReputationFacts = _mapper.MapReputationFacts(model, entity);
                    _reputationRepository.Update(entity);
                }
            
                _reputationRepository.Save();
            }
        }
        catch (Exception e)
        {
            return new ServiceResponse
            {
                Message = e.Message,
                WasSuccessfull = false
            };
        }

        return new ServiceResponse
        {
            WasSuccessfull = true,
            Message = "Reputations downloaded successfully!"
        };
    }

    private List<StockOverview> GetOverviewsToUpdate()
    {
        return _stockOverviewRepository
            .GetAsQueryableAsNoTracking()
            .Include(o => o.Reputation)
            .Where(o => o.Reputation == null || o.Reputation.ModifiedOn < DateTime.Now.AddDays(60))
            .ToList();
    }
}