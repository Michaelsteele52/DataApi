using System.Net;

namespace DataApi.Models.Response;

public class StrategyResponse
{
    public CompanyInfoResponse? CompanyInfoResponse { get; init; }
    public HttpStatusCode Status { get; init; } = HttpStatusCode.OK;
}