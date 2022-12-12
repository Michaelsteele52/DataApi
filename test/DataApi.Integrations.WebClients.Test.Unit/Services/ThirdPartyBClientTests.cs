using System.Net;
using System.Net.Http.Json;
using AutoFixture;
using DataApi.Integrations.WebClients.ThirdPartyB;
using FluentAssertions;
using Hellang.Middleware.ProblemDetails;
using RichardSzalay.MockHttp;
using Xunit;

namespace DataApi.Integrations.WebClients.Test.Component.Services;

public class ThirdPartyBClientTests
{
    private readonly ThirdPartyBClient _sut;
    private readonly MockHttpMessageHandler _httpMessageHandler = new();
    
    public ThirdPartyBClientTests()
    {
        _sut = new ThirdPartyBClient(new HttpClient(_httpMessageHandler)
        {
            BaseAddress = new Uri("https://interview-df854r23.sikoia.com")
        });
    }
    
    [Fact]
    public async Task GetCompanyDataShouldReturnExpectedResponse()
    {
        //Arrange
        var content = new Fixture().Create<ThirdPartyBCompanyDataResponse>();
        var response = JsonContent.Create<ThirdPartyBCompanyDataResponse>(content);
        
        const string countryCode = "cc";
        const int companyId = 123;
        var url = $"https://interview-df854r23.sikoia.com/v1/company-data?jurisdictionCode={countryCode}&companyNumber={companyId}";
        _httpMessageHandler.When(url)
            .Respond(HttpStatusCode.OK, response);
        
        //Act
        var result = await _sut.GetCompanyData(countryCode, companyId, CancellationToken.None);

        //Assert
        result.Should().NotBeNull();
        result.CompanyName.Should().Be(content.CompanyName);
        //todo: add additional validations
    }

    [Theory]
    [InlineData(HttpStatusCode.InternalServerError, HttpStatusCode.BadRequest)]
    [InlineData(HttpStatusCode.Forbidden, HttpStatusCode.NotFound)]
    [InlineData(HttpStatusCode.NotFound, HttpStatusCode.NotFound)]
    [InlineData(HttpStatusCode.BadRequest, HttpStatusCode.NotFound)]
    public async Task GetCompanyDataShouldThrowProblemDetailsException(HttpStatusCode statusCodeResponse,
        HttpStatusCode expectedMappedResponseCode)
    {
        //Arrange
        var content = new Fixture().Create<ProblemDetailsException>();
        var response = JsonContent.Create<ProblemDetailsException>(content);
        
        const string countryCode = "cc";
        const int companyId = 123;
        var url = $"https://interview-df854r23.sikoia.com/v1/company-data?jurisdictionCode={countryCode}&companyNumber={companyId}";
        _httpMessageHandler.When(url)
            .Respond(statusCodeResponse, response);
        
        //Act & Assert
        var exceptionResponse = await Assert.ThrowsAsync<ProblemDetailsException>(() => _sut.GetCompanyData(countryCode, companyId, CancellationToken.None));

        exceptionResponse.Details.Status.Should().Be((int)expectedMappedResponseCode);
        //todo: add additional validations
    }
}