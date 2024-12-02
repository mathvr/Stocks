using STOCKS.Models;
using STOCKS.Models.Connection;

namespace stocks.Services.Admin;

public interface IConnectionService
{
    ServiceResponse CreateConnection(CreateConnectionModel model);
}