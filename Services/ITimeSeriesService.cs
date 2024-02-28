using STOCKS.Models;

namespace stocks.Services;

public interface ITimeSeriesService
{
    TServiceResponse<TimeSerieApiModel> GetTimeSerie(string symbol, int? intervalMinutes);
    ServiceResponse SaveAllSeries(int? intervalMinutes);
}