using Microsoft.IdentityModel.Tokens;
using STOCKS.Clients;
using STOCKS.Data.Repository.AppUser;
using STOCKS.Data.Repository.StockOverview;
using STOCKS.Mappers;
using STOCKS.Models;

namespace STOCKS.Services.StockOverviews
{
    public class StockOverviewService : IStocksOverviewService
    {
        private readonly IStocksHttpClient _stocksHttpClient;
        private readonly IStocksMapper _stocksMapper;
        private readonly IStockOverviewRepository _stockOverviewRepository;
        public IAppUserRepository _appUserRepository { get; }

        public StockOverviewService(IStocksHttpClient stocksHttpClient, IAppUserRepository appUserRepository, IStocksMapper stocksMapper, IStockOverviewRepository stockOverviewRepository)
        {
            _appUserRepository = appUserRepository;
            _stocksHttpClient = stocksHttpClient;
            _stocksMapper = stocksMapper;
            _stockOverviewRepository = stockOverviewRepository;
        }

        public ServiceResponse AddCompanyToUserProfile(string userEmail, string companySymbol)
        {
            var userProfile = _appUserRepository
                .GetByEmail(userEmail, true);

            if (userProfile == null)
            {
                return new ServiceResponse
                {
                    WasSuccessfull = false,
                    Message = $"Could not retrieve user profile from provided user email : {userEmail}"
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

            if (userProfile.Stockoverviews.IsNullOrEmpty())
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
    }
}