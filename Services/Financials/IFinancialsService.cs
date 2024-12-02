using STOCKS.Models;
using STOCKS.Models.Financials;

namespace stocks.Services.Financials;

public interface IFinancialsService
{
    ServiceResponse CreateForAllSymbols(string? forYear = null);
    TServiceResponse<List<FinancialDto>> GetBySymbol(string symbol);
}