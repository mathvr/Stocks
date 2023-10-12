using STOCKS.Models;

namespace stocks.Services.AppUsers;

public interface IAppUserService
{
    ServiceResponse AddAppUser(AddAppUserApiModel model);
}