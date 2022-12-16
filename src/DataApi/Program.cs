using DataApi.Configuration;
using DataApi.Extensions;
using DataApi.Integrations.WebClients.ThirdPartyA;
using DataApi.Integrations.WebClients.ThirdPartyB;
using DataApi.Mapper;
using DataApi.Strategies;
using Hellang.Middleware.ProblemDetails;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();

var companyIdConfiguration = config.GetSection("CompanyIdConfiguration");
var configuration = companyIdConfiguration.Get<CompanyIdConfiguration>();
builder.Services.AddSingleton(configuration);

builder.Services.AddScoped<IJurisdictionStrategy, UkStrategy>();
builder.Services.AddScoped<IJurisdictionStrategy, DeStrategy>();
builder.Services.AddScoped<IJurisdictionStrategy, NlStrategy>();
builder.Services.AddScoped<IStrategyResolver, JurisdictionStrategyResolver>();
builder.Services.AddScoped<IStrategyResponseMapper, StrategyResponseMapper>();

builder.Services.AddHttpClient<IThirdPartyAClient, ThirdPartyAClient, ThirdPartyAClientOptions>(config, "ThirdPartyA");
builder.Services.AddHttpClient<IThirdPartyBClient, ThirdPartyBClient, ThirdPartyBClientOptions>(config, "ThirdPartyB");

builder.Services.AddProblemDetails();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseProblemDetails();

app.UseExceptionMiddleware();

//todo: Validate Strategies on startup

app.UseAuthorization();

app.MapControllers();

app.Run();