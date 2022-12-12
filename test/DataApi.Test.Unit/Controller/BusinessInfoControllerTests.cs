using DataApi.Controllers;
using DataApi.Models.Response;
using DataApi.Strategies;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace DataApi.Test.Unit.Controller;

public class BusinessInfoControllerTests
{
    private BusinessInfoController _businessInfoController;
    private Mock<IStrategyResolver> _strategyResolverMock;
    private Mock<IJurisdictionStrategy> _jurisdictionStrategyMock;

    public BusinessInfoControllerTests()
    {
        _strategyResolverMock = new Mock<IStrategyResolver>();
        _jurisdictionStrategyMock = new Mock<IJurisdictionStrategy>();
    }

    [Fact]
    public async Task ShouldResponseWithCompanyInfo()
    {
        //Arrange
        const int companyId = 123;

        _jurisdictionStrategyMock.Setup(x => x.GetInfo(companyId, It.IsAny<CancellationToken>())).ReturnsAsync(
            new StrategyResponse
            {
                CompanyInfoResponse = new CompanyInfoResponse()
                {
                    CompanyNumber = companyId.ToString()
                }
            });
        _strategyResolverMock.Setup(x => x.GetJurisdictionStrategy(It.IsAny<string>(), companyId)).Returns(_jurisdictionStrategyMock.Object);
        _businessInfoController = new BusinessInfoController(_strategyResolverMock.Object);

        //Act
        var result = await _businessInfoController.Get("uk", companyId, CancellationToken.None) as ObjectResult;

        //Assert
        result.Should().NotBeNull();
        result?.StatusCode.Should().Be(StatusCodes.Status200OK);
        result?.Value.Should().BeOfType<StrategyResponse>();
    }
}