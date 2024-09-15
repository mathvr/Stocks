using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using stocks.Data.Entities;
using STOCKS.Models;
using Article = stocks.Data.Entities.Article;

namespace STOCKS.Mappers
{
    public class StocksMapper : IStocksMapper
    {
        public StockOverview StockOverViewApiToEntity(StockOverviewApiModel apiModel, bool asUpdate)
        {
            return new StockOverview
            {
                Name = apiModel.Name,
                Exchange = apiModel.Exchange,
                Description = apiModel.Description, 
                Symbol = apiModel.Symbol,
                CreatedOn = asUpdate ? null : DateTimeOffset.Now,
                ModifiedOn = asUpdate ? DateTimeOffset.Now : null
            };
        }
        
        public void StockOverViewApiToEntityUpdate(StockOverviewApiModel apiModel, StockOverview stockoverview)
        {
            stockoverview.Name = apiModel.Name;
            stockoverview.Exchange = apiModel.Exchange;
            stockoverview.Description = apiModel.Description;
            stockoverview.Symbol = apiModel.Symbol;
            stockoverview.ModifiedOn = DateTimeOffset.Now;
        }
        
        public StockOverview StockOverViewApiToEntity(StockOverviewApiModel apiModel)
        {
            return new StockOverview
            {
                Name = apiModel.Name,
                Exchange = apiModel.Exchange,
                Description = apiModel.Description,
                Symbol = apiModel.Symbol,
                ModifiedOn = DateTimeOffset.Now
            };
        }

        public StockOverviewDto MapStockOverviewToDto(StockOverview stockOverview)
        {
            return new StockOverviewDto
            {
                Name = stockOverview.Name,
                Exchange = stockOverview.Exchange,
                Symbol = stockOverview.Symbol,
                Description = stockOverview.Description, 
                StartDate = stockOverview.StartDate,
                EndDate = stockOverview.EndDate
            };
        }

        public StockHistory MapTimeSerieToEntity(TimeSerieApiModel model, StockOverview stockOverview)
        {
            return new StockHistory
            {
                CloseValue = model.CloseValue,
                OpenValue = model.OpenValue,
                HighValue = model.High,
                LowValue = model.Low,
                Volume = model.Volume,
                Date = model.Date,
                Id = Guid.NewGuid(),
                CreatedOn = DateTimeOffset.Now,
                StockOverviewId = stockOverview?.Id
            };
        }

        public Article MapArticleToEntity(ArticleApiModel articleApiModel, StockOverview stockOverview)
        {
            return new Article
            {
                Title = articleApiModel.Title ?? string.Empty,
                Author = articleApiModel.Author ?? string.Empty,
                Content = articleApiModel.Content ?? string.Empty,
                CreatedOn = DateTimeOffset.Now,
                Description = articleApiModel.Description ?? string.Empty,
                Id = Guid.NewGuid(),
                PublicationDate = articleApiModel.PublishDate,
                StockOverviewId = stockOverview.Id,
                ModifiedOn = null,
                SourceName = articleApiModel.Source.Name ?? string.Empty,
                Url = articleApiModel.Url ?? string.Empty,
                StockOverview = stockOverview
            };
        }

        public ArticleDto MapArticleToDto(Article article)
        {
            return new ArticleDto
            {
                Title = article.Title ?? string.Empty,
                Author = article.Author ?? string.Empty,
                Content = article.Content ?? string.Empty,
                Id = article.Id,
                PublicationDate = article.PublicationDate,
                SourceName = article.SourceName ?? string.Empty,
                StockSymbol = article.StockOverview?.Symbol ?? string.Empty,
                Url = article.Url ?? string.Empty,
            };
        }

        public Split MapSplitToEntity(PolygonSplitApiModel.Split splitApiModel)
        {
            return new Split
            {
                ExecutionDate = splitApiModel.ExecutionDate,
                InitialValue = splitApiModel.SplitFrom,
                NewValue = splitApiModel.SplitTo,
                SplitApiId = splitApiModel.SplitApiId,
                Symbol = splitApiModel.Ticker,
            };
        }
    }
}