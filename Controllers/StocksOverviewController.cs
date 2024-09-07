using Microsoft.AspNetCore.Mvc;
using STOCKS.Clients;
using stocks.Data.Entities;
using STOCKS.Models;
using stocks.Services.Computation;
using stocks.Services.News;
using STOCKS.Services.StockOverviews;
using stocks.Services.TimeSeries;

namespace STOCKS.Controllers;

[ApiController]
[Route("StocksOverview")]
public class StocksOverviewController : ControllerBase
{
    private readonly INewsService _newsService;
    private readonly IStocksHttpClient _stocksHttpClient;
    private readonly IStocksOverviewService _stocksOverviewService;
    private readonly ITimeSeriesService _timeSeriesService;
    private readonly IComputationService _computationService;

    public StocksOverviewController(INewsService newsService, IStocksHttpClient stocksHttpClient, IStocksOverviewService stocksOverviewService, ITimeSeriesService timeSeriesService, IComputationService computationService)
    {
        _newsService = newsService;
        _stocksHttpClient = stocksHttpClient;
        _stocksOverviewService = stocksOverviewService;
        _timeSeriesService = timeSeriesService;
        _computationService = computationService;
    }

    [HttpGet]
    [Route("GetCompanyOverview/companySymbol={companySymbol}")]
    public ActionResult<StockOverviewApiModel> Get(string companySymbol)
    {
        if(companySymbol != null)
        {
            return _stocksHttpClient.GetCompanyOverview(companySymbol);
        }
        
        return BadRequest("No company symbol was provided");
    }
    
    [HttpGet]
    [Route("GetCompanyOverviews/query={query}&top={top}")]
    public ActionResult<List<StockOverviewDto>> GetStockOverviews(string query, int top)
    {
        var response = _stocksOverviewService.GetExistingStockOverviews(query, top);

        if (response.WasSuccessfull)
        {
            return response.Data.Any()
                ? Ok(response.Data)
                : NotFound(response.Message);
        }

        return BadRequest(response.Message);
    }

    [HttpGet]
    [Route("Compute/PriceDifference/SinceDays={days}")]
    public ActionResult<StockProgressionModels> GetPriceDifferencesSince(int days)
    {
        if (days < 1)
        {
            return BadRequest("Days must be greater than 0");
        }
        
        var startDate = DateTime.Now.AddDays(-days);
        var endDate = DateTime.Now;
        var response = _computationService.GetByMostProgression(startDate, endDate);

        return Ok(response);
    }

    [HttpPost]
    [Route("AddOverview/symbol={symbol}")]
    public ActionResult AddOverview(string symbol)
    {
        var response = _stocksOverviewService.AddCompanyToSite(symbol);

        if (!response.WasSuccessfull)
        {
            return BadRequest(response.Message);
        }

        return Ok(response.Message);
    }
    
    [HttpGet]
    [Route("GetCompanyOverviews/UserProfileEmail={email}")]
    public ActionResult<List<StockOverview>> GetUserStocks(string email)
    {
        var overviews = _stocksOverviewService.GetUserStockOverviews(email);

        return overviews.Any()
            ? Ok(overviews)
              : NotFound();
    }

    [HttpPost]
    [Route("DownloadTimeSeries/FromDaysAgo={daysAgo}")]
    public ActionResult<string> DownloadTimeSeries(int daysAgo)
    {
        var response = _timeSeriesService.SaveAllSeries(DateTime.Now.AddDays(-1 * daysAgo));
        
        if (response.WasSuccessfull)
        {
            return Ok();
        }

        return BadRequest(response.Message);
    }

    [HttpPost]
    [Route("AddCompanyToUserProfile")]
    public ActionResult<string> AddCompanyToUserProfile([FromBody] AddCompanyApiModel model)
    {
        var result = _stocksOverviewService.AddCompanyToUserProfile(model.CompanySymbol);

        if(result.WasSuccessfull)
        {
            return Ok(result.Message);
        }

        return BadRequest(result.Message);
    }
    
    [HttpPost]
    [Route("AddCompanyOverview/companySymbol={companySymbol}")]
    public ActionResult<string> AddCompanyToSite(string companySymbol)
    {
        var result = _stocksOverviewService.AddCompanyToSite(companySymbol);

        if(result.WasSuccessfull)
        {
            return Ok(result.Message);
        }

        return BadRequest(result.Message);
    }

    [HttpPatch]
    [Route("UpdateStockOverviews")]
    public IActionResult UpdateStockOverviews()
    {
        var result = _stocksOverviewService.UpdateStockOverviews();

        if(result.WasSuccessfull)
        {
            return Ok(result.Message);
        }

        return BadRequest(result.Message);
    }

    [HttpPost]
    [Route("DownloadNews/fromDaysAgo={daysAgo}")]
    public IActionResult DownloadNews(int daysAgo)
    {
        if (daysAgo < 1)
        {
            return BadRequest("Days must be greater than 0");
        }
        
        var result = _newsService.AddArticles(daysAgo);

        if (result.WasSuccessfull)
        {
            return Ok(result.Message);
        }
        
        return BadRequest(result.Message);
    }

    [HttpGet]
    [Route("GetNews/symbol={symbol}&fromDays={fromDays}")]
    public ActionResult<List<ArticleDto>> GetNews(string symbol, int fromDays)
    {
        if (fromDays < 1)
        {
            return BadRequest("From days must be greater than 0");
        }

        if (string.IsNullOrWhiteSpace(symbol))
        {
            return BadRequest("Symbol must be provided");
        }

        var serviceResponse = _newsService.GetByStockOverview(symbol, fromDays);

        if (serviceResponse.WasSuccessfull && serviceResponse.Data.Any())
        {
            return Ok(serviceResponse.Data);
        }
        else if (serviceResponse.WasSuccessfull && !serviceResponse.Data.Any())
        {
            return NotFound("No articles were found");
        }
        else if (!serviceResponse.WasSuccessfull)
        {
            return Conflict("An error occured while getting the data");
        }

        return BadRequest();
    }
}
