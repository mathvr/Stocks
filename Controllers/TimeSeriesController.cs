using Microsoft.AspNetCore.Mvc;
using stocks.Services.Splits;
using stocks.Services.TimeSeries;

namespace STOCKS.Controllers;

[ApiController]
[Route("TimeSeries")]
public class TimeSeriesController : ControllerBase
{
    private readonly ITimeSeriesService _timeSeriesService;
    private readonly ISplitService _splitService;

    public TimeSeriesController(ITimeSeriesService timeSeriesService, ISplitService splitService)
    {
        _timeSeriesService = timeSeriesService;
        _splitService = splitService;
    }
    
    [HttpPost]
    [Route("DownloadValues/FromDaysAgo={daysAgo}")]
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
    [Route("DownloadSplits")]
    public IActionResult DownloadSplits()
    {
        var response = _splitService.AddSplits();

        if (response.WasSuccessfull)
        {
            return Ok(response.Message);
        }
        
        return BadRequest(response.Message);
    }
}