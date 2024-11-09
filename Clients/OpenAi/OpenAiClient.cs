using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using STOCKS;
using stocks.Data;
using STOCKS.Data.Repository.Connection;
using STOCKS.Models.ApiModels.OpenAi;

namespace stocks.Clients.OpenAi;

public class OpenAiClient : IOpenAiClient
{
    private readonly IConnectionRepository _connectionRepository;

    public OpenAiClient(IConnectionRepository connectionRepository)
    {
        _connectionRepository = connectionRepository;
    }

    public async Task<HttpResponseMessage> GetSocialReputation(string symbol, Connection connection)
    {
        using var client = new HttpClient
        {
            BaseAddress = new Uri(connection.BaseUrl),
            Timeout = TimeSpan.FromSeconds(30),
            DefaultRequestHeaders =
            {
                Authorization = new AuthenticationHeaderValue("Bearer", DecodeBase64(connection?.ClientSecret))}
        };

        var query = $"/v1/chat/completions";
        var body = new StringContent(GetBody(symbol), Encoding.UTF8, "application/json");

        return await client.PostAsync(query, body);
    }

    public List<GetReputationApiModel> GetReputations(List<string> symbols)
    {
        var connection = GetOpenAiConnection();

        return symbols
            .AsParallel()
            .Select(s => GetModel(s, connection))
            .ToList();
    }

    private Connection GetOpenAiConnection()
    {
        return _connectionRepository
            .GetByName("OpenAi");
    }

    private GetReputationApiModel GetModel(string symbol, Connection connection)
    {
        var asJson = GetSocialReputation(symbol, connection).Result?.Content?.ReadAsStringAsync()?.Result;

        if (string.IsNullOrEmpty(asJson))
        {
            return null;
        }
                
        var model = JsonConvert.DeserializeObject<GetReputationApiModel>(asJson);

        foreach (var choice in model.Choices)
        {
            var startIndex = choice.Message.Content.IndexOf("{");
            var lastIndex = choice.Message.Content.LastIndexOf("}");
            var length = lastIndex - startIndex + 1;

            var jsonData = choice.Message.Content.Substring(startIndex, length);
            choice.Message.CompanyReputation = JsonConvert.DeserializeObject<CompanyInfo>(jsonData);
        }

        return model;
    }

    private string GetBody(string symbol)
    {
        var model =  new GetReputationBodyModel
        {
            Model = AppConstants.OpenAi.GptModel,
            Temperature = AppConstants.OpenAi.GptQuestionTemperature,
            Messages =
            [
                new GetReputationBodyModel.Message
                {
                    Role = AppConstants.OpenAi.UserRole,
                    Content = $"{AppConstants.OpenAi.RequestSocialRatingText}{symbol}"
                }
            ]
        };

        return JsonConvert.SerializeObject(model);
    }
    
    private string DecodeBase64(string encodedCs)
    {
        return Encoding.UTF8.GetString(Convert.FromBase64String(encodedCs)); 
    }
}