using Microsoft.AspNetCore.Identity;
using STOCKS.Models;

namespace stocks.Services.AppUsers;

public interface IAppUserService
{
    TServiceResponse<Task<IdentityResult>> AddAppUser(AddAppUserApiModel model);
    Task<TServiceResponse<AppUserDto>> LoginUser(LoginApiModel loginApiModel);
    Task<TServiceResponse<AppUserDto>> GetCurrentUser(string userName);

}