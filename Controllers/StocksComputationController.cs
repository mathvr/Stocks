using Microsoft.AspNetCore.Mvc;
using STOCKS.Models;
using stocks.Services.Computation;

namespace STOCKS.Controllers;

[ApiController]
[Route("Computation")]
public class StocksComputationController : ControllerBase
{
    private readonly IComputationService _computationService;

    public StocksComputationController(IComputationService computationService)
    {
        _computationService = computationService;
    }
    
    [HttpGet]
    [Route("PriceEvolution/SinceDays={days}")]
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
}