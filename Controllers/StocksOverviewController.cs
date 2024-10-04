using Microsoft.AspNetCore.Mvc;
using STOCKS.Clients;
using stocks.Clients.OpenAi;
using stocks.Data.Entities;
using STOCKS.Models;
using STOCKS.Models.ApiModels.OpenAi;
using stocks.Services.Reputation;
using STOCKS.Services.StockOverviews;

namespace STOCKS.Controllers;

[ApiController]
[Route("StocksOverview")]
public class StocksOverviewController : ControllerBase
{
    private readonly IStocksHttpClient _stocksHttpClient;
    private readonly IStocksOverviewService _stocksOverviewService;
    private readonly IOpenAiClient _openAiClient;
    private readonly IReputationService _reputationService;

    public StocksOverviewController(IStocksHttpClient stocksHttpClient, IStocksOverviewService stocksOverviewService, IOpenAiClient openAiClient, IReputationService reputationService)
    {
        _stocksHttpClient = stocksHttpClient;
        _stocksOverviewService = stocksOverviewService;
        _openAiClient = openAiClient;
        _reputationService = reputationService;
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
    [Route("Reputation/GetSocialReputation/symbol={symbol}")]
    public ActionResult<GetReputationApiModel> GetReputation(string symbol)
    {
        var list = new List<string>();
        list.Add(symbol);
        return Ok(_openAiClient.GetReputations(list));
    }
    
    [HttpGet]
    [Route("Reputation/DownloadValues")]
    public ActionResult DownloadReputations()
    {
        var serviceResponse = _reputationService.DownloadReputations();

        if (serviceResponse.WasSuccessfull)
        {
            return Ok(serviceResponse.Message);
        }
        
        return StatusCode(500, serviceResponse.Message);
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
