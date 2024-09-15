using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using stocks.Data.Entities;
using STOCKS.Models;

namespace STOCKS.Mappers
{
    public interface IStocksMapper
    {
        StockOverview StockOverViewApiToEntity(StockOverviewApiModel apiModel, bool asUpdate);
        void StockOverViewApiToEntityUpdate(StockOverviewApiModel apiModel, StockOverview stockoverview);
        StockOverviewDto MapStockOverviewToDto(StockOverview stockOverview);
        StockOverview StockOverViewApiToEntity(StockOverviewApiModel apiModel);
        StockHistory MapTimeSerieToEntity(TimeSerieApiModel model, StockOverview stockOverview);
        Article MapArticleToEntity(ArticleApiModel articleApiModel, StockOverview stockOverview);
        ArticleDto MapArticleToDto(Article article);
        Split MapSplitToEntity(PolygonSplitApiModel.Split splitApiModel);
    }
}