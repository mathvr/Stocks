using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using STOCKS.Clients;
using STOCKS.Models;
using STOCKS.Services.StockOverviews;

namespace STOCKS.Controllers;

[ApiController]
[Route("StocksOverview")]
public class StocksOverviewController : ControllerBase
{
    private readonly IStocksHttpClient _stocksHttpClient;
    private readonly IStocksOverviewService _stocksOverviewService;

    public StocksOverviewController(IStocksHttpClient stocksHttpClient, IStocksOverviewService stocksOverviewService)
    {
        _stocksHttpClient = stocksHttpClient;
        _stocksOverviewService = stocksOverviewService;
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

    [HttpPost]
    [Route("AddCompanyToUserProfile")]
    public ActionResult<string> AddCompanyToUserProfile([FromBody] AddCompanyApiModel model)
    {
        var result = _stocksOverviewService.AddCompanyToUserProfile(model.UserEmail, model.CompanySymbol);

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
