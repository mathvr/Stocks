using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using STOCKS.Models;
using stocks.Services.AppUsers;

namespace STOCKS.Controllers;

public class AppUserController : ControllerBase
{
    private readonly IAppUserService _appUserService;

    public AppUserController(IAppUserService appUserService)
    {
        _appUserService = appUserService;
    }
    
    [HttpPost]
    [Route("AddAppUser")]
    public ActionResult<string> AddAppUser([FromBody] AddAppUserApiModel model)
    {
        var result = _appUserService.AddAppUser(model);

        if(result.WasSuccessfull)
        {
            return Ok(result.Message);
        }

        return BadRequest(result.Message);
    }
}