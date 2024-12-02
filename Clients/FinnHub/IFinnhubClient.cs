namespace stocks.Clients.FinnHub;

public interface IFinnhubClient
{
    Task<HttpResponseMessage>? GetFinancialsBySymbol(string symbol, string? forYear = null);
}