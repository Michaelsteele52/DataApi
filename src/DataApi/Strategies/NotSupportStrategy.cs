using System.Net;
using DataApi.Models.Response;
using Microsoft.AspNetCore.Mvc;

namespace DataApi.Strategies;

public class NotSupportStrategy : IJurisdictionStrategy
{
    public int OrderNumber => (int)JurisdictionStrategyOrder.NotSupported;
    public bool IsSupported(string jurisdictionCode, int companyId) => true;

    public Task<StrategyResponse> GetInfo(int companyId, CancellationToken cancellationToken)
        => throw new JurisdictionNotSupportedException(new ProblemDetails
        {
            Detail = "This Jurisdiction is not supported",
            Status = (int)HttpStatusCode.UnprocessableEntity,
            Title = "This jurisdiction is not supported"
        });
}