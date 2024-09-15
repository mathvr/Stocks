using STOCKS.Models;

namespace stocks.Clients.PolygonIo;

public interface IPolygonClient
{
    PolygonSplitApiModel? GetPolygonSplitModel();
    PolygonSplitApiModel? GetPolygonSplitModelFromUrl(string url);
}