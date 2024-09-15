using STOCKS.Clients;
using stocks.Data.Entities;
using STOCKS.Data.Repository;
using STOCKS.Data.Repository.AppUser;
using STOCKS.Mappers;
using STOCKS.Models;

namespace STOCKS.Services.StockOverviews
{
    public class StockOverviewService : IStocksOverviewService
    {
        private readonly IStocksHttpClient _stocksHttpClient;
        private readonly IStocksMapper _stocksMapper;
        private readonly IRepository<StockOverview> _stockOverviewRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public IAppUserRepository _appUserRepository { get; }

        public StockOverviewService(IStocksHttpClient stocksHttpClient, IAppUserRepository appUserRepository, IStocksMapper stocksMapper, IRepository<StockOverview> stockOverviewRepository, IHttpContextAccessor httpContextAccessor)
        {
            _appUserRepository = appUserRepository;
            _stocksHttpClient = stocksHttpClient;
            _stocksMapper = stocksMapper;
            _stockOverviewRepository = stockOverviewRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public ServiceResponse AddCompanyToUserProfile(string companySymbol)
        {
            //TODO: Corriger ceci
            var userProfile = new Appuser();

            if (userProfile == null)
            {
                return new ServiceResponse
                {
                    WasSuccessfull = false,
                    Message = "Could not retrieve user profile from current Session"
                };
            }

            var company = _stocksHttpClient.GetCompanyOverview(companySymbol);

            if(company == null || company.Symbol == null)
            {
                return new ServiceResponse
                {
                    WasSuccessfull = false,
                    Message = $"Could not retrieve company from company symbol : {companySymbol}"
                };
            }

            if (userProfile.Stockoverviews == null || !userProfile.Stockoverviews.Any())
            {
                userProfile.Stockoverviews = new List<StockOverview>();
            }
            
            userProfile.Stockoverviews.Add(_stocksMapper.StockOverViewApiToEntity(company, false));
            
            _appUserRepository.Save();

            return new ServiceResponse
            {
                WasSuccessfull = true,
                Message = "Company added successfully!"
            };
        }

        public ServiceResponse AddCompanyToSite(string companySymbol)
        {
            var alreadyExists = _stockOverviewRepository
                .GetAsQueryableAsNoTracking()
                .Any(s => s.Symbol.ToLower().Equals(companySymbol.ToLower()));

            if (alreadyExists)
            {
                return new ServiceResponse
                {
                    Message = $"{companySymbol} already exists in the system",
                    WasSuccessfull = false
                };
            }

            try
            {
                var apiData = _stocksHttpClient.GetCompanyOverview(companySymbol);

                _stockOverviewRepository.Add(_stocksMapper.StockOverViewApiToEntity(apiData, false));

                return new ServiceResponse
                {
                    Message = $"{companySymbol} added successfully!",
                    WasSuccessfull = true
                };
            }
            catch(Exception e)
            {
                return new ServiceResponse
                {
                    Message = $"An error occured while trying to add the new company information for {companySymbol}: {e.Message}",
                    WasSuccessfull = false
                };
            }
        }

        public ServiceResponse UpdateStockOverviews()
        {
            var activeData = _stockOverviewRepository
                .GetAsQueryable();

            var symbols = activeData
                .Select(d => d.Symbol)
                .ToList();

            var apiResponses = _stocksHttpClient
                .GetCompanyOverviews(symbols);
            
            apiResponses.ForEach(response =>
            {
                var correspondingDbData = activeData
                    .FirstOrDefault(d => d.Symbol.Equals(response.Symbol));

                if (correspondingDbData != null)
                {
                    _stocksMapper.StockOverViewApiToEntityUpdate(response, correspondingDbData);
                    _stockOverviewRepository.Update(correspondingDbData);
                }
            });

            return new ServiceResponse
            {
                Message = "Company Stock Overviews Updated Successfully!",
                WasSuccessfull = true
            };
        }

        public TServiceResponse<List<StockOverviewDto>> GetExistingStockOverviews(string query, int top)
        {
            try
            {
                var dbData = RetrieveStockFromQuery(query);

                if (dbData.Any())
                {
                    return new TServiceResponse<List<StockOverviewDto>>
                    {
                        Data = dbData
                            .Select(ov => _stocksMapper.MapStockOverviewToDto(ov))
                            .DistinctBy(ov => ov.Symbol)
                            .OrderBy(ov => ov.Symbol)
                            .ThenBy(ov => ov.Name)
                            .Take(top)
                            .ToList(),
                        Message = "Query successfully executed!",
                        WasSuccessfull = true
                    };
                }

                return new TServiceResponse<List<StockOverviewDto>>
                {
                    WasSuccessfull = true,
                    Message = "This title could not be retrieved from database, try adding it with endpoint AddOverview/symbol={symbol}"
                };
            }
            catch(Exception e)
            {
                Console.WriteLine($"An exception occured while retrieving the company information for query: {query}...  {e.Message}");

                return new TServiceResponse<List<StockOverviewDto>>
                {
                    WasSuccessfull = false,
                    Message = "An error occured while retrieving the company information for query."
                };
            }
        }

        public List<StockOverviewDto> GetUserStockOverviews(string userEmail)
        {
            return _stockOverviewRepository
                .GetAsQueryableAsNoTracking()
                .Where(s => s.Appusers
                    .Any(u => u.Email == userEmail))
                .Select(s => _stocksMapper.MapStockOverviewToDto(s))
                .ToList();
        }

        private List<StockOverview> RetrieveStockFromQuery(string query)
        {
            return _stockOverviewRepository
                .GetAsQueryableAsNoTracking()
                .Where(s =>
                    s.Name.Contains(query)
                    || s.Symbol.Contains(query)
                    || s.Exchange.Contains(query))
                .ToList();
        }
    }
}