using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using STOCKS;
using STOCKS.Clients;
using stocks.Clients.FinnHub;
using stocks.Clients.News;
using stocks.Clients.OpenAi;
using stocks.Clients.PolygonIo;
using stocks.Clients.Stocks;
using stocks.Data.DbContext;
using stocks.Data.Entities;
using STOCKS.Data.Repository;
using STOCKS.Data.Repository.AppUser;
using STOCKS.Data.Repository.Connection;
using STOCKS.Data.Repository.FInancials;
using STOCKS.Data.Repository.News;
using STOCKS.Data.Repository.Reputation;
using STOCKS.Data.Repository.Splits;
using STOCKS.Data.Repository.StockHistory;
using STOCKS.Data.Repository.StockOverview;
using STOCKS.Mappers;
using stocks.Services.Admin;
using stocks.Services.AppUsers;
using stocks.Services.Computation;
using stocks.Services.Financials;
using stocks.Services.News;
using stocks.Services.Reputation;
using stocks.Services.Session;
using stocks.Services.Splits;
using STOCKS.Services.StockOverviews;
using stocks.Services.TimeSeries;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddIdentityCore<Appuser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<StocksContext>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_TOKEN")))
        };
    });
    
//builder.Services.AddAuthorization();

builder.Services.AddDbContext<StocksContext>(
    options => options.UseSqlServer(Environment.GetEnvironmentVariable("CONNECTION_STRING")));

// Add services to the container.

builder.Services.AddScoped<IAppUserRepository, AppUserRepository>();
builder.Services.AddScoped<IConnectionRepository, ConnectionRepository>();
builder.Services.AddScoped<IRepository<StockOverview>, StockOverviewRepository>();
builder.Services.AddScoped<IRepository<StockHistory>, StockHistoryRepository>();
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
builder.Services.AddScoped<IPolygonClient, PolygonClient>();
builder.Services.AddScoped<ISplitService, SplitService>();
builder.Services.AddScoped<IRepository<Split>, SplitRepository>();
builder.Services.AddScoped<IRepository<Reputation>, ReputationRepository>();
builder.Services.AddScoped<IOpenAiClient, OpenAiClient>();
builder.Services.AddScoped<IReputationService, ReputationService>();
builder.Services.AddScoped<IConnectionService, ConnectionService>();
builder.Services.AddScoped<IRepository<Financials>, FinancialsRepository>();
builder.Services.AddScoped<IFinnhubClient, FinnhubClient>();
builder.Services.AddScoped<IFinancialsService, FinancialsService>();
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

app.UseCors(opt =>
{
    //opt.AllowAnyHeader().AllowAnyMethod().WithOrigins($"http://{Environment.GetEnvironmentVariable("CLIENT_NAME")}:3000");
    opt.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
});

//app.UseAuthorization();

app.MapControllers();

app.Run();

