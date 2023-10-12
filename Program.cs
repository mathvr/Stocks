using Microsoft.EntityFrameworkCore;
using STOCKS;
using STOCKS.Clients;
using STOCKS.Data.Repository.AppUser;
using STOCKS.Data.Repository.Connection;
using STOCKS.Data.Repository.StockHistory;
using STOCKS.Data.Repository.StockOverview;
using STOCKS.Mappers;
using stocks.Services.AppUsers;
using STOCKS.Services.StockOverviews;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<StocksContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("StocksDb")));

builder.Services.AddScoped<IAppUserRepository, AppUserRepository>();
builder.Services.AddScoped<IConnectionRepository, ConnectionRepository>();
builder.Services.AddScoped<IStockOverviewRepository, StockOverviewRepository>();
builder.Services.AddScoped<IStockHistoryRepository, StockHistoryRepository>();
builder.Services.AddScoped<IStocksHttpClient, StocksHttpClient>();
builder.Services.AddScoped<IStocksMapper, StocksMapper>();
builder.Services.AddScoped<IStocksOverviewService, StockOverviewService>();
builder.Services.AddScoped<IAppUserService, AppUserService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

