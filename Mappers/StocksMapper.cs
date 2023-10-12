using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using STOCKS.Models;

namespace STOCKS.Mappers
{
    public class StocksMapper : IStocksMapper
    {
        public StockOverview StockOverViewApiToEntity(StockOverviewApiModel apiModel, bool asUpdate)
        {
            return new StockOverview
            {
                Name = apiModel.Name,
                Cik = apiModel.Cik,
                Industry = apiModel.Industry,
                Exchange = apiModel.Exchange,
                Description = apiModel.Description, 
                Currency = apiModel.Currency, 
                Country = apiModel.Country,
                Sector = apiModel.Sector,
                Symbol = apiModel.Symbol,
                FiscalYearEnd = apiModel.Fiscalyearend, 
                MarketCapitalization = apiModel.Marketcapitalization,
                BookValue = apiModel.Bookvalue,
                ProfitMargin = apiModel.Profitmargin,
                LatestQuarter = apiModel.Latestquarted,
                CreatedBy = asUpdate ? string.Empty : "Admin",
                CreatedOn = asUpdate ? null : DateTimeOffset.Now,
                ModifiedBy = asUpdate ? "Admin" : string.Empty ,
                ModifiedOn = asUpdate ? DateTimeOffset.Now : null
            };
        }
        
        public void StockOverViewApiToEntityUpdate(StockOverviewApiModel apiModel, StockOverview stockoverview)
        {
            stockoverview.Name = apiModel.Name;
            stockoverview.Cik = apiModel.Cik;
            stockoverview.Industry = apiModel.Industry;
            stockoverview.Exchange = apiModel.Exchange;
            stockoverview.Description = apiModel.Description;
            stockoverview.Currency = apiModel.Currency;
            stockoverview.Country = apiModel.Country;
            stockoverview.Sector = apiModel.Sector;
            stockoverview.Symbol = apiModel.Symbol;
            stockoverview.FiscalYearEnd = apiModel.Fiscalyearend;
            stockoverview.MarketCapitalization = apiModel.Marketcapitalization;
            stockoverview.BookValue = apiModel.Bookvalue;
            stockoverview.ProfitMargin = apiModel.Profitmargin;
            stockoverview.LatestQuarter = apiModel.Latestquarted;
            stockoverview.ModifiedBy = "Admin";
            stockoverview.ModifiedOn = DateTimeOffset.Now;
        }
    }
}