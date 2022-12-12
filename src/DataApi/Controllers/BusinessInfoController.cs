using DataApi.Strategies;
using Microsoft.AspNetCore.Mvc;

namespace DataApi.Controllers;

[ApiController]
[Route("/v1")]
public class BusinessInfoController
{
    private readonly IStrategyResolver _strategyResolver;

    public BusinessInfoController(IStrategyResolver strategyResolver)
    {
        _strategyResolver = strategyResolver;
    }

    [HttpGet("company/{jurisdiction_code}/{company_number}")]
    public async Task<IActionResult> Get(string jurisdiction_code, int company_number, CancellationToken cancellationToken)
    {
        var strategy = _strategyResolver.GetJurisdictionStrategy(jurisdiction_code, company_number);

        var response = await strategy.GetInfo(company_number, cancellationToken);

        return new ObjectResult(response) { StatusCode = (int)response.Status };
    }
}