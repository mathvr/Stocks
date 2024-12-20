using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using stocks.Data.Entities;
using STOCKS.Models;
using STOCKS.Models.ApiModels.OpenAi;
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
                EndDate = stockOverview.EndDate,
                ReputationValue = stockOverview.Reputation?.ReputationValue,
                ReputationFacts = stockOverview.Reputation?.ReputationFacts?.Select(f => f.Fact).ToList()
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
                PublicationDateText = article.PublicationDate.Date.ToString("dd/MM/yyyy"),
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

        public Reputation MapReputationToEntity(CompanyInfo reputationModel, StockOverview stockOverview)
        {
            return new Reputation
            {
                StockOverviewId = stockOverview.Id,
                Symbol = reputationModel.Symbol,
                ReputationValue = reputationModel.Reputation,
                ModifiedOn = DateTimeOffset.Now
            };
        }

        public List<ReputationFact> MapReputationFacts(CompanyInfo reputationModel, Reputation reputation)
        {
            return reputationModel.Facts
                .Select(fact => new ReputationFact
                {
                    Fact = fact,
                    Reputation = reputation,
                    CreatedOn = DateTimeOffset.Now,
                    ModifiedOn = DateTimeOffset.Now
                })
                .ToList();
        }

        public IEnumerable<Financials> MapToFinancials(GetFinancialsApiModel financialsApiModel)
        {
            var list = new List<Financials>();

            foreach (var report in financialsApiModel.Data.SelectMany(s => s.Report.BalanceSheet))
            {
                list.Add(new Financials
                {
                    Id = Guid.NewGuid(),
                    Symbol = financialsApiModel.Data.FirstOrDefault()?.Symbol,
                    Year = financialsApiModel.Data.FirstOrDefault()?.Year.ToString(),
                    FromDate = DateTimeOffset.Parse(financialsApiModel.Data.FirstOrDefault()?.StartDate),
                    ToDate = DateTimeOffset.Parse(financialsApiModel.Data.FirstOrDefault()?.EndDate),
                    Property = report.Concept,
                    Value = report.Value,
                    PropertyName = report.Label,
                    Unit = report.Unit,
                    CreatedOn = DateTimeOffset.Now,
                });
            }
            
            foreach (var report in financialsApiModel.Data.SelectMany(s => s.Report.CashFlow))
            {
                list.Add(new Financials
                {
                    Id = Guid.NewGuid(),
                    Symbol = financialsApiModel.Data.FirstOrDefault()?.Symbol,
                    Year = financialsApiModel.Data.FirstOrDefault()?.Year.ToString(),
                    FromDate = DateTimeOffset.Parse(financialsApiModel.Data.FirstOrDefault()?.StartDate),
                    ToDate = DateTimeOffset.Parse(financialsApiModel.Data.FirstOrDefault()?.EndDate),
                    Property = report.Concept,
                    Value = report.Value,
                    PropertyName = report.Label,
                    Unit = report.Unit,
                    CreatedOn = DateTimeOffset.Now,
                });
            }
            
            foreach (var report in financialsApiModel.Data.SelectMany(s => s.Report.IncomeStatement))
            {
                list.Add(new Financials
                {
                    Id = Guid.NewGuid(),
                    Symbol = financialsApiModel.Data.FirstOrDefault()?.Symbol,
                    Year = financialsApiModel.Data.FirstOrDefault()?.Year.ToString(),
                    FromDate = DateTimeOffset.Parse(financialsApiModel.Data.FirstOrDefault()?.StartDate),
                    ToDate = DateTimeOffset.Parse(financialsApiModel.Data.FirstOrDefault()?.EndDate),
                    Property = report.Concept,
                    Value = report.Value,
                    PropertyName = report.Label,
                    Unit = report.Unit,
                    CreatedOn = DateTimeOffset.Now,
                });
            }

            return list;
        }
    }
}