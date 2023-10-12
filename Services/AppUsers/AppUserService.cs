using STOCKS;
using STOCKS.Data.Repository;
using STOCKS.Data.Repository.AppUser;
using STOCKS.Models;

namespace stocks.Services.AppUsers;

public class AppUserService : IAppUserService
{
    private readonly IAppUserRepository _appUserRepository;

    public AppUserService(IAppUserRepository appUserRepository)
    {
        _appUserRepository = appUserRepository;
    }

    public ServiceResponse AddAppUser(AddAppUserApiModel model)
    {
        if (model == null || model.UserNumber == null || model.Password == null ||
            model.AccessLevel == null)
        {
            return new ServiceResponse
            {
                WasSuccessfull = false,
                Message = $"Some of the provided required information was null"
            };
        }
        
        _appUserRepository.Add(new Appuser
        {
            AccessLevel = model.AccessLevel,
            Email = model.UserNumber,
            FirstName = model.FirstName,
            LastName = model.LastName,
            Password = EncodePassword(model.Password),
        });

        return new ServiceResponse
        {
            WasSuccessfull = true,
            Message = $"User {model.UserNumber} added Successfully!"
        };
    }

    private string EncodePassword(string password)
    {
        return System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password));
    }
}