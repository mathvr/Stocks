using STOCKS;
using STOCKS.Data.Repository;
using STOCKS.Data.Repository.Connection;

namespace stocks.Clients.OpenAi;

public class OpenAiClient
{
    private readonly IConnectionRepository _connectionRepository;

    public OpenAiClient(IConnectionRepository connectionRepository)
    {
        _connectionRepository = connectionRepository;
    }

    private Connection GetOpenAiConnection()
    {
        return _connectionRepository
            .GetByName("OpenAi");
    }
}