using STOCKS;
using STOCKS.Data.Repository.Connection;
using STOCKS.Models;
using STOCKS.Models.Connection;
using STOCKS.Models.Helpers;
using stocks.Services.Helpers;

namespace stocks.Services.Admin;

public class ConnectionService : IConnectionService
{
    private readonly IConnectionRepository _connectionRepository;

    public ConnectionService(IConnectionRepository connectionRepository)
    {
        _connectionRepository = connectionRepository;
    }

    public ServiceResponse CreateConnection(CreateConnectionModel model)
    {
        var validationResponse = ModelValidator.ValidateObject(model);

        if (!validationResponse.IsValid)
        {
            return new ServiceResponse
            {
                WasSuccessfull = false,
                Message = $"Property {validationResponse.PropertyName} failed to be validated."
            };
        }
        
        var connectionEntity = new Connection
        {
            Id = Guid.NewGuid(),
            Name = model.Name,
            BaseUrl = model.BaseUrl,
            ClientSecret = EncryptionService.GetEncrypted(model.ClientSecret),
            CreatedOn = DateTimeOffset.Now,
            CreatedBy = "admin",
        };
        
        _connectionRepository.Add(connectionEntity);

        return new ServiceResponse
        {
            WasSuccessfull = true,
            Message = "Connection Created Successfully."
        };
    }
}