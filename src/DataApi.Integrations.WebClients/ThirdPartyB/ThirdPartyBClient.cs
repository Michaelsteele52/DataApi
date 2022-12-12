using System.Net;
using System.Net.Http.Json;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace DataApi.Integrations.WebClients.ThirdPartyB;

public class ThirdPartyBClient : IThirdPartyBClient
{
    private readonly HttpClient _httpClient;

    public ThirdPartyBClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ThirdPartyBCompanyDataResponse> GetCompanyData(string countryCode, int companyId, CancellationToken cancellationToken)
    {
        var queryParams = new Dictionary<string, string?>
        {
            ["jurisdictionCode"] = countryCode,
            ["companyNumber"] = companyId.ToString()
        };

        var requestUri = QueryHelpers.AddQueryString("v1/company-data", queryParams);

        var response = await _httpClient.GetAsync(requestUri, cancellationToken);

        if (!response.IsSuccessStatusCode)
            throw response.StatusCode switch
            {
                HttpStatusCode.InternalServerError => throw new ProblemDetailsException(
                    new ProblemDetails{
                        Detail = $"Company:{companyId} not found at jurisdiction:{countryCode}",
                        Status = (int?)HttpStatusCode.BadRequest,
                        Title = "Invalid Request"
                    }),
                HttpStatusCode.NotFound => throw new ProblemDetailsException(
                    new ProblemDetails{
                        Detail = "Not Found",
                        Status = (int)HttpStatusCode.NotFound,
                        Title = "NotFound"
                    }),
                _ => throw new ProblemDetailsException( new ProblemDetails()
                {
                    Detail = $"Company:{companyId} not found at jurisdiction:{countryCode}",
                    Status = (int)HttpStatusCode.NotFound,
                    Title = "Not Found"
                })
            };

        var companyDataResponse = await response.Content.ReadFromJsonAsync<ThirdPartyBCompanyDataResponse>(cancellationToken: cancellationToken);

        return companyDataResponse ?? new ThirdPartyBCompanyDataResponse();
    }
}