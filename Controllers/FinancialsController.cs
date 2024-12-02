using Microsoft.AspNetCore.Mvc;
using stocks.Services.Financials;

namespace STOCKS.Controllers;

[ApiController]
[Route("Financials")]
public class FinancialsController (IFinancialsService _financialsService) : ControllerBase
{
    [HttpPost]
    [Route("SaveAll")]
    public ActionResult<string> AddAppUser([FromQuery] string? date)
    {
        var serviceResponse = date != null ? _financialsService.CreateForAllSymbols(date) : _financialsService.CreateForAllSymbols();
        
        return serviceResponse.WasSuccessfull
            ? Ok(serviceResponse.Message)
            : BadRequest(serviceResponse.Message);
    }

    [HttpGet]
    [Route("GetBySymbol/{symbol}")]
    public ActionResult<string> GetBySymbol(string symbol)
    {
        var serviceResponse = _financialsService.GetBySymbol(symbol);
        
        return serviceResponse.WasSuccessfull
            ? Ok(serviceResponse.Data)
            : BadRequest(serviceResponse.Message);
    }
}