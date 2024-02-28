using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using STOCKS.Clients;
using STOCKS.Models;
using stocks.Services;
using STOCKS.Services.StockOverviews;

namespace STOCKS.Controllers;

[ApiController]
[Route("StocksOverview")]
public class StocksOverviewController : ControllerBase
{
    private readonly IStocksHttpClient _stocksHttpClient;
    private readonly IStocksOverviewService _stocksOverviewService;
    private readonly ITimeSeriesService _timeSeriesService;

    public StocksOverviewController(IStocksHttpClient stocksHttpClient, IStocksOverviewService stocksOverviewService, ITimeSeriesService timeSeriesService)
    {
        _stocksHttpClient = stocksHttpClient;
        _stocksOverviewService = stocksOverviewService;
        _timeSeriesService = timeSeriesService;
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
        var overviews = _stocksOverviewService.GetExistingStockOverviews(query, top);

        if (overviews.Any())
        {
            return Ok(overviews);
        }

        return NotFound();
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
    [Route("DownloadTimeSeries")]
    public ActionResult<string> DownloadTimeSeries()
    {
        var response = _timeSeriesService.SaveAllSeries(60);

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
}
