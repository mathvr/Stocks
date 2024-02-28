using STOCKS;

namespace stocks.Services.Session;

public interface ITokenService
{
    Task<string> GenerateToken(Appuser appuser);
}