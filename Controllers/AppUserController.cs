using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using STOCKS.Models;
using stocks.Services.AppUsers;

namespace STOCKS.Controllers;

[ApiController]
[Route("AppUser")]
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

        if(!result.WasSuccessfull)
        {
            return BadRequest(result.Message);        
        }

        if (result.Data.Result.Errors.Any())
        {
            return BadRequest(string.Join(';', result.Data.Result.Errors.Select(e => e.Description)));
        }

        return Ok($"{model.UserNumber} Registered Successfully");
    }

    [HttpPost]
    [Route("Login")]
    public ActionResult<AppUserDto> Login(LoginApiModel loginApiModel)
    {
        if (string.IsNullOrEmpty(loginApiModel.email) || string.IsNullOrEmpty(loginApiModel.password))
        {
            return StatusCode(204);
        }

        var response = _appUserService.LoginUser(loginApiModel).Result;

        if (response.WasSuccessfull)
        {
            return Ok(response.Data.ToJson());
        }

        return BadRequest(response.Message);
    }

    [Authorize]
    [HttpGet]
    [Route("GetCurrentUser")]
    public ActionResult<AppUserDto> GetCurrentUser()
    {
        var userName = User.Identity;
        
        Console.WriteLine(User.Identity.Name);
        Console.WriteLine(User.Identity.IsAuthenticated);
        var response = _appUserService.GetCurrentUser(User?.Identity?.Name);

        if (response.Result.WasSuccessfull)
        {
            return Ok(response.Result.Data);
        }

        return BadRequest(response.Result.Message);
    }
}