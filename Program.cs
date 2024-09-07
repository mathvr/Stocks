using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using STOCKS;
using STOCKS.Clients;
using stocks.Clients.News;
using stocks.Clients.Stocks;
using stocks.Data.DbContext;
using stocks.Data.Entities;
using STOCKS.Data.Repository;
using STOCKS.Data.Repository.AppUser;
using STOCKS.Data.Repository.Connection;
using STOCKS.Data.Repository.News;
using STOCKS.Data.Repository.StockHistory;
using STOCKS.Data.Repository.StockOverview;
using STOCKS.Mappers;
using stocks.Services.AppUsers;
using stocks.Services.Computation;
using stocks.Services.News;
using stocks.Services.Session;
using STOCKS.Services.StockOverviews;
using stocks.Services.TimeSeries;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddIdentityCore<Appuser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<StocksContext>();

var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.Development.json", false, false)
    .Build();


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWTSettings:TokenKey"]))
        };
    });
    
builder.Services.AddAuthorization();

builder.Services.AddDbContext<StocksContext>(
    options => options.UseSqlServer(config["Database:ConnectionString"]));

// Add services to the container.

builder.Services.AddScoped<IAppUserRepository, AppUserRepository>();
builder.Services.AddScoped<IConnectionRepository, ConnectionRepository>();
builder.Services.AddScoped<IStockOverviewRepository, StockOverviewRepository>();
builder.Services.AddScoped<IStockHistoryRepository, StockHistoryRepository>();
builder.Services.AddScoped<IStocksHttpClient, StocksHttpClient>();
builder.Services.AddScoped<IStocksMapper, StocksMapper>();
builder.Services.AddScoped<IStocksOverviewService, StockOverviewService>();
builder.Services.AddScoped<IAppUserService, AppUserService>();
builder.Services.AddScoped<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<ITimeSeriesService, TimeSeriesService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IConfigurationHelper, ConfigurationHelper>();
builder.Services.AddScoped<IComputationService, ComputationService>();
builder.Services.AddScoped<INewsHttpClient, NewsHttpClient>();
builder.Services.AddScoped<IRepository<Article>, NewsRepository>();
builder.Services.AddScoped<INewsService, NewsService>();
builder.Services.AddCors();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var scope = app.Services.CreateScope();
var userManager = scope.ServiceProvider.GetRequiredService<UserManager<Appuser>>();

app.UseHttpsRedirection();

app.UseCors(opt =>
{
    opt.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:3000");
});

app.UseAuthorization();

app.MapControllers();

app.Run();

