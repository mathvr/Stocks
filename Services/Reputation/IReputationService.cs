using STOCKS.Models;

namespace stocks.Services.Reputation;

public interface IReputationService
{
    ServiceResponse DownloadReputations();
}