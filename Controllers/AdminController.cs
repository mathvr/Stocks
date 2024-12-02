using Microsoft.AspNetCore.Mvc;
using STOCKS.Models.Connection;
using stocks.Services.Admin;

namespace STOCKS.Controllers;

[ApiController]
[Route("Admin")]
public class AdminController : ControllerBase
{
    private readonly IConnectionService _connectionService;

    public AdminController(IConnectionService connectionService)
    {
        _connectionService = connectionService;
    }
    
    [HttpPost]
    [Route("Connection/Create")]
    public ActionResult<string> AddAppUser([FromBody] CreateConnectionModel model)
    {
        var serviceResponse = _connectionService.CreateConnection(model);
        
        return serviceResponse.WasSuccessfull
            ? Ok(serviceResponse.Message)
            : BadRequest(serviceResponse.Message);
    }
}