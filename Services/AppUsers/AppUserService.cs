using Microsoft.AspNetCore.Identity;
using STOCKS;
using STOCKS.Data.Repository.AppUser;
using STOCKS.Models;
using stocks.Services.Session;

namespace stocks.Services.AppUsers;

public class AppUserService : IAppUserService
{
    private readonly IAppUserRepository _appUserRepository;
    private readonly UserManager<Appuser> _userManager;
    private readonly ITokenService _tokenService;

    public AppUserService(IAppUserRepository appUserRepository, UserManager<Appuser> userManager, ITokenService tokenService)
    {
        _appUserRepository = appUserRepository;
        _userManager = userManager;
        _tokenService = tokenService;
    }

    public TServiceResponse<Task<IdentityResult>> AddAppUser(AddAppUserApiModel model)
    {
        if (string.IsNullOrEmpty(model.UserNumber)|| string.IsNullOrEmpty(model.Password) ||
            string.IsNullOrEmpty(model.AccessLevel))
        {
            return new TServiceResponse<Task<IdentityResult>>
            {
                WasSuccessfull = false,
                Message = $"Some of the provided required information was null"
            };
        }

        var activeUser = _appUserRepository
            .GetAsQueryableAsNoTracking()
            .FirstOrDefault(u => u.Email.Equals(model.UserNumber));

        if (activeUser != null)
        {
            return new TServiceResponse<Task<IdentityResult>>
            {
                WasSuccessfull = false,
                Message = $"User {model.UserNumber} already exists, please use the Login form instead"
            };
        }

        var result = _userManager.CreateAsync(new Appuser
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
            AccessLevel = model.AccessLevel,
            CreatedBy = "admin",
            CreatedOn = DateTimeOffset.Now,
            Email = model.UserNumber,
            Password = EncryptionService.GetEncrypted(model.Password),
            UserName = model.UserNumber
        });

        Task.WaitAll(result);

        var user = _appUserRepository
            .GetAsQueryableAsNoTracking()
            .FirstOrDefault(u => u.UserName.Equals(model.UserNumber));

        if (user != null)
        {
            _userManager.AddToRoleAsync(user, model.AccessLevel);
        }
        
        
        return new TServiceResponse<Task<IdentityResult>>
        {
            WasSuccessfull = true,
            Message = $"User {model.UserNumber} added Successfully!",
            Data = result
        };
    }

    public async Task<TServiceResponse<AppUserDto>> LoginUser(LoginApiModel loginApiModel)
    {
        var correspondingUser = await _userManager
            .FindByNameAsync(loginApiModel.email);
        
        if (correspondingUser == null || EncryptionService.GetDecrypted(correspondingUser.Password) != loginApiModel.password)
        {
            return new TServiceResponse<AppUserDto>
            {
                WasSuccessfull = false,
                Message = "Wrong password, or Unregistered user!"
            };
        }

        return new TServiceResponse<AppUserDto>
        {
            WasSuccessfull = true,
            Data = new AppUserDto
            {
                email = correspondingUser.UserName ?? correspondingUser.Email,
                Token = await _tokenService.GenerateToken(correspondingUser)
            }
        };
    }

    public async Task<TServiceResponse<AppUserDto>> GetCurrentUser(string userName)
    {
        var user = await _userManager.FindByNameAsync(userName);

        if (user == null)
        {
            return new TServiceResponse<AppUserDto>
            {
                WasSuccessfull = false,
                Message = "Could not find current user, are you logged in?"
            };
        }

        return new TServiceResponse<AppUserDto>
        {
            WasSuccessfull = true,
            Data = new AppUserDto
            {
                email = user.Email,
                Token = await _tokenService.GenerateToken(user)
            }
        };
    }
}