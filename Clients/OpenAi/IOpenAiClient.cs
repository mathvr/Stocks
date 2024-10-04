using STOCKS;
using STOCKS.Models.ApiModels.OpenAi;

namespace stocks.Clients.OpenAi;

public interface IOpenAiClient
{
    Task<HttpResponseMessage> GetSocialReputation(string symbol, Connection connection);
    List<GetReputationApiModel> GetReputations(List<string> symbols);
}