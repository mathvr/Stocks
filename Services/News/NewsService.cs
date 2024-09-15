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

    public TServiceResponse<List<ArticleDto>> GetByStockOverview(string symbol, int fromDays)
    {
        try
        {
            var data = _articleRepository
                .GetAsQueryableAsNoTracking()
                .Include(a => a.StockOverview)
                .Where(a =>
                    a.StockOverview.Symbol == symbol &&
                    a.PublicationDate > DateTime.Now.AddDays(-1 * fromDays))
                .Select(a => _mapper.MapArticleToDto(a))
                .ToList();

            return new TServiceResponse<List<ArticleDto>>
            {
                Data = data,
                WasSuccessfull = true
            };
        }
        catch (Exception e)
        {
            return new TServiceResponse<List<ArticleDto>>
            {
                Data = null,
                WasSuccessfull = false
            };
        }
        
    }

    public ServiceResponse AddArticles(int fromDays)
    {
        var isSuccess = true;
        var unsucessfullUpdateSymbols = new List<string>();
        var responseList = new List<Task<StockOverviewandResponse>>();
        
        GetStockOvervies()
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

    private List<StockOverview> GetStockOvervies()
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
            foreach (var article in articles.Where(a => !existingArticleNames.Contains(a.Title)))
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
}