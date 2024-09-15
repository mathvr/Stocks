using Microsoft.AspNetCore.Mvc;
using STOCKS.Models;
using stocks.Services.News;

namespace STOCKS.Controllers;

[ApiController]
[Route("News")]
public class NewsController : ControllerBase
{
    private readonly INewsService _newsService;

    public NewsController(INewsService newsService)
    {
        _newsService = newsService;
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