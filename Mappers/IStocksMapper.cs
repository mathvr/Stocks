using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using STOCKS.Models;

namespace STOCKS.Mappers
{
    public interface IStocksMapper
    {
        StockOverview StockOverViewApiToEntity(StockOverviewApiModel apiModel, bool asUpdate);
        void StockOverViewApiToEntityUpdate(StockOverviewApiModel apiModel, StockOverview stockoverview);
    }
}