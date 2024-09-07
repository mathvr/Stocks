using STOCKS.Models;

namespace stocks.Services.TimeSeries;

public interface ITimeSeriesService
{
    ServiceResponse SaveAllSeries(DateTime fromDate);
}