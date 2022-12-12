using System.Net;
using System.Net.Http.Json;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Mvc;

namespace DataApi.Integrations.WebClients.ThirdPartyA;

public class ThirdPartyAClient : IThirdPartyAClient
{
    private readonly HttpClient _httpClient;

    public ThirdPartyAClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ThirdPartyACompanyDataResponse> GetCompanyData(string countryCode, int companyId, CancellationToken cancellationToken)
    {
        var requestUri = $"v1/company/{countryCode}/{companyId}";

        var response = await _httpClient.GetAsync(requestUri, cancellationToken);

        if (!response.IsSuccessStatusCode)
            throw response.StatusCode switch
            {
                HttpStatusCode.InternalServerError => throw new ProblemDetailsException(
                    new ProblemDetails{
                        Detail = "Error calling ThirdPartyA, please try again later",
                        Status = (int)HttpStatusCode.InternalServerError,
                        Title = "InternalServerError"
                    }),
                HttpStatusCode.Forbidden => throw new ProblemDetailsException(
                    await MapForbiddenResponse(response, cancellationToken)),
                HttpStatusCode.NotFound => throw new ProblemDetailsException(
                    new ProblemDetails{
                        Detail = "Not Found",
                        Status = (int)HttpStatusCode.NotFound,
                        Title = "NotFound"
                    }),
                _ => throw new ProblemDetailsException(new ProblemDetails
                {
                    Detail = "Invalid Request.",
                    Status = (int)HttpStatusCode.BadRequest,
                    Title = "InvalidRequest"
                })
            };

        var companyDataResponse = await response.Content.ReadFromJsonAsync<ThirdPartyACompanyDataResponse>(cancellationToken: cancellationToken);

        return companyDataResponse ?? new ThirdPartyACompanyDataResponse();
    }

    private static async Task<ProblemDetails> MapForbiddenResponse(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        try
        {
            var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails?>(cancellationToken: cancellationToken);

            return new ProblemDetails
            {
                Detail = problemDetails?.Detail,
                Status = (int?)HttpStatusCode.BadRequest,
                Type = problemDetails?.Type,
                Title = problemDetails?.Title
            };
        }
        catch (Exception e)
        {
            //log deserialization failue
            return new ProblemDetails
            {
                Detail = "Company not found",
                Status = (int?)response.StatusCode,
                Title = "Not Found"
            };
        }
    }
}