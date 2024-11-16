using System.Net;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using stocks.Clients.News;
using stocks.Data.Entities;
using STOCKS.Data.Repository;
using STOCKS.Data.Repository.StockOverview;
using STOCKS.Mappers;
using STOCKS.Models;
using Article = stocks.Data.Entities.Article;

namespace stocks.Services.News;

public class NewsService : INewsService
{
    private readonly IRepository<Article> _articleRepository;
    private readonly IRepository<StockOverview> _stockOverviewRepository;
    private readonly INewsHttpClient _newsHttpClient;
    private readonly IStocksMapper _mapper;

    public NewsService(IRepository<Article> articleRepository, IRepository<StockOverview> stockOverviewRepository, 
        INewsHttpClient newsHttpClient, IStocksMapper mapper)
    {
        _articleRepository = articleRepository;
        _stockOverviewRepository = stockOverviewRepository;
        _newsHttpClient = newsHttpClient;
        _mapper = mapper;
    }

    public TServiceResponse<IList<ArticlesGroup>> GetByStockOverview(string symbol, int fromDays, int perSymbol)
    {
        try
        {
            var data = _articleRepository
                .GetAsQueryableAsNoTracking()
                .Include(a => a.StockOverview)
                .Where(a =>
                    a.StockOverview.Symbol == symbol &&
                    a.PublicationDate > DateTime.Now.AddDays(-1 * fromDays))
                .ToList()
                .Select(a => _mapper.MapArticleToDto(a))
                .GroupBy(a => a.SourceName)
                .Select(group => new ArticlesGroup
                {
                    GroupName = group.Key,
                    Articles = group
                        .OrderByDescending(a => a.PublicationDate)
                        .Take(perSymbol)
                        .ToList()
                })
                .ToList();

            return new TServiceResponse<IList<ArticlesGroup>> 
            {
                Data = data,
                WasSuccessfull = true
            };
        }
        catch (Exception e)
        {
            return new TServiceResponse<IList<ArticlesGroup>> 
            {
                Data = new System.Collections.Generic.List<ArticlesGroup>(),
                WasSuccessfull = false,
                Message = e.Message
            };
        }
        
    }

    public ServiceResponse AddArticles(int fromDays)
    {
        var isSuccess = true;
        var unsucessfullUpdateSymbols = new List<string>();
        var responseList = new List<Task<StockOverviewandResponse>>();
        
        GetStockOverviews()
            .AsParallel()
            .ForEach(stock =>
            {
                try
                {
                    Console.WriteLine($"News :: NewsService - Getting News for symbol: {stock.Symbol}");
                    var response = _newsHttpClient.GetNewsForTitleAsync(fromDays, stock);
                    
                    responseList.Add(response);
                }
                catch (Exception e)
                {
                    isSuccess = false;
                    unsucessfullUpdateSymbols.Add(stock.Symbol); 
                }
            });

        responseList
            .ForEach(response =>
            {
                var data = response.Result;
                var result = data.Response?.Content?.ReadAsStringAsync().Result;

                if (!string.IsNullOrEmpty(result))
                {
                    var newsModel = JsonConvert.DeserializeObject<NewsApiModel>(result);
                    SaveNews(newsModel?.Articles, data.StockOverview);
                }
                else
                {
                    isSuccess = false;
                }
            });
        
        if (isSuccess)
        {
            return new ServiceResponse
            {
                WasSuccessfull = true,
                Message = "News successfully added",
            };
        }

        return new ServiceResponse
        {
            WasSuccessfull = false,
            Message = $"News inserts failed for symbols : [{string.Join(", ", unsucessfullUpdateSymbols.Distinct())}]",
        };
    }

    public ServiceResponse DeleteAllArticles()
    {
        var query = "DELETE FROM ARTICLES";

        try
        {
            _articleRepository.SqlQuery(query);
            _articleRepository.Save();

            return new ServiceResponse
            {
                WasSuccessfull = true,
                Message = "News successfully deleted",
            };
        }
        catch (Exception e)
        {
            Console.Error.WriteLine(e.Message);
            return new ServiceResponse
            {
                WasSuccessfull = false,
                Message = "An error occured while trying to delete the Articles"
            };
        }
    }

    private List<StockOverview> GetStockOverviews()
    {
        return _stockOverviewRepository
            .GetAsQueryable()
            .ToList();
    }

    private bool SaveNews(List<ArticleApiModel> articles, StockOverview stockOverview)
    {
        if (articles == null || !articles.Any())
        {
            return false;
        }

        try
        {
            var existingArticleNames = GetExistingArticleNames();
            foreach (var article in articles
                         .Where(a => !existingArticleNames.Contains(a.Title)
                         && GetNewsProviders().Contains(a?.Source?.Name)))
            {
                var entity = _mapper.MapArticleToEntity(article, stockOverview);
                _articleRepository.Add(entity);
            }
            
            _articleRepository.Save();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }

    private List<string> GetExistingArticleNames()
    {
        return _articleRepository
            .GetAsQueryableAsNoTracking()
            .Select(a => a.Title)
            .ToList();
    }

    private string[] GetNewsProviders()
    {
        return
        [
            "BBC News", "Business Insider", "CNET", "Forbes", "CNN", "CNBC", "HuffPost", "Investing.com",
            "Investor's Business Daily", "Le Monde", "Yahoo Entertainment"
        ];
    }
}