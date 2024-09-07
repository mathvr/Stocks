using STOCKS.Models;

namespace stocks.Services.Computation;

public interface IComputationService
{
    StockProgressionModels GetByMostProgression(DateTime startDate, DateTime endDate); 
}